using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class SenderUserFiles
    {
        // Class returns temp folder file paths.
        public static HashSet<string> PackageUserFiles(Dictionary<string, bool> keyValuePairs, CancellationToken cancellationToken)
        {
            HashSet<string> tempFilePaths = [];

            try
            {
                HashSet<string> filePaths = GatherFilePaths(keyValuePairs);

                foreach (var filePath in filePaths)
                {
                    // Path to a temporary zip folder.
                    string tempZipPath = Path.Combine(Path.GetTempPath(), "Temp_" + Path.GetFileNameWithoutExtension(filePath) + ".zip");

                    tempFilePaths.Add(tempZipPath);

                    // If temporary directory, exists then delete it
                    if (File.Exists(tempZipPath))
                    {
                        File.Delete(tempZipPath);
                        Console.WriteLine("Deleted tempzip");
                    }

                    // Create a zip folder and add files
                    using var archive = ZipFile.Open(tempZipPath, ZipArchiveMode.Create);
                    if (Directory.Exists(filePath))
                    {
                        Console.WriteLine(filePath);
                        AddDirectoryToZip(archive, filePath, Path.GetFileName(filePath), cancellationToken);
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in PackageUserFiles: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return tempFilePaths;
        }

        private static HashSet<string> GatherFilePaths(Dictionary<string, bool> keyValuePairs)
        {
            HashSet<string> filePaths = [];
            bool includeAllUserFiles = keyValuePairs.TryGetValue("Käyttäjän tiedostot", out bool includeAll) && includeAll;

            foreach (KeyValuePair<string, bool> entry in keyValuePairs)
            {
                if (entry.Value)
                {
                    if (includeAllUserFiles && IsUserFileCategory(entry.Key))
                    {
                        continue;
                    }
                    filePaths.UnionWith(GetSelectedFilePath(entry.Key));
                }
            }

            if (includeAllUserFiles)
            {
                filePaths.UnionWith(GetSelectedFilePath("Käyttäjän tiedostot"));
            }

            return filePaths;
        }

        private static bool IsUserFileCategory(string category)
        {
            return category switch
            {
                "Kuvat" => true,
                "Videot" => true,
                "Musiikki" => true,
                "Tiedostot" => true,
                "Ladatut tiedostot" => true,
                "Työpöytä" => true,
                _ => false,
            };
        }

        private static List<string> GetSelectedFilePath(string desiredFile)
        {
            // Can be optimized to return a switch statement
            List<string> strings = [];

            string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string videosFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            string musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string filesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            switch (desiredFile)
            {
                case "Käyttäjän tiedostot":
                    strings.AddRange([picturesFolder, videosFolder, musicFolder, filesFolder, downloadsFolder, desktopFolder]);
                    break;
                case "Kuvat":
                    strings.Add(picturesFolder);
                    break;
                case "Videot":
                    strings.Add(videosFolder);
                    break;
                case "Musiikki":
                    strings.Add(musicFolder);
                    break;
                case "Tiedostot":
                    strings.Add(filesFolder);
                    break;
                case "Ladatut tiedostot":
                    strings.Add(downloadsFolder);
                    break;
                case "Työpöytä":
                    strings.Add(desktopFolder);
                    break;
            }

            return strings;
        }
        public static void AddDirectoryToZip(ZipArchive archive, string sourceDir, string entryName, CancellationToken cancellationToken)
        {
            var files = SafeEnumerateFiles(sourceDir, "*", SearchOption.AllDirectories)
                   .Where(f => !f.EndsWith("desktop.ini", StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) break;

                    var relativePath = Path.GetRelativePath(sourceDir, file);
                    archive.CreateEntryFromFile(file, Path.Combine(entryName, relativePath));
                    Console.WriteLine($"Added file to zip: {file}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception adding file to zip: {ex.Message}");
                }
            }
        }

        private static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var directories = new Stack<string>();
            directories.Push(path);

            while (directories.Count > 0)
            {
                var currentDir = directories.Pop();
                IEnumerable<string> subDirs = Enumerable.Empty<string>();
                IEnumerable<string> files = Enumerable.Empty<string>();

                try
                {
                    subDirs = Directory.EnumerateDirectories(currentDir);
                    files = Directory.EnumerateFiles(currentDir, searchPattern);
                }
                catch (UnauthorizedAccessException) { }
                catch (DirectoryNotFoundException) { }
                catch (IOException) { }

                foreach (var file in files)
                {
                    yield return file;
                }

                if (searchOption == SearchOption.AllDirectories)
                {
                    foreach (var subDir in subDirs)
                    {
                        var dirInfo = new DirectoryInfo(subDir);
                        if (!dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            directories.Push(subDir);
                        }
                    }
                }
            }
        }
    }
}
