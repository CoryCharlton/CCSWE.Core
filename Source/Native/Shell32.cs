using System;
using System.Runtime.InteropServices;

namespace CCSWE.Native
{
    public static class Shell32
    {
        // ReSharper disable InconsistentNaming
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        // ReSharper restore InconsistentNaming
    }
}
