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

        #endregion

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
#endif

#if OSX
        using System.Runtime.InteropServices;

        public static void GetCursorPosition()
        public static void SetCursorPosition(int x, int y)
        public static void GetKeysKey(string key)
        public static void PressMouseKey(key=RightDown)
        public static void PressKey(keyCode key)
        public static void ReleaseKey(keyCode key)

#endif
    }
}
