using System;
using System.IO;

namespace CCSWE.IO
{
    /// <summary>
    /// Helper methods for file operations
    /// </summary>
    public static class FileUtils
    {
        #region Public Methods
        /// <summary>
        /// Safely deletes a file. Useful for situations where it would be nice if the file was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="path">The name of the file to be deleted.</param>
        /// <returns><c>true</c> if the file was deleted</returns>
        public static bool SafeDelete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                return new FileInfo(path).SafeDelete();
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }

            return false;
        }

        /// <summary>
        /// Safely moves a file. Useful for situations where it would be nice if the file was moved but it's ok if it isn't.
        /// </summary>
        /// <param name="sourcePath">The name of the file to move.</param>
        /// <param name="destinationPath">The new path for the file.</param>
        /// <returns><c>true</c> if the file was moved.</returns>
        public static bool SafeMove(string sourcePath, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                return false;
            }

            try
            {
                var destinationDirectory = Path.GetDirectoryName(destinationPath);

                if (string.IsNullOrWhiteSpace(destinationDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                File.Move(sourcePath, destinationPath);

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
