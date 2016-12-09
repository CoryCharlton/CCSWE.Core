using System;
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
            Ensure.IsNotNull(nameof(directory), directory);

            return !(directory.EnumerateDirectories().Any() || directory.EnumerateFiles().Any());
        }

        /// <summary>
        /// Checks if a directory is empty.
        /// </summary>
        /// <param name="path">The directory to check.</param>
        /// <returns><c>true</c> if the directory contains no files or subdirectories</returns>
        public static bool IsEmpty(string path)
        {
            Ensure.IsNotNullOrWhitespace(nameof(path), path);

            return IsEmpty(new DirectoryInfo(path));
        }

        /// <summary>
        /// Safely deletes a directory. Useful for situations where it would be nice if the directory was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="directory">A <see cref="DirectoryInfo"/> representing the directory to be deleted.</param>
        /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        /// <returns><c>true</c> if the directory was deleted</returns>
        public static bool SafeDelete(DirectoryInfo directory, bool recursive = false)
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

        /// <summary>
        /// Safely deletes a directory. Useful for situations where it would be nice if the directory was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="path">The name of the directory to be deleted..</param>
        /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
        /// <returns><c>true</c> if the directory was deleted</returns>
        public static bool SafeDelete(string path, bool recursive = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                return SafeDelete(new DirectoryInfo(path), recursive);
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
