using System;
using System.IO;
using System.Linq;

namespace CCSWE.IO
{
    /// <summary>
    /// Extension methods for <see cref="DirectoryInfo"/>;
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        #region Public Methods
        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a directory.
        /// </summary>
        /// <param name="directoryInfo">The directory to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        public static void AddAttributes(this DirectoryInfo directoryInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(directoryInfo), directoryInfo);

            directoryInfo.AddAttributes(attributes, false);
        }

        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a directory.
        /// </summary>
        /// <param name="directoryInfo">The directory to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        /// <param name="recursive"><c>true</c> to add attributes from this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void AddAttributes(this DirectoryInfo directoryInfo, FileAttributes attributes, bool recursive)
        {
            Ensure.IsNotNull(nameof(directoryInfo), directoryInfo);

            ((FileSystemInfo)directoryInfo).AddAttributes(attributes, recursive);
        }

        /// <summary>
        /// Checks if a directory is empty.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns><c>true</c> if the directory contains no files or subdirectories</returns>
        public static bool IsEmpty(this DirectoryInfo directory)
        {
            Ensure.IsNotNull(nameof(directory), directory);

            return !(directory.EnumerateDirectories().Any() || directory.EnumerateFiles().Any());
        }

        /// <summary>
        /// Removes the specified <see cref="FileAttributes"/> from a directory.
        /// </summary>
        /// <param name="directoryInfo">The directory to remove <see cref="FileAttributes"/> from.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to remove.</param>
        public static void RemoveAttributes(this DirectoryInfo directoryInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(directoryInfo), directoryInfo);

            directoryInfo.RemoveAttributes(attributes, false);
        }

        /// <summary>
        /// Removes the specified <see cref="FileAttributes"/> from a directory.
        /// </summary>
        /// <param name="directoryInfo">The directory to remove <see cref="FileAttributes"/> from.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to remove.</param>
        /// <param name="recursive"><c>true</c> to remove attributes from this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void RemoveAttributes(this DirectoryInfo directoryInfo, FileAttributes attributes, bool recursive)
        {
            Ensure.IsNotNull(nameof(directoryInfo), directoryInfo);

            ((FileSystemInfo)directoryInfo).RemoveAttributes(attributes, recursive);
        }

        /// <summary>
        /// Safely deletes a directory. Useful for situations where it would be nice if the directory was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="directory">A <see cref="DirectoryInfo"/> representing the directory to be deleted.</param>
        /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        /// <returns><c>true</c> if the directory was deleted</returns>
        public static bool SafeDelete(this DirectoryInfo directory, bool recursive = false)
        {
            if (directory == null)
            {
                return false;
            }

            try
            {
                directory.RemoveAttributes(FileAttributes.ReadOnly, recursive);
                directory.Delete(recursive);

                return true;
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }

            return false;
        }
        #endregion
    }
}
