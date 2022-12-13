
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace ScriptImage
{
    public class Mouse : DllHolder
    {
        public enum WMessages : uint
        {
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205, //Right mousebutton up
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202, //Left mousebutton up   
        }

        public static void Right_Click(IntPtr hWnd, (int x,int y) Location,  double delayTime = 1)
        {

            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.x, Location.y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONUP, IntPtr.Zero, MakeLParam(Location.x, Location.y));
        }

        public static void Left_Click(IntPtr hWnd, (int x, int y) Location, double delayTime = 1)
        {

            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.x, Location.y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.x, Location.y));
        }

        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }




    }
}
