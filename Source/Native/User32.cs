using System;
using System.Runtime.InteropServices;

namespace CCSWE.Native
{
    /// <summary>
    /// Contains Native Windows API from user32.dll
    /// </summary>
    public static class User32
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// The GetClientRect function retrieves the coordinates of a window's client area. The client coordinates specify the upper-left and lower-right corners of the client area. Because client coordinates are relative to the upper-left corner of a window's client area, the coordinates of the upper-left corner are (0,0).
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window whose client coordinates are to be retrieved.</param>
        /// <param name="lpRect">[out] A <see cref="RECT"/> that receives the client coordinates. The left and top members are zero. The right and bottom members contain the width and height of the window.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect([In] IntPtr hWnd, [Out] RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, [In, Out] SCROLLINFO lpsi);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong([In] IntPtr hWnd, [In] int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, [Out] RECT rect);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, int wParam, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, [In, Out] BlittableStruct lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, [In, Out] CHARFORMAT2 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, [In, Out] FORMATRANGE lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, [In, Out] PARAFORMAT2 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, [In, Out] RECT lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent([In] IntPtr hWndChild, [In] IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetScrollInfo(IntPtr hwnd, UInt32 fnBar, [In, Out] SCROLLINFO lpsi, bool fRedraw);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowScrollBar(IntPtr hWnd, UInt32 wBar, bool bShow);
        // ReSharper restore InconsistentNaming
    }
}
