using System.Diagnostics;

namespace ScriptImage;

public class FindWindow
{
    //handle by window name
    //return window handle if not find return Zero
    public static IntPtr ByName(String WindowName)
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
    public static IntPtr ById(int windowId)
    {
        foreach (Process pList in Process.GetProcesses())
        {
            if (pList.Id == (windowId))
            {
                return pList.MainWindowHandle;
            }
        }
        return IntPtr.Zero;
    }

}
