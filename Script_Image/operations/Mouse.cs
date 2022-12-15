using OpenCvSharp;
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
        public static void Right_Click(IntPtr hWnd, Point Location,  double delayTime = 1)
        {
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
        }

        //mouse Left Click
        public static void Left_Click(IntPtr hWnd, Point Location, double delayTime = 1)
        {
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
        }

        //return LParam
        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        //function to return mouse position 
        public static Point GetCursorPosition(IntPtr hWnd = default)
        {
            GetCursorPos(out Point lpPoint);
            if (hWnd != IntPtr.Zero)
            {
                ScreenToClient(hWnd, ref lpPoint);
                return lpPoint;
            }
            else
               return lpPoint; 
         }

        //move mouse
        public static void MoveMouse(IntPtr hWnd, (int x, int y) Location, int speed = 1)
        {
            var cp = GetCursorPosition(hWnd);
            var (x1, y1) = Location;
            var (x2, y2) = (x1 - cp.X, y1 - cp.Y);
            var (x3, y3) = (x2 / speed, y2 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp.X + x3, cp.Y + y3));
            }
        }

        //drag and drop.
        public static void DragAndDrop(IntPtr hWnd, (int x, int y) Location, (int x, int y) Location2, int speed = 1)
        {
            var cp = GetCursorPosition(hWnd);
            var (x1, y1) = Location;
            var (x2, y2) = (x1 - cp.X, y1 - cp.Y);
            var (x3, y3) = (x2 / speed, y2 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp.X + x3, cp.Y + y3));
            }
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.x, Location.y));
            DelayTime.Delay(0.5);
            var cp2 = GetCursorPosition(hWnd);
            var (x5, y5) = Location2;
            var (x6, y6) = (x5 - cp2.X, y5 - cp2.Y);
            var (x7, y7) = (x6 / speed, y6 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp2.X + x7, cp2.Y + y7));
            }
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location2.x, Location2.y));
        }

    }
}
