using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class ReceiverHTTP
    {
        //private static ConcurrentQueue<(string fileName, byte[] fileData)> fileQueue;
        private static bool continueConnection;
        private static Dictionary<string, string> operations = [];
        public static async Task Start(IPAddress senderIP, CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting HTTP server");
            var host = Dns.GetHostEntry(Dns.GetHostName());

            continueConnection = true;
            operations = [];

            // Make a firewall rule to allow only 1 specific ip to connect
            ManageFirewallRule.AddFirewallRule(senderIP.ToString());

            var ipv4Address = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            if (ipv4Address == null)
            {
                Console.WriteLine("No IPv4 address found.");
                return;
            }
            Console.WriteLine($"Host IPv4: {ipv4Address}");

            // Start listening on the computers own ip adress at port 8888
            HttpListener listener = new();
            string urlPrefix = $"http://{ipv4Address}:8888/";
            listener.Prefixes.Add(urlPrefix);
            listener.Start();
            Console.WriteLine($"Listening on {urlPrefix}");

            while (continueConnection)
            {
                // Look for requests and process them.
                HttpListenerContext context = await listener.GetContextAsync();
                await Task.Run(() => ProcessRequest(context, senderIP, cancellationToken));

                // Check if cancellation is requested.
                if(cancellationToken.IsCancellationRequested)
                {
                    listener.Stop();
                    listener.Close();
                    listener.Prefixes.Clear();

                    wifiExtractPath = "";
                    wifiFiles = [];
                    receivedWiFiFiles = false;

                    continueConnection = false;
                }
            }

            listener.Stop();
        }

        public static Dictionary<string, string> GetReceiverOperations()
        {
            return operations;
        }

        private static async Task ProcessRequest(HttpListenerContext context, IPAddress senderIP, CancellationToken cancellationToken)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // Check if the request to send data is from the correct ip address.
            if (!request.RemoteEndPoint.Address.Equals(senderIP))
            {
                Console.WriteLine($"Rejected connection from: {request.RemoteEndPoint.Address}");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Close();
                return;
            }

            // Only POST method is allowed
            if (request.HttpMethod == "POST")
            {
                if (request.Headers["Item-Type"] == "Confirmation")
                {
                    // If final confirmation, close the connection, remove firewall rule and export the WiFi profiles.
                    continueConnection = false;
                    Console.WriteLine("Received final confirmation. Stopping server. Do not close the program.");
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Close();

                    ManageFirewallRule.RemoveFirewallRule();

                    if(receivedWiFiFiles) WiFiProfileHandler.ExportWifiProfiles(wifiFiles, wifiExtractPath);
                }
                else
                {
                    Console.WriteLine("Received data.");

                    using MemoryStream ms = new();
                    await request.InputStream.CopyToAsync(ms, 81920, cancellationToken);

                    HandleReceivedFiles(request, ms);

                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Close();
                }

            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.Close();
            }
        }

        private static void HandleReceivedFiles(HttpListenerRequest request, MemoryStream ms)
        {
            // Check the item type header
            if (request.Headers["Item-Type"] != null)
            {
                string? folderName = request.Headers["Folder-Name"];
                if (folderName != null)
                {
                    // Use FileHandlerFactory to make the correct type of file handler.
                    var fileHandler = FileHandlerFactory.GetFileHandler(request.Headers["Item-Type"]);
                    fileHandler.HandleFile(folderName, ms.ToArray());
                }
                else
                {
                    Console.WriteLine("Folder-Name HTTP header is null");
                }
            }
        }

        public static class FileHandlerFactory
        {
            public static IFileHandler GetFileHandler(string itemType)
            {
                return itemType switch
                {
                    "UserFile" => new UserFileHandler(),
                    "WiFi-Profiles" => new WiFiProfileHandler(),
                    "Wallpaper" => new WallpaperHandler(),
                    "Selected-Folder" => new SelectedFolderFileHandler(),
                    "Browser" => new BrowserFileHandler(),
                    _ => throw new InvalidOperationException("Unknown item type")
                };
            }
        }

        public interface IFileHandler
        {
            void HandleFile(string fileName, byte[] fileData);
        }

        public class UserFileHandler : IFileHandler
        {
            public void HandleFile(string fileName, byte[] fileData)
            {
                try
                {
                    string tempZipPath = Path.Combine(Path.GetTempPath(), fileName);
                    if (File.Exists(tempZipPath)) File.Delete(tempZipPath);

                    File.WriteAllBytes(tempZipPath, fileData);
                    Console.WriteLine($"File written; {fileName}");

                    string[] fileNameParts = fileName.Split(["_"], StringSplitOptions.None);
                    string folderName = fileNameParts[1];
                    string destinationFolder = folderName switch
                    {
                        "Pictures" => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                        "Videos" => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                        "Music" => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                        "Documents" => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Downloads" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                        "Desktop" => Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        _ => throw new InvalidOperationException("Unknown folder name")
                    };
                    string extractPath = Path.Combine(destinationFolder, "Received files");

                    if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                
                    ZipFile.ExtractToDirectory(tempZipPath, extractPath);
                    File.Delete(tempZipPath);

                    Console.WriteLine($"Deleted received zip folder: {fileName}");

                    operations.Add(folderName, "Completed");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operations.Add(fileName, "Error");
                }

            }
        }

        private static string wifiExtractPath = "";
        private static string[] wifiFiles = [];
        private static bool receivedWiFiFiles = false;

        public class WiFiProfileHandler : IFileHandler
        {
            public void HandleFile(string fileName, byte[] fileData)
            {
                try
                {
                    receivedWiFiFiles = true;
                    string tempZipPath = Path.Combine(Path.GetTempPath(), fileName);
                    if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
                    File.WriteAllBytes(tempZipPath, fileData);

                    string extractPath = Path.Combine(Path.GetTempPath(),"ReceivedWiFiProfiles");
                    ZipFile.ExtractToDirectory(tempZipPath, extractPath);
                    File.Delete(tempZipPath);
                    Console.WriteLine($"Extracting received WiFi profiles to path {extractPath}");

                    string[] files = Directory.GetFiles(extractPath, "*.xml", SearchOption.AllDirectories);

                    wifiExtractPath = extractPath;
                    wifiFiles = files;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public static void ExportWifiProfiles(string[] files, string extractPath)
            {
                try
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine($"Exported profile {file}");
                        Process process = new()
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "netsh",
                                Arguments = $"wlan add profile filename=\"{file}",
                                RedirectStandardOutput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };

                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        Console.WriteLine(output);
                    }

                    Directory.Delete(extractPath, true);
                    Console.WriteLine("Deleted received WiFi profiles folder.");

                    Console.WriteLine("All WiFi profiles imported successfully.");

                    operations.Add("Wi-Fi yhteydet ja salasanat", "Completed");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operations.Add("Wi-Fi yhteydet ja salasanat", "Error");
                }
            }
        }

        public class WallpaperHandler : IFileHandler
        {
            private const int SPI_SETDESKWALLPAPER = 0x0014;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]

            private static extern int SystemParametersInfo(int uAction, int uParam, string pvParam, int fWinIni);
            public void HandleFile(string fileName, byte[] fileData)
            {
                try
                {
                    string wallpaperPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Wallpaper.png");
                    if (File.Exists(wallpaperPath)) File.Delete(wallpaperPath);

                    File.WriteAllBytes(wallpaperPath, fileData);

                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperPath, 0);

                    // Wait for a second. Doesn't work without this for some reason.
                    Thread.Sleep(1000);

                    Console.WriteLine("Wallpaper set");
                    operations.Add("Taustakuva", "Completed");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operations.Add("Taustakuva", "Completed");
                }

            }
        }

        public class BrowserFileHandler : IFileHandler
        {
            public void HandleFile(string fileName, byte[] fileData)
            {
                try
                {
                    string tempZipPath = Path.Combine(Path.GetTempPath(), fileName);
                    if (File.Exists(tempZipPath)) File.Delete(tempZipPath);

                    File.WriteAllBytes(tempZipPath, fileData);

                    string extractPath = Path.Combine(Path.GetTempPath(), "Browser Data");
                    if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);

                    ZipFile.ExtractToDirectory(tempZipPath, extractPath);
                    Console.WriteLine($"Extracting received browser data to: {extractPath}");

                    File.Delete(tempZipPath);
                    Console.WriteLine("Deleted temp browser zip");

                    Dictionary<string, string> browserDefaultPaths = new()
                    {
                        {"Chrome", GetBrowserPath("Chrome")},
                        {"Opera", GetBrowserPath("Opera")},
                        {"Edge", GetBrowserPath("Edge")},
                    };

                    //foreach (var browser in browserDefaultPaths)
                    //{
                    //    CopyReceivedFilesToBrowsers(browser, extractPath);
                    //}

                    BackupBrowserFolder(browserDefaultPaths);

                    operations.Add("Selainten tiedot", "Completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operations.Add("Selainten tiedot", "Error");
                }
            }

            private static string GetBrowserPath(string browser)
            {
                return browser switch
                {
                    "Chrome" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data", "Default"),
                    "Opera" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software", "Opera Stable", "Default"),
                    "Edge" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Edge", "User Data", "Default"),
                    //"Firefox" => Path.Combine(Environment.SpecialFolder.) Don't know yet, research where it stores data.
                };
            }

            private static void CopyReceivedFilesToBrowsers(KeyValuePair<string, string> browserPath, string extractPath)
            {


            }

            private static void BackupBrowserFolder(Dictionary<string, string> browserDefaultPaths)
            {
                string backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Browser default backup");

                if(!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                foreach (var path in browserDefaultPaths)
                {
                    var browserFiles = Directory.EnumerateFiles(path.Value, "*", SearchOption.AllDirectories);
                    
                    foreach (var file in browserFiles)
                    {
                        var relativePath = Path.GetRelativePath(path.Value, file);
                        File.Copy(file, Path.Combine(path.Key));
                    }

                    Console.WriteLine($"Backedup {path.Key} files");
                }
            }
        }

        public class SelectedFolderFileHandler : IFileHandler
        {
            public void HandleFile(string fileName, byte[] fileData)
            {
                try
                {
                    string tempZipPath = Path.Combine(Path.GetTempPath(), fileName);
                    if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
                    File.WriteAllBytes(tempZipPath, fileData);

                    Console.WriteLine($"File written: {fileName}");

                    string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Received selected folder");
                    if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);

                    ZipFile.ExtractToDirectory(tempZipPath, extractPath);

                    File.Delete(tempZipPath);

                    Console.WriteLine($"Deleted temp zip for file: {fileName}");

                    operations.Add("Valittu kansio", "Completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operations.Add("Valittu kansio", "Completed");
                }
            }
        }
    }
}
