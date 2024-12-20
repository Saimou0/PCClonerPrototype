using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;

namespace Sender
{
    public class SenderHTTP
    {
        private static readonly HttpClient client = new();
        private static Dictionary<string, string> operations;
        public static async Task Start(IPEndPoint clientEP, Dictionary<string, bool> keyValuePairs, string selectedFolder, CancellationToken cancellationToken)
        {
            operations = [];
            string serverUrl = $"http://{clientEP.Address}:{clientEP.Port}/";

            try
            {
                // The code in this class is awful.

                // User files
                var userFileKeyValuePairs = GetUserFilesKeyValuePairs(keyValuePairs);
                HashSet<string> packagedUserFiles = SenderUserFiles.PackageUserFiles(userFileKeyValuePairs, cancellationToken);

                if (packagedUserFiles.Count != 0 && !cancellationToken.IsCancellationRequested)
                {
                    await SendUserFiles(packagedUserFiles, serverUrl, cancellationToken);
                }

                if (keyValuePairs["Työpöydän taustakuva"] == true && !cancellationToken.IsCancellationRequested)
                {
                    await SendWallpaper(SenderWallpaper.GetCurrentDesktopBackground(), serverUrl, cancellationToken);
                }

                if (keyValuePairs["Selainten kirjanmerkit ja muut tiedot"] == true && !cancellationToken.IsCancellationRequested)
                {
                    string tempZipPath = SenderBrowserData.PackageBrowserData(cancellationToken);
                    await SendBrowserFiles(tempZipPath, serverUrl, cancellationToken);
                    //operations.Add("Selain tiedot", "Completed");
                }

                if (keyValuePairs["Valitse kansio"] == true && !cancellationToken.IsCancellationRequested)
                {
                    if (selectedFolder != "")
                    {
                        string tempZipPath = SenderChooseFolder.PackageSelectedFolder(selectedFolder, cancellationToken);
                        await SendSelectedFolder(tempZipPath, serverUrl, cancellationToken);
                    }
                }

                if (keyValuePairs["Wi-Fi yhteydet ja salasanat"] == true && !cancellationToken.IsCancellationRequested)
                {
                    string tempZipPath = SenderWiFi.PackageWiFiProfiles(cancellationToken);
                    await SendWiFiProfiles(tempZipPath, serverUrl, cancellationToken);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stacktrace: {ex.StackTrace}");
            }
            finally
            {
                await SendFinalConfirmation(serverUrl);
            }
        }

        public static Dictionary<string, string> GetSenderOperations()
        {
            return operations;
        }

        private static async Task SendUserFiles(HashSet<string> tempFilePaths, string serverUrl, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var filePath in tempFilePaths)
                {
                    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using var content = new StreamContent(fileStream, 81920);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

                    content.Headers.Add("Item-Type", "UserFile");
                    content.Headers.Add("Folder-Name", Path.GetFileNameWithoutExtension(filePath));

                    Console.WriteLine($"Sending: {Path.GetFileName(filePath)} to: {serverUrl}");

                    HttpResponseMessage response = await client.PostAsync(serverUrl, content, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File sent successfully");

                        content.Dispose();
                        fileStream.Dispose();

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Console.WriteLine("Deleted temp zip file.");

                            string pathName = Path.GetFileNameWithoutExtension(filePath);
                            string[] pathNameHalves = pathName.Split("_");
                            operations[pathNameHalves[1]] = "Completed";
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                        }
                    }
                    else
                    {
                        operations[Path.GetFileNameWithoutExtension(filePath)] = "Not sent";

                        if (cancellationToken.IsCancellationRequested)
                        {
                            Console.WriteLine("User files cancellation requested.");
                        }
                        else
                        {
                            Console.WriteLine("User files not sent successfully");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                operations.Add("Käyttäjä tiedostot", "Error");
            }
            finally
            {
                operations.Add("Käyttäjä tiedostot", "Completed");
            }
        }

        private static Dictionary<string, bool> GetUserFilesKeyValuePairs(Dictionary<string, bool> keyValuePairs)
        {
            var userFilesKeys = new HashSet<string>
            {
                "Kuvat",
                "Videot",
                "Musiikki",
                "Tiedostot",
                "Ladatut tiedostot",
                "Työpöytä",
                "Käyttäjän tiedostot"
            };

            return keyValuePairs
                .Where(kvp => userFilesKeys.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        private static async Task SendWiFiProfiles(string filePath ,string serverUrl, CancellationToken cancellationToken)
        {

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var content = new StreamContent(fileStream, 81920);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

            content.Headers.Add("Item-Type", "WiFi-Profiles");
            content.Headers.Add("Folder-Name", "WiFi");

            Console.WriteLine($"Sending WiFi profiles to: {serverUrl}");

            HttpResponseMessage response = await client.PostAsync(serverUrl, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("File sent successfully");

                content.Dispose();
                fileStream.Dispose();

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Deleted temp WiFi zip file.");
                    operations.Add("Wi-Fi yhteydet ja salasanat", "Completed");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            else
            {
                operations.Add("Wi-Fi yhteydet ja salasanat", "Not sent");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("WiFi profiles cancellation requested.");
                }
                else
                {
                    Console.WriteLine("WiFi profiles not sent successfully");
                }
            }

        }

        private static async Task SendWallpaper(string filePath, string serverUrl, CancellationToken cancellationToken)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var content = new StreamContent(fileStream, 81920);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Headers.Add("Item-Type", "Wallpaper");
            content.Headers.Add("Folder-Name", "Wallpaper");

            Console.WriteLine($"Sending wallpaper to: {serverUrl}");

            HttpResponseMessage response = await client.PostAsync(serverUrl, content, cancellationToken);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Wallpaper sent successfully");
                operations.Add("Taustakuva", "Completed");

                content.Dispose();
                fileStream.Close();
            }
            else
            {
                operations.Add("Taustakuva","Not sent");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Wallpaper cancellation requested.");
                }
                else
                {
                    Console.WriteLine("Wallpaper not sent successfully");
                }
            }
        }

        private static async Task SendBrowserFiles(string filePath, string serverUrl, CancellationToken cancellationToken)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var content = new StreamContent(fileStream, 81920);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

            content.Headers.Add("Item-Type", "Browser");
            content.Headers.Add("Folder-Name", "Browser-Files");

            Console.WriteLine($"Sending browser files to: {serverUrl}");

            HttpResponseMessage response = await client.PostAsync(serverUrl, content, cancellationToken);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Browser files sent successfully.");

                content.Dispose();
                fileStream.Close();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Deleted temp browser zip file.");
                    operations.Add("Selainten tiedot", "Completed");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            } 
            else
            {
                operations.Add("Selainten tiedot", "Error");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Browser files cancellation requested.");
                } else
                {
                    Console.WriteLine("Browser files not sent successfully");
                }
            }
        }

        private static async Task SendSelectedFolder(string filePath, string serverUrl, CancellationToken cancellationToken)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var content = new StreamContent(fileStream, 81920);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

            content.Headers.Add("Item-Type", "Selected-Folder");
            content.Headers.Add("Folder-Name", "Selected-Folder");

            Console.WriteLine($"Sending selected folder to: {serverUrl}");

            HttpResponseMessage response = await client.PostAsync(serverUrl, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Selected folder sent successfully.");

                content.Dispose();
                fileStream.Close();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Deleted temp selected folder zip file.");
                    operations.Add("Valittu kansio", "Completed");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            else
            {
                operations.Add("Valittu kansio", "Not sent");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Selected folder cancellation requested.");
                }
                else
                {
                    Console.WriteLine("Selected folder not sent successfully");
                }
            }
        }

        private static async Task SendFinalConfirmation(string serverUrl)
        {
            var content = new StringContent("Sending complete");
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            content.Headers.Add("Item-Type", "Confirmation");
            Console.WriteLine($"Sending final confirmation to: {serverUrl}");

            HttpResponseMessage response = await client.PostAsync(serverUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Final confirmation sent successfully!");
            }
            else
            {
                Console.WriteLine($"Error sending final confirmation: {response.StatusCode}");
            }
        }


    }
}
