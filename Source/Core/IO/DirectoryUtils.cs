using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace CCSWE.IO
{
    public static class DirectoryUtils
    {
        #region Public Methods
        /// <summary>
        /// Checks if a directory is empty.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns><c>true</c> if the directory contains no files or subdirectories</returns>
        public static bool IsEmpty(DirectoryInfo directory)
        {
            Contract.Requires<ArgumentNullException>(directory != null);

            return !(directory.EnumerateDirectories().Any() || directory.EnumerateFiles().Any());
        }

        /// <summary>
        /// Checks if a directory is empty.
        /// </summary>
        /// <param name="path">The directory to check.</param>
        /// <returns><c>true</c> if the directory contains no files or subdirectories</returns>
        public static bool IsEmpty(string path)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(path));

            return IsEmpty(new DirectoryInfo(path));
        }

        /// <summary>
        /// Safely deletes a directory. Useful for situations where it would be nice if the directory was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="directory">A <see cref="DirectoryInfo"/> representing the directory to be deleted.</param>
        /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void SafeDelete(DirectoryInfo directory, bool recursive = false)
        {
            if (directory == null)
            {
                return;
            }

            try
            {
                DirectoryInfoExtensions.RemoveAttributes(directory, FileAttributes.ReadOnly, recursive);
                directory.Delete(recursive);
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }
        }

        /// <summary>
        /// Safely deletes a directory. Useful for situations where it would be nice if the directory was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="path">The name of the directory to be deleted..</param>
        /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        public static void SafeDelete(string path, bool recursive = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                SafeDelete(new DirectoryInfo(path), recursive);
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }
        }
        #endregion
    }
}
