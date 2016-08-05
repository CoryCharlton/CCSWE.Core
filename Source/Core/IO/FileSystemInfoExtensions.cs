using System.Collections.Generic;
using System.IO;

namespace CCSWE.IO
{
    internal static class FileSystemInfoExtensions
    {
        #region Private Methods
        public static void AddAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes)
        {
            fileSystemInfo.AddAttributes(attributes, false);
        }

        public static void AddAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes, bool recursive)
        {
            fileSystemInfo.Refresh();

            if (recursive && (fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var directoryQueue = new Queue<string>();
                var fileQueue = new Queue<string>();

                directoryQueue.Enqueue(fileSystemInfo.FullName);

                while (directoryQueue.Count > 0)
                {
                    var currentDirectory = directoryQueue.Dequeue();

                    foreach (var directory in Directory.GetDirectories(currentDirectory))
                    {
                        directoryQueue.Enqueue(directory);
                    }

                    foreach (var file in Directory.GetFiles(currentDirectory))
                    {
                        fileQueue.Enqueue(file);
                    }

                    new DirectoryInfo(currentDirectory).Attributes |= attributes;
                }

                while (fileQueue.Count > 0)
                {
                    new FileInfo(fileQueue.Dequeue()).Attributes |= attributes;
                }
            }
            else
            {
                fileSystemInfo.Attributes |= attributes;
            }
        }

        public static void RemoveAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes)
        {
            fileSystemInfo.RemoveAttributes(attributes, false);
        }

        public static void RemoveAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes, bool recursive)
        {
            fileSystemInfo.Refresh();

            if (recursive && (fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var directoryQueue = new Queue<string>();
                var fileQueue = new Queue<string>();

                directoryQueue.Enqueue(fileSystemInfo.FullName);

                while (directoryQueue.Count > 0)
                {
                    var currentDirectory = directoryQueue.Dequeue();

                    foreach (var directory in Directory.GetDirectories(currentDirectory))
                    {
                        directoryQueue.Enqueue(directory);
                    }

                    foreach (var file in Directory.GetFiles(currentDirectory))
                    {
                        fileQueue.Enqueue(file);
                    }

                    new DirectoryInfo(currentDirectory).Attributes &= ~attributes;
                }

                while (fileQueue.Count > 0)
                {
                    new FileInfo(fileQueue.Dequeue()).Attributes &= ~attributes;
                }
            }
            else
            {
                fileSystemInfo.Attributes &= ~attributes;
            }
        }
        #endregion
    }
}
