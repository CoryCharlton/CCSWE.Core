using System.IO;

namespace CCSWE.IO
{
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
        #endregion
    }
}
