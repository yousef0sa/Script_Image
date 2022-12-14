using System;
using System.Diagnostics;

namespace ScriptImage
{
    public class FindWindow
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
    }
}