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
        public static void SafeDelete(FileInfo file)
        {
            if (file == null)
            {
                return;
            }

            try
            {
                if (file.IsReadOnly)
                {
                    file.IsReadOnly = false;
                }

                file.Delete();
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }
        }

        /// <summary>
        /// Safely deletes a files. Useful for situations where it would be nice if the file was deleted but it's ok if it isn't.
        /// </summary>
        /// <param name="path">The name of the file to be deleted.</param>
        public static void SafeDelete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                SafeDelete(new FileInfo(path));
            }
            catch (Exception)
            {
                // Move along nothing to see here
            }
        }
        #endregion
    }
}
