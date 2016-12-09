using System.IO;

namespace CCSWE.IO
{
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
        #endregion
    }
}
