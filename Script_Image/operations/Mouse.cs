using System;

namespace ScriptImage
{
    public class Mouse : DllHolder
    {
        private enum WMessages : uint
        {
            WM_RBUTTONDOWN = 0x204, //Right mouse-button down
            WM_RBUTTONUP = 0x205,   //Right mouse-button up
            WM_LBUTTONDOWN = 0x201, //Left  mouse-button down
            WM_LBUTTONUP = 0x202,   //Left  mouse-button up   
            WM_MOUSEMOVE = 0x206,   //Mouse move
        }

        //mouse Left Click
        public static void Right_Click(IntPtr hWnd, (int x,int y) Location,  double delayTime = 1)
        {
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.x, Location.y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONUP, IntPtr.Zero, MakeLParam(Location.x, Location.y));
        }

        //mouse Left Click
        public static void Left_Click(IntPtr hWnd, (int x, int y) Location, double delayTime = 1)
        {
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.x, Location.y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.x, Location.y));
        }

        //return LParam
        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        //move mouse to location
        public static void MoveMouse(IntPtr hWnd, (int x, int y) Location)
        {
            PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(Location.x, Location.y));
        }

        //function to return mouse position 
        public static (int x, int y) GetCursorPosition(IntPtr hWnd = default)
        {
            GetCursorPos(out OpenCvSharp.Point lpPoint);
            if (hWnd != IntPtr.Zero)
            {
                ScreenToClient(hWnd, ref lpPoint);
                return (lpPoint.X, lpPoint.Y);
            }
            else
               return (lpPoint.X, lpPoint.Y); 
         }
    }
}
