using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace PhoneMouse
{
    public class InputHelper
    {
#if WINDOWS
        #region Imports/Structs/Enums
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }

        public struct Input
        {
            public int type;
            public InputUnion u;
        }

        [Flags]
        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        public enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }

        [Flags]
        public enum MouseEventF
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint Type;
            public MOUSEINPUT Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public int dwExtraInfo;
        }

        private const int INPUT_MOUSE = 0;
        private const int MOUSEEVENTF_MOVE = 0x0001;

        public static void MoveMouse(int dx, int dy)
        {
            mouse_event((uint)MouseEventF.Move, (uint)dx, (uint)dy, 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public const Keys _ShiftKey = Keys.ShiftKey;
        #endregion


        #region Wrapper Methods
        public static POINT GetCursorPosition()
        {
            GetCursorPos(out POINT point);
            return point;
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }


        public static Vector2 GetCursorPositionVector2()
        {
            GetCursorPos(out POINT point);
            return new Vector2(point.X, point.Y);
        }

        public static void MoveMouseRelative(int dx, int dy)
        {
            POINT point;
            GetCursorPos(out point);

            SetCursorPos(point.X + dx, point.Y + dy);
        }

        public static void PressMouseKey(MouseEventF key = MouseEventF.RightDown)
        {
            mouse_event((uint)key, 0, 0, 0, 0);
        }

        public static Keys GetKeysKey(string key)
        {
            switch (key)
            {
                // Uppercase and lowercase keys (A-Z, 0-9)
                case "1": return Keys.D1;
                case "2": return Keys.D2;
                case "3": return Keys.D3;
                case "4": return Keys.D4;
                case "5": return Keys.D5;
                case "6": return Keys.D6;
                case "7": return Keys.D7;
                case "8": return Keys.D8;
                case "9": return Keys.D9;
                case "0": return Keys.D0;

                case "Q": return Keys.Q;
                case "W": return Keys.W;
                case "E": return Keys.E;
                case "R": return Keys.R;
                case "T": return Keys.T;
                case "Y": return Keys.Y;
                case "U": return Keys.U;
                case "I": return Keys.I;
                case "O": return Keys.O;
                case "P": return Keys.P;
                case "A": return Keys.A;
                case "S": return Keys.S;
                case "D": return Keys.D;
                case "F": return Keys.F;
                case "G": return Keys.G;
                case "H": return Keys.H;
                case "J": return Keys.J;
                case "K": return Keys.K;
                case "L": return Keys.L;
                case "Z": return Keys.Z;
                case "X": return Keys.X;
                case "C": return Keys.C;
                case "V": return Keys.V;
                case "B": return Keys.B;
                case "N": return Keys.N;
                case "M": return Keys.M;

                case "q": return Keys.Q;
                case "w": return Keys.W;
                case "e": return Keys.E;
                case "r": return Keys.R;
                case "t": return Keys.T;
                case "y": return Keys.Y;
                case "u": return Keys.U;
                case "i": return Keys.I;
                case "o": return Keys.O;
                case "p": return Keys.P;
                case "a": return Keys.A;
                case "s": return Keys.S;
                case "d": return Keys.D;
                case "f": return Keys.F;
                case "g": return Keys.G;
                case "h": return Keys.H;
                case "j": return Keys.J;
                case "k": return Keys.K;
                case "l": return Keys.L;
                case "z": return Keys.Z;
                case "x": return Keys.X;
                case "c": return Keys.C;
                case "v": return Keys.V;
                case "b": return Keys.B;
                case "n": return Keys.N;
                case "m": return Keys.M;
                // Shift key
                case "⇧": return Keys.Shift;

                // Backspace
                case "BACK": return Keys.Back;

                // Enter
                case "ENTER": return Keys.Enter;

                // Special characters

                case "~": return Keys.Oemtilde;// Tilde (Shift + `)
                case "`": return Keys.Oemtilde;
                case "@": return Keys.D2;  // At symbol (Shift + 2)
                case "#": return Keys.D3;  // Hash/Number sign (Shift + 3)
                case "$": return Keys.D4;  // Dollar sign (Shift + 4)
                case "%": return Keys.D5;  // Percent sign (Shift + 5)
                case "^": return Keys.D6;  // Caret (Shift + 6)
                case "&": return Keys.D7;  // Ampersand (Shift + 7)
                case "*": return Keys.D8;  // Asterisk (Shift + 8)
                case "(": return Keys.D9;  // Left parenthesis (Shift + 9)
                case ")": return Keys.D0;  // Right parenthesis (Shift + 0)

                // Other Special Characters
                case "-": return Keys.OemMinus;  // Hyphen/Minus
                case "_": return Keys.OemMinus;  // Underscore (Shift + -)
                case "=": return Keys.Oemplus;   // Equals
                case "+": return Keys.Oemplus;   // Plus sign (Shift + =)

                // Brackets and Backslash
                case "{": return Keys.OemOpenBrackets;  // Left brace (Shift + [)
                case "}": return Keys.OemCloseBrackets; // Right brace (Shift + ])
                case "[": return Keys.OemOpenBrackets;  // Left bracket
                case "]": return Keys.OemCloseBrackets; // Right bracket
                case "|": return Keys.OemBackslash;     // Pipe (Shift + \)
                case "\\": return Keys.OemBackslash;    // Backslash

                // Punctuation and Symbols
                case "/": return Keys.OemQuestion;     // Slash
                case ":": return Keys.OemSemicolon; // Colon (Shift + ;)
                case ";": return Keys.OemSemicolon; // Semicolon
                case "<": return Keys.Oemcomma;     // Less than (Shift + ,)
                case ">": return Keys.OemPeriod;    // Greater than (Shift + .)
                case ".": return Keys.OemPeriod;    // Period
                case ",": return Keys.Oemcomma;     // Comma
                case "?": return Keys.OemQuestion;     // Question mark (Shift + /)
                case "!": return Keys.D1;           // Exclamation mark (Shift + 1)

                // Arrow keys
                case "↑": return Keys.Up;
                case "↓": return Keys.Down;
                case "←": return Keys.Left;
                case "→": return Keys.Right;

                // Miscellaneous keys
                case "Space": return Keys.Space;
                case "Tab": return Keys.Tab;
                case "CapsLock": return Keys.Capital;
                case "Esc": return Keys.Escape;
                case "F1": return Keys.F1;
                case "F2": return Keys.F2;
                case "F3": return Keys.F3;
                case "F4": return Keys.F4;
                case "F5": return Keys.F5;
                case "F6": return Keys.F6;
                case "F7": return Keys.F7;
                case "F8": return Keys.F8;
                case "F9": return Keys.F9;
                case "F10": return Keys.F10;
                case "F11": return Keys.F11;
                case "F12": return Keys.F12;

                // If the key isn't recognized, return None
                default: return Keys.None;
            }
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(ushort bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;
        const byte VK_A = 0x41;

        public static void PressKey(Keys key)
        {
            // Simulate key down
            keybd_event((byte)key, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        }

        public static void PressKey(byte virtualKeyCode)
        {
            // Simulate key down
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        }

        public static void ReleaseKey(Keys key)
        {
            // Simulate key up
            keybd_event((byte)key, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void ReleaseKey(byte virtualKeyCode)
        {
            // Simulate key up
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void ReleaseKeys()
        {
            InputHelper.ReleaseKey((byte)Keys.W);
            InputHelper.ReleaseKey((byte)Keys.A);
            InputHelper.ReleaseKey((byte)Keys.S);
            InputHelper.ReleaseKey((byte)Keys.D);
            InputHelper.ReleaseKey((byte)Keys.X);
            InputHelper.ReleaseKey((byte)Keys.K);
            InputHelper.ReleaseKey((byte)Keys.R);
            InputHelper.ReleaseKey((byte)Keys.F);
            InputHelper.ReleaseKey((byte)Keys.D3);
            InputHelper.ReleaseKey((byte)Keys.ControlKey);
            InputHelper.PressMouseKey(InputHelper.MouseEventF.LeftUp);
            InputHelper.PressMouseKey(InputHelper.MouseEventF.RightUp);
        }

        public static MouseEventF GetMouseButton(string mButton, bool down) 
        {
            if (mButton == "M1" && !down) 
            {
                return MouseEventF.LeftUp;
            }
            if (mButton == "M1" && down) 
            {
                return MouseEventF.LeftDown;
            }
            if (mButton == "M2" && !down) 
            {
                return MouseEventF.RightUp;
            }
            if (mButton == "M2" && down) 
            {
                return MouseEventF.RightUp;
            }
            return MouseEventF.LeftUp;
        }

        #endregion

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
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
        private static extern IntPtr CGEventCreateMouseEvent(IntPtr eventSource, int type, CGPoint point, int mouseEvent);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern void CGEventSetType(IntPtr eventSource, int type);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern IntPtr CGEventCreateScrollWheelEvent(int axis, int scrollType, int wheelCount, int delta);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern void CGEventSetIntegerValueField(IntPtr eventSource, int field, int value);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        public static extern IntPtr CGEventCreateKeyboardEvent(IntPtr eventSource, uint virtualKey, bool keyDown);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern void CGEventPost(IntPtr eventSource, IntPtr eventTarget);

        [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
        public static extern void CFRelease(IntPtr cf);


        const int kCGEventMouseMoved = 1;
        const int kCGEventLeftMouseDown = 10;
        const int kCGEventLeftMouseUp = 11;
        public enum MacVirtualKeyCodes
        {
            
            kVK_ANSI_A                    = 0x00,
            kVK_ANSI_S                    = 0x01,
            kVK_ANSI_D                    = 0x02,
            kVK_ANSI_F                    = 0x03,
            kVK_ANSI_H                    = 0x04,
            kVK_ANSI_G                    = 0x05,
            kVK_ANSI_Z                    = 0x06,
            kVK_ANSI_X                    = 0x07,
            kVK_ANSI_C                    = 0x08,
            kVK_ANSI_V                    = 0x09,
            kVK_ANSI_B                    = 0x0B,
            kVK_ANSI_Q                    = 0x0C,
            kVK_ANSI_W                    = 0x0D,
            kVK_ANSI_E                    = 0x0E,
            kVK_ANSI_R                    = 0x0F,
            kVK_ANSI_Y                    = 0x10,
            kVK_ANSI_T                    = 0x11,
            kVK_ANSI_1                    = 0x12,
            kVK_ANSI_2                    = 0x13,
            kVK_ANSI_3                    = 0x14,
            kVK_ANSI_4                    = 0x15,
            kVK_ANSI_6                    = 0x16,
            kVK_ANSI_5                    = 0x17,
            kVK_ANSI_Equal                = 0x18,
            kVK_ANSI_9                    = 0x19,
            kVK_ANSI_7                    = 0x1A,
            kVK_ANSI_Minus                = 0x1B,
            kVK_ANSI_8                    = 0x1C,
            kVK_ANSI_0                    = 0x1D,
            kVK_ANSI_RightBracket         = 0x1E,
            kVK_ANSI_O                    = 0x1F,
            kVK_ANSI_U                    = 0x20,
            kVK_ANSI_LeftBracket          = 0x21,
            kVK_ANSI_I                    = 0x22,
            kVK_ANSI_P                    = 0x23,
            kVK_ANSI_L                    = 0x25,
            kVK_ANSI_J                    = 0x26,
            kVK_ANSI_Quote                = 0x27,
            kVK_ANSI_K                    = 0x28,
            kVK_ANSI_Semicolon            = 0x29,
            kVK_ANSI_Backslash            = 0x2A,
            kVK_ANSI_Comma                = 0x2B,
            kVK_ANSI_Slash                = 0x2C,
            kVK_ANSI_N                    = 0x2D,
            kVK_ANSI_M                    = 0x2E,
            kVK_ANSI_Period               = 0x2F,
            kVK_ANSI_Grave                = 0x32,
            kVK_ANSI_KeypadDecimal        = 0x41,
            kVK_ANSI_KeypadMultiply       = 0x43,
            kVK_ANSI_KeypadPlus           = 0x45,
            kVK_ANSI_KeypadClear          = 0x47,
            kVK_ANSI_KeypadDivide         = 0x4B,
            kVK_ANSI_KeypadEnter          = 0x4C,
            kVK_ANSI_KeypadMinus          = 0x4E,
            kVK_ANSI_KeypadEquals         = 0x51,
            kVK_ANSI_Keypad0              = 0x52,
            kVK_ANSI_Keypad1              = 0x53,
            kVK_ANSI_Keypad2              = 0x54,
            kVK_ANSI_Keypad3              = 0x55,
            kVK_ANSI_Keypad4              = 0x56,
            kVK_ANSI_Keypad5              = 0x57,
            kVK_ANSI_Keypad6              = 0x58,
            kVK_ANSI_Keypad7              = 0x59,
            kVK_ANSI_Keypad8              = 0x5B,
            kVK_ANSI_Keypad9              = 0x5C,
            kVK_Return                    = 0x24,
            kVK_Tab                       = 0x30,
            kVK_Space                     = 0x31,
            kVK_Delete                    = 0x33,
            kVK_Escape                    = 0x35,
            kVK_Command                   = 0x37,
            kVK_Shift                     = 0x38,
            kVK_CapsLock                  = 0x39,
            kVK_Option                    = 0x3A,
            kVK_Control                   = 0x3B,
            kVK_RightShift                = 0x3C,
            kVK_RightOption               = 0x3D,
            kVK_RightControl              = 0x3E,
            kVK_Function                  = 0x3F,
            kVK_F17                       = 0x40,
            kVK_VolumeUp                  = 0x48,
            kVK_VolumeDown                = 0x49,
            kVK_Mute                      = 0x4A,
            kVK_F18                       = 0x4F,
            kVK_F19                       = 0x50,
            kVK_F20                       = 0x5A,
            kVK_F5                        = 0x60,
            kVK_F6                        = 0x61,
            kVK_F7                        = 0x62,
            kVK_F3                        = 0x63,
            kVK_F8                        = 0x64,
            kVK_F9                        = 0x65,
            kVK_F11                       = 0x67,
            kVK_F13                       = 0x69,
            kVK_F16                       = 0x6A,
            kVK_F14                       = 0x6B,
            kVK_F10                       = 0x6D,
            kVK_F12                       = 0x6F,
            kVK_F15                       = 0x71,
            kVK_Help                      = 0x72,
            kVK_Home                      = 0x73,
            kVK_PageUp                    = 0x74,
            kVK_ForwardDelete             = 0x75,
            kVK_F4                        = 0x76,
            kVK_End                       = 0x77,
            kVK_F2                        = 0x78,
            kVK_PageDown                  = 0x79,
            kVK_F1                        = 0x7A,
            kVK_LeftArrow                 = 0x7B,
            kVK_RightArrow                = 0x7C,
            kVK_DownArrow                 = 0x7D,
            kVK_UpArrow                   = 0x7E
        }



        public enum CGEventType
        {
            MouseMoved = 5,
            LeftMouseDown = 1,
            LeftMouseUp = 2,
            RightMouseDown = 3,
            RightMouseUp = 4
        }

        public enum CGMouseButton
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        public enum CGEventTapLocation
        {
            HID = 0,
            Session = 1,
            AnnotatedSession = 2
        }
        public const MacVirtualKeyCodes _ShiftKey = MacVirtualKeyCodes.kVK_Shift;

        public static void GetCursorPosition() {}

        public static void SetCursorPosition(double x, double y)
        {
            CGPoint newPosition = new CGPoint(x, y);
            IntPtr mouseEvent = CGEventCreateMouseEvent(IntPtr.Zero, (int)CGEventType.MouseMoved, newPosition,(int)CGMouseButton.Left);
            CGEventPost((nint)CGEventTapLocation.HID, mouseEvent);
        }

        public static MacVirtualKeyCodes GetKeysKey(string key)
        {
            switch (key)
            {
                case "1": return MacVirtualKeyCodes.kVK_ANSI_1;
                case "2": return MacVirtualKeyCodes.kVK_ANSI_2;
                case "3": return MacVirtualKeyCodes.kVK_ANSI_3;
                case "4": return MacVirtualKeyCodes.kVK_ANSI_4;
                case "5": return MacVirtualKeyCodes.kVK_ANSI_5;
                case "6": return MacVirtualKeyCodes.kVK_ANSI_6;
                case "7": return MacVirtualKeyCodes.kVK_ANSI_7;
                case "8": return MacVirtualKeyCodes.kVK_ANSI_8;
                case "9": return MacVirtualKeyCodes.kVK_ANSI_9;
                case "0": return MacVirtualKeyCodes.kVK_ANSI_0;

                case "a":
                case "A": return MacVirtualKeyCodes.kVK_ANSI_A;
                case "s":
                case "S": return MacVirtualKeyCodes.kVK_ANSI_S;
                case "d":
                case "D": return MacVirtualKeyCodes.kVK_ANSI_D;
                case "f":
                case "F": return MacVirtualKeyCodes.kVK_ANSI_F;
                case "h":
                case "H": return MacVirtualKeyCodes.kVK_ANSI_H;
                case "g":
                case "G": return MacVirtualKeyCodes.kVK_ANSI_G;
                case "z":
                case "Z": return MacVirtualKeyCodes.kVK_ANSI_Z;
                case "x":
                case "X": return MacVirtualKeyCodes.kVK_ANSI_X;
                case "c":
                case "C": return MacVirtualKeyCodes.kVK_ANSI_C;
                case "v":  
                case "V": return MacVirtualKeyCodes.kVK_ANSI_V;
                case "b":
                case "B": return MacVirtualKeyCodes.kVK_ANSI_B;
                case "q":
                case "Q": return MacVirtualKeyCodes.kVK_ANSI_Q;
                case "w":
                case "W": return MacVirtualKeyCodes.kVK_ANSI_W;
                case "e":
                case "E": return MacVirtualKeyCodes.kVK_ANSI_E;
                case "r":
                case "R": return MacVirtualKeyCodes.kVK_ANSI_R;
                case "y":
                case "Y": return MacVirtualKeyCodes.kVK_ANSI_Y;
                case "t":
                case "T": return MacVirtualKeyCodes.kVK_ANSI_T;
                case "i":
                case "I": return MacVirtualKeyCodes.kVK_ANSI_I;
                case "j":
                case "J": return MacVirtualKeyCodes.kVK_ANSI_J;
                case "k":
                case "K": return MacVirtualKeyCodes.kVK_ANSI_K;
                case "l":
                case "L": return MacVirtualKeyCodes.kVK_ANSI_L;
                case "m":
                case "M": return MacVirtualKeyCodes.kVK_ANSI_M;
                case "n":
                case "N": return MacVirtualKeyCodes.kVK_ANSI_N;
                case "o":
                case "O": return MacVirtualKeyCodes.kVK_ANSI_O;
                case "p":
                case "P": return MacVirtualKeyCodes.kVK_ANSI_P;
                case "u":
                case "U": return MacVirtualKeyCodes.kVK_ANSI_U;

                case "⇧": return MacVirtualKeyCodes.kVK_Shift;

                // Backspace
                case "BACK": return MacVirtualKeyCodes.kVK_Delete;

                // Enter
                case "ENTER": return MacVirtualKeyCodes.kVK_Return;

                // Special characters

                case "~": // Tilde (Shift + `)
                case "`": return MacVirtualKeyCodes.kVK_ANSI_Grave;
                case "!": return MacVirtualKeyCodes.kVK_ANSI_1;  // Exclamation mark (Shift + 1)
                case "@": return MacVirtualKeyCodes.kVK_ANSI_2;  // At symbol (Shift + 2)
                case "#": return MacVirtualKeyCodes.kVK_ANSI_3;  // Hash/Number sign (Shift + 3)
                case "$": return MacVirtualKeyCodes.kVK_ANSI_4;  // Dollar sign (Shift + 4)
                case "%": return MacVirtualKeyCodes.kVK_ANSI_5;  // Percent sign (Shift + 5)
                case "^": return MacVirtualKeyCodes.kVK_ANSI_6;  // Caret (Shift + 6)
                case "&": return MacVirtualKeyCodes.kVK_ANSI_7;  // Ampersand (Shift + 7)
                case "*": return MacVirtualKeyCodes.kVK_ANSI_8;  // Asterisk (Shift + 8)
                case "(": return MacVirtualKeyCodes.kVK_ANSI_9;  // Left parenthesis (Shift + 9)
                case ")": return MacVirtualKeyCodes.kVK_ANSI_0;  // Right parenthesis (Shift + 0)

                // Other Special Characters
                case "-":   // Hyphen/Minus
                case "_": return MacVirtualKeyCodes.kVK_ANSI_Minus;  // Underscore (Shift + -)Gl
                case "=":    // Equals
                case "+": return MacVirtualKeyCodes.kVK_ANSI_Equal;   // Plus sign (Shift + =)

                // Brackets and Backslash
                case "{": return MacVirtualKeyCodes.kVK_ANSI_LeftBracket;  // Left brace (Shift + [)
                case "}": return MacVirtualKeyCodes.kVK_ANSI_RightBracket; // Right brace (Shift + ])
                case "[": return MacVirtualKeyCodes.kVK_ANSI_LeftBracket;  // Left bracket
                case "]": return MacVirtualKeyCodes.kVK_ANSI_RightBracket; // Right bracket
                case "|": return MacVirtualKeyCodes.kVK_ANSI_Backslash;     // Pipe (Shift + \)
                case "\\": return MacVirtualKeyCodes.kVK_ANSI_Backslash;    // Backslash

                // Punctuation and Symbols
                case "/": return MacVirtualKeyCodes.kVK_ANSI_Slash;     // Slash
                case ":":  // Colon (Shift + ;)
                case ";": return MacVirtualKeyCodes.kVK_ANSI_Semicolon; // Semicolon
                case "<": return MacVirtualKeyCodes.kVK_ANSI_Comma;     // Less than (Shift + ,)
                case ">":     // Greater than (Shift + .)
                case ".": return MacVirtualKeyCodes.kVK_ANSI_Period;    // Period
                case ",": return MacVirtualKeyCodes.kVK_ANSI_Comma;     // Comma
                case "?": return MacVirtualKeyCodes.kVK_ANSI_Slash;  // Question mark (Shift + /)
                

                // Arrow keys
                case "↑": return MacVirtualKeyCodes.kVK_UpArrow;
                case "↓": return MacVirtualKeyCodes.kVK_DownArrow;
                case "←": return MacVirtualKeyCodes.kVK_LeftArrow;
                case "→": return MacVirtualKeyCodes.kVK_RightArrow;

                // Miscellaneous keys
                case "Space": return MacVirtualKeyCodes.kVK_Space;
                case "Tab": return MacVirtualKeyCodes.kVK_Tab;
                case "CapsLock": return MacVirtualKeyCodes.kVK_CapsLock;
                case "Esc": return MacVirtualKeyCodes.kVK_Escape;
                case "F1": return MacVirtualKeyCodes.kVK_F1;
                case "F2": return MacVirtualKeyCodes.kVK_F2;
                case "F3": return MacVirtualKeyCodes.kVK_F3;
                case "F4": return MacVirtualKeyCodes.kVK_F4;
                case "F5": return MacVirtualKeyCodes.kVK_F5;
                case "F6": return MacVirtualKeyCodes.kVK_F6;
                case "F7": return MacVirtualKeyCodes.kVK_F7;
                case "F8": return MacVirtualKeyCodes.kVK_F8;
                case "F9": return MacVirtualKeyCodes.kVK_F9;
                case "F10": return MacVirtualKeyCodes.kVK_F10;
                case "F11": return MacVirtualKeyCodes.kVK_F11;
                case "F12": return MacVirtualKeyCodes.kVK_F12;
                
                // Add any other keys as needed

                default: throw new ArgumentException($"Key '{key}' is not recognized.");
            }
        }
        public static void PressMouseKey(int mButton)
        {
            CGPoint cursor = GetCurrentMousePoint();
            CGEventType eventType = (CGEventType)mButton;

            IntPtr mouseEvent = CGEventCreateMouseEvent(IntPtr.Zero, (int)eventType, cursor, (int)CGMouseButton.Left);
            CGEventPost(0, mouseEvent);
            CFRelease(mouseEvent);
        }
        
        public static void PressKey(MacVirtualKeyCodes key)
        {
            IntPtr keyDown = CGEventCreateKeyboardEvent(IntPtr.Zero, (ushort)key, true);
            CGEventPost((nint)CGEventTapLocation.HID, keyDown);
        }

        public static void ReleaseKey(MacVirtualKeyCodes key)
        {
            IntPtr keyUp = CGEventCreateKeyboardEvent(IntPtr.Zero, (ushort)key, false);
            CGEventPost((nint)CGEventTapLocation.HID, keyUp);
        }

        public static int GetMouseButton(string mButton, bool down) 
        {
            // Simulate mouse button press/release for M1 (left) and M2 (right)
            if (mButton == "M1")
            {
                return down ? (int)CGEventType.LeftMouseDown : (int)CGEventType.LeftMouseUp;
            }
            else if (mButton == "M2")
            {
                return down ? (int)CGEventType.RightMouseDown : (int)CGEventType.RightMouseUp;
            }
            return -1; // Invalid button
        }

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern CGPoint CGEventGetLocation(IntPtr eventRef);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern IntPtr CGEventCreate(IntPtr eventSource);

        private static CGPoint GetCurrentMousePoint()
        {
            //Create a point to store the current mouse position
            IntPtr mouseEvent = CGEventCreate(IntPtr.Zero);
            CGPoint currentPoint = CGEventGetLocation(mouseEvent);
            return currentPoint;
        }
#endif
    }
}
