using System;
using System.IO;

namespace CCSWE.IO
{
    public static class FileUtils
    {
        #region Public Methods
        /// <summary>
        /// Safely deletes a files. Useful for situations where it would be nice if the file was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="file">A <see cref="FileInfo"/> representing the file to be deleted.</param>
        /// <returns><c>true</c> if the file was deleted</returns>
        public static bool SafeDelete(FileInfo file)
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
        /// Safely deletes a files. Useful for situations where it would be nice if the file was deleted but it's ok if it isn't.
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
                return SafeDelete(new FileInfo(path));
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
