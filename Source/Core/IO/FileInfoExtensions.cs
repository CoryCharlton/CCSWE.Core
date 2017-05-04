using System;
using System.IO;
using System.Threading;

namespace CCSWE.IO
{
    /// <summary>
    /// Extension methods for <see cref="FileInfo"/>;
    /// </summary>
    public static class FileInfoExtensions
    {
        #region Public Methods
        /// <summary>
        /// Adds the specified <see cref="FileAttributes"/> to a file.
        /// </summary>
        /// <param name="fileInfo">The file to add <see cref="FileAttributes"/> to.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to add.</param>
        public static void AddAttributes(this FileInfo fileInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(fileInfo), fileInfo);

            ((FileSystemInfo) fileInfo).AddAttributes(attributes);
        }

        /// <summary>
        /// Removes the specified <see cref="FileAttributes"/> from a file.
        /// </summary>
        /// <param name="fileInfo">The file to remove <see cref="FileAttributes"/> from.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to remove.</param>
        public static void RemoveAttributes(this FileInfo fileInfo, FileAttributes attributes)
        {
            Ensure.IsNotNull(nameof(fileInfo), fileInfo);

            ((FileSystemInfo) fileInfo).RemoveAttributes(attributes);
        }

        /// <summary>
        /// Safely deletes a files. Useful for situations where it would be nice if the file was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="file">A <see cref="FileInfo"/> representing the file to be deleted.</param>
        /// <returns><c>true</c> if the file was deleted</returns>
        public static bool SafeDelete(this FileInfo file)
        {
            if (file == null)
            {
                return false;
            }

            try
            {
                file.RemoveAttributes(FileAttributes.ReadOnly);
                file.Delete();

                return true;
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }

            return false;
        }

        /// <summary>
        /// Converts the size of a file to a user friendly string representation.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> that contains the length.</param>
        /// <returns>A user friendly string representation of the number of bytes.</returns>
        public static string ToFileSizeString(this FileInfo file)
        {
            return FileUtils.ToFileSizeString(file.Length);
        }
        #endregion
    }
}
