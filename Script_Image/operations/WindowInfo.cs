using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace ScriptImage
{
    public class WindowInfo : DllHolder
    {
        #region FindWindow
        //handle by window Title
        //return window handle if not find return Zero
        public static IntPtr ByTitle(String WindowName)
        {
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle == (WindowName))
                {
                    return pList.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        //handle by window id Process
        //return window handle if not find return Zero
        public static IntPtr ProcessId(int ProcessId)
        {
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.Id == (ProcessId))
                {
                    return pList.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        //handle by window Process Name
        //return window handle if not find return Zero
        public static IntPtr ProcessName(String ProcessName)
        {
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.ProcessName == (ProcessName))
                {
                    return pList.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        //Get all window child by parent handle.
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                {
                    listHandle.Free();
                }
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }



        //return Window Title by handle
        public static string GetWindowTitle(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }
        #endregion

        #region WindowCapture
        //From https://stackoverflow.com/a/46174804/20574919
        //Capture specific Window by using handle
        //Return Mat
        public static Mat Capture(IntPtr hWnd, TernaryRasterOperations TernaryRasterOperations = TernaryRasterOperations.SRCCOPY)
        {
            var rect = new Rect();
            GetClientRect(hWnd, ref rect);

            var point = new System.Drawing.Point(0, 0);
            ClientToScreen(hWnd, ref point);

            var bounds = new Rectangle(point.X, point.Y, rect.Right, rect.Bottom);


            IntPtr hWndDc = GetDC(hWnd);
            IntPtr hMemDc = CreateCompatibleDC(hWndDc);
            IntPtr hBitmap = CreateCompatibleBitmap(hWndDc, bounds.Width, bounds.Height);
            SelectObject(hMemDc, hBitmap);

            BitBlt(hMemDc, 0, 0, bounds.Width, bounds.Height, hWndDc, 0, 0, TernaryRasterOperations);
            using (Bitmap bitmap = Bitmap.FromHbitmap(hBitmap))
            {
                DeleteObject(hBitmap);
                ReleaseDC(hWnd, hWndDc);
                //ReleaseDC(IntPtr.Zero, hMemDc);
                DeleteDC(hMemDc);

                return bitmap.ToMat();
            }
        }


        //Capture Window image by location using handle
        public static Mat CaptureForeground(IntPtr handle)
        {
            var rect = new Rect();
            GetClientRect(handle, ref rect);

            var point = new System.Drawing.Point(0, 0);
            ClientToScreen(handle, ref point);

            var bounds = new Rectangle(point.X, point.Y, rect.Right, rect.Bottom);

            using (var result = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.CopyFromScreen(point.X, point.Y, 0, 0, result.Size);
                }
                return result.ToMat();

            }

        }

        //function to Bring Window To front
        public static bool BringWindowToTop(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                return SetForegroundWindow((IntPtr)handle);
            }
            return false;
        }

        #endregion

        #region FPS
        private static DateTime _lastCheckTime = DateTime.Now;
        private static long _frameCount = 0;


        private static DateTime LastCheckTime
        {
            get { return _lastCheckTime; }
            set { _lastCheckTime = value; }
        }

        // called whenever a map is updated
        private static void OnMapUpdated()
        {
            Interlocked.Increment(ref _frameCount);
        }

        // called every once in a while
        public static double GetFps()
        {
            OnMapUpdated();

            double secondsElapsed = (DateTime.Now - LastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            LastCheckTime = DateTime.Now;
            return fps;
        }
        #endregion

    }
}








