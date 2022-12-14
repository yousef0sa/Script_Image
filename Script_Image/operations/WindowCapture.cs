using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
namespace ScriptImage

{
    public class WindowCapture : DllHolder 
    {
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

    }
}