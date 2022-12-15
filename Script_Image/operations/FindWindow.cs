using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ScriptImage
{
    public class FindWindow : DllHolder
    {
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
    }
}








