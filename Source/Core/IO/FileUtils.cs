using System;
using System.IO;

namespace CCSWE.IO
{
    /// <summary>
    /// Helper methods for file operations
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// A constant that represents the number of bytes in a kilobyte
        /// </summary>
        public const long Kilobyte = 1024L;

        /// <summary>
        /// A constant that represents the number of bytes in a megabyte
        /// </summary>
        public const long Megabyte = 1024L * 1024;

        /// <summary>
        /// A constant that represents the number of bytes in a gigabyte
        /// </summary>
        public const long Gigabyte = 1024L * 1024 * 1024;

        /// <summary>
        /// A constant that represents the number of bytes in a terabyte
        /// </summary>
        public const long Terabyte = 1024L * 1024 * 1024 * 1024;

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

        /// <summary>
        /// Converts the number of bytes to a user friendly string representation.
        /// </summary>
        /// <param name="fileSize">The size of the file.</param>
        /// <returns>A user friendly string representation of the number of bytes.</returns>
        public static string ToFileSizeString(long fileSize)
        {
            var absoluteFileSize = Math.Abs(fileSize);
            string stringFileSize;

            if (absoluteFileSize > Terabyte)
            {
                stringFileSize = (fileSize / (double) Terabyte).ToString("N2") + " TB";
            }
            else if (absoluteFileSize > Gigabyte)
            {
                stringFileSize = (fileSize / (double) Gigabyte).ToString("N2") + " GB";
            }
            else if (absoluteFileSize > Megabyte)
            {
                stringFileSize = (fileSize / (double) Megabyte).ToString("N2") + " MB";
            }
            else if (absoluteFileSize > Kilobyte)
            {
                stringFileSize = (fileSize / (double) Kilobyte).ToString("N2") + " KB";
            }
            else
            {
                stringFileSize = fileSize.ToString("N2") + " B";
            }

            return stringFileSize;
        }
        #endregion
    }
}
