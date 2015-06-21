using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Interop;

namespace CCSWE
{
    /// <summary>
    /// Adapted from System.Windows.Forms.Screen
    /// </summary>
    // ReSharper disable InconsistentNaming
    public class Screen
    {
        #region Native Code
        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors (HandleRef hdc, IntPtr rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out]MONITORINFOEX info);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

        private delegate bool MonitorEnumProc (IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        private class MONITORINFOEX
        {
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            internal RECT rcMonitor = new RECT();
            internal RECT rcWork = new RECT();
            internal int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const int PRIMARY_MONITOR = -1163005939;
        private const int MONITOR_DEFAULTTONULL = 0;
        private const int MONITOR_DEFAULTTOPRIMARY = 1;
        private const int MONITOR_DEFAULTTONEAREST = 2;
        private const int MONITORINFOF_PRIMARY = 1;
        #endregion

        #region Constructor
        internal Screen(IntPtr monitor): this(monitor, IntPtr.Zero)
        {
        }

        internal Screen(IntPtr monitor, IntPtr hdc)
        {
            var info = new MONITORINFOEX();
            GetMonitorInfo(new HandleRef(null, monitor), info);

            Bounds = RectFromLTRB(info.rcMonitor.left, info.rcMonitor.top, info.rcMonitor.right, info.rcMonitor.bottom);
            WorkingArea = RectFromLTRB(info.rcWork.left, info.rcWork.top, info.rcWork.right, info.rcWork.bottom);
            IsPrimary = ((info.dwFlags & MONITORINFOF_PRIMARY) != 0);
            Name = new string(info.szDevice).TrimEnd((char)0);
        }        
        #endregion

        #region Private Static Fields
        //private static bool multiMonitorSupport = UnsafeNativeMethods.GetSystemMetrics(80) != 0;
        private static List<Screen> Screens;
        #endregion

        #region Public Static Fields
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);        
        #endregion

        #region Public Properties
        public static IEnumerable<Screen> AllScreens
        {
            get
            {
                if (Screens == null)
                {
                    //if (Screen.multiMonitorSupport)
                    //{

                    var monitorEnumCallback = new MonitorEnumCallback();
                    var lpfnEnum = new MonitorEnumProc(monitorEnumCallback.Callback);
                    EnumDisplayMonitors(NullHandleRef, IntPtr.Zero, lpfnEnum, IntPtr.Zero);

                    if (monitorEnumCallback.Screens.Count > 0)
                    {
                        Screens = monitorEnumCallback.Screens.ToList();
                    }
                    else
                    {
                        //Screen.screens = new Screen[1]
                        //{
                        //    new Screen((IntPtr) - 1163005939)
                        //};
                    }
                    //}
                    //else
                    //{
                    //    Screen.screens = new Screen[1]
                    //    {
                    //        Screen.PrimaryScreen
                    //    };
                    //}
                    //SystemEvents.DisplaySettingsChanging += new EventHandler(Screen.OnDisplaySettingsChanging);
                }

                return Screens;
            }
        }

        public Rect Bounds { get; private set; }

        public bool IsPrimary { get; private set; }

        public string Name { get; private set; }

        //public static Screen PrimaryScreen
        //{
        //    get
        //    {
        //        if (!Screen.multiMonitorSupport)
        //            return new Screen((IntPtr) - 1163005939, IntPtr.Zero);
        //        Screen[] allScreens = Screen.AllScreens;
        //        for (int index = 0; index < allScreens.Length; ++index)
        //        {
        //            if (allScreens[index].primary)
        //                return allScreens[index];
        //        }
        //        return (Screen)null;
        //    }
        //}

        public Rect WorkingArea { get; private set; }        
        #endregion

        #region Private Methods
        private static Rect RectFromLTRB(int left, int top, int right, int bottom)
        {
            return new Rect(left, top, right - left, bottom - top);
        }
        #endregion

        #region Public Methods
        public static Screen FromWindow(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);

            //if (Screen.multiMonitorSupport)
            //{
            //return new Screen(SafeNativeMethods.MonitorFromWindow(new HandleRef((object)null, hwnd), 2));
            return new Screen(MonitorFromWindow(new HandleRef((object)null, windowInteropHelper.Handle), 2));
            //}

            //return new Screen((IntPtr) PRIMARY_MONITOR, IntPtr.Zero);
        }
        #endregion

        #region MonitorEnumCallback
        private class MonitorEnumCallback
        {
            public List<Screen> Screens = new List<Screen>();

            public bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam)
            {
                Screens.Add(new Screen(monitor, hdc));
                return true;
            }
        }
	    #endregion
    }
}
