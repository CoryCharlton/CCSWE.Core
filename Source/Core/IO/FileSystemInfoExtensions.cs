using System.Collections.Generic;
using System.IO;

namespace CCSWE.IO
{
    internal static class FileSystemInfoExtensions
    {
        #region Private Methods
        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a directory.
        /// </summary>
        /// <param name="fileSystemInfo">The directory or file to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        public static void AddAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(fileSystemInfo), fileSystemInfo);

            fileSystemInfo.AddAttributes(attributes, false);
        }

        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a directory.
        /// </summary>
        /// <param name="fileSystemInfo">The directory or file to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        /// <param name="recursive"><c>true</c> to add attributes from this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void AddAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes, bool recursive)
        {
            Ensure.IsNotNull(nameof(fileSystemInfo), fileSystemInfo);

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

        /// <summary>
        /// Removes the specified <see cref="FileAttributes"/> from a file.
        /// </summary>
        /// <param name="fileSystemInfo">The directory or file to remove <see cref="FileAttributes"/> from.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to remove.</param>
        public static void RemoveAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(fileSystemInfo), fileSystemInfo);

            fileSystemInfo.RemoveAttributes(attributes, false);
        }

        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a directory.
        /// </summary>
        /// <param name="fileSystemInfo">The directory or file to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        /// <param name="recursive"><c>true</c> to add attributes from this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void RemoveAttributes(this FileSystemInfo fileSystemInfo, FileAttributes attributes, bool recursive)
        {
            Ensure.IsNotNull(nameof(fileSystemInfo), fileSystemInfo);

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
