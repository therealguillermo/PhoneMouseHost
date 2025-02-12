using System;
using System.Runtime.InteropServices;

namespace PhoneMouse
{
    public class MouseController
    {
#if WINDOWS
        //private Point savedMousePos = new Point(Cursor.Position.X, Cursor.Position.Y); 
        private Point savedMousePos = new Point(Cursor.Position.X, Cursor.Position.Y);
        public void SaveMouseState()
        {
            var currentPos = this.GetCurrentMousePoint();
            this.savedMousePos = new Point(currentPos.X, currentPos.Y);
            Console.WriteLine("SavedMouseState");
        }

        private Point GetCurrentMousePoint()
        {
            return new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        public Point GetSavedMouseState()
        {
            return this.savedMousePos;
        }
#endif

#if !WINDOWS

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
#endif

    }
}
