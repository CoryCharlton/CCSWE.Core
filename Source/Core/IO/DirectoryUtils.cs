using System;
using System.IO;
using System.Linq;

namespace CCSWE.IO
{
    /// <summary>
    /// Extension methods for <see cref="DirectoryInfo"/>.
    /// </summary>
    public static class DirectoryUtils
    {
        #region Public Methods
        /// <summary>
        /// Checks if a directory is empty.
        /// </summary>
        /// <param name="path">The directory to check.</param>
        /// <returns><c>true</c> if the directory contains no files or subdirectories</returns>
        public static bool IsEmpty(string path)
        {
            Ensure.IsNotNullOrWhitespace(nameof(path), path);

            return new DirectoryInfo(path).IsEmpty();
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
                return new DirectoryInfo(path).SafeDelete(recursive);
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
