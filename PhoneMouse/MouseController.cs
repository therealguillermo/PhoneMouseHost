using System;
using System.Runtime.InteropServices;

namespace PhoneMouse
{
    public class MouseController
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct CGPoint
        {
            public double X;
            public double Y;

            public CGPoint(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern IntPtr CGEventCreate(IntPtr eventSource);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern CGPoint CGEventGetLocation(IntPtr eventRef);

        private CGPoint savedMousePos;

        public void SaveMouseState()
        {
            CGPoint currentPos = GetCurrentMousePoint();
            this.savedMousePos = currentPos;
            //Console.WriteLine($"SavedMouseState: X={currentPos.X}, Y={currentPos.Y}");
        }

        private CGPoint GetCurrentMousePoint()
        {
            //Create a point to store the current mouse position
            IntPtr mouseEvent = CGEventCreate(IntPtr.Zero);
            CGPoint currentPoint = CGEventGetLocation(mouseEvent);
            //CGPoint currentPoint = NSEvent_mouseLocation();
            //Console.WriteLine($"CurrentMouseState: X={currentPoint.X}, Y={currentPoint.Y}");
            return currentPoint;
        }

        public CGPoint GetSavedMouseState()
        {
            return this.savedMousePos;
        }
    }
}
