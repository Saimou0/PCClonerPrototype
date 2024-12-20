using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class SenderChooseFolder
    {
        public static string PackageSelectedFolder(string selectedFolderPath, CancellationToken cancellationToken)
        {
            string tempZipPath = Path.Combine(Path.GetTempPath(), "SelectedFolder.zip");

            if(File.Exists(tempZipPath)) File.Delete(tempZipPath);

            using var archive = ZipFile.Open(tempZipPath, ZipArchiveMode.Create);
            if (Directory.Exists(selectedFolderPath))
            {
                Console.WriteLine($"Adding dir: {selectedFolderPath}");
                SenderUserFiles.AddDirectoryToZip(archive, selectedFolderPath, Path.GetFileName(selectedFolderPath), cancellationToken);
            } else
            {
                Console.WriteLine("Selected folder doesn't exist.");
            }

            return tempZipPath;
        }

        //public static void AddDirectoryToZip(ZipArchive archive, string sourceDir, string entryName)
        //{
        //    var files = SafeEnumerateFiles(sourceDir, "*", SearchOption.AllDirectories)
        //           .Where(f => !f.EndsWith("desktop.ini", StringComparison.OrdinalIgnoreCase));

        //    foreach (var file in files)
        //    {
        //        try
        //        {
        //            var relativePath = Path.GetRelativePath(sourceDir, file);
        //            archive.CreateEntryFromFile(file, Path.Combine(entryName, relativePath));
        //            Console.WriteLine($"Added file to zip: {file}");

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Exception adding file to zip: {ex.Message}");
        //        }
        //    }

        //}

        //private static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        //{
        //    var directories = new Stack<string>();
        //    directories.Push(path);

        //    while (directories.Count > 0)
        //    {
        //        var currentDir = directories.Pop();
        //        IEnumerable<string> subDirs = Enumerable.Empty<string>();
        //        IEnumerable<string> files = Enumerable.Empty<string>();

        //        try
        //        {
        //            subDirs = Directory.EnumerateDirectories(currentDir);
        //            files = Directory.EnumerateFiles(currentDir, searchPattern);
        //        }
        //        catch (UnauthorizedAccessException) { }
        //        catch (DirectoryNotFoundException) { }
        //        catch (IOException) { }

        //        foreach (var file in files)
        //        {
        //            yield return file;
        //        }

        //        if (searchOption == SearchOption.AllDirectories)
        //        {
        //            foreach (var subDir in subDirs)
        //            {
        //                var dirInfo = new DirectoryInfo(subDir);
        //                if (!dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
        //                {
        //                    directories.Push(subDir);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
