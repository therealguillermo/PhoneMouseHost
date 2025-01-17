using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using static PhoneMouse.InputHelper;

namespace PhoneMouse
{
    public class ControlHub : Hub
    {
        private readonly MouseController _mouseController;

        public ControlHub(MouseController mouseController)
        {
            this._mouseController = mouseController;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await Clients.Client(Context.ConnectionId).SendAsync("ClientConnected", "Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Heartbeat() 
        {
            Console.WriteLine("Heartbeat Confirmed");
            await Clients.Caller.SendAsync("CommandReceived", $"alive");
        }

        public async Task SendCommand(string command)
        {
            Console.WriteLine($"Received command: {command}");
            // Handle the command, e.g., volume up, open app, etc.
            await Clients.Caller.SendAsync("CommandReceived", $"Command '{command}' executed.");
        }
        public async Task MoveMouse(double deltaX, double deltaY)
        {
            Console.WriteLine($"Move mouse: DeltaX={deltaX}, DeltaY={deltaY}");

            // Simulate mouse movement (in a simplified way)
            int deltaXInt = (int)deltaX;
            int deltaYInt = (int)deltaY;
            MoveMousePointer(deltaXInt, deltaYInt);

            await Clients.Caller.SendAsync("MouseMoved", $"Mouse moved by ({deltaX}, {deltaY}).");
        }

        // Moves the cursor to position based off of initial mousepos start
        private void MoveMousePointer(int deltaX, int deltaY)
        {
            //var currentPos = Cursor.Position;
            var currentPos = this._mouseController.GetSavedMouseState();
            Console.WriteLine($"currentPos (should not change b4 state end) : {currentPos}");
            Cursor.Position = new Point(currentPos.X + deltaX, currentPos.Y + deltaY);
        }

        // Called when cursor drag is finished
        public async Task MouseMoveFinished() 
        {
            //this.SaveMouseState();
            this._mouseController.SaveMouseState();
            await Clients.Caller.SendAsync("MouseMoved", $"Mouse move finished");
        }

        public async Task MouseClick(string mButton)
        {
            if (mButton == "M1")
            {
                // Simulate left mouse button press (LeftDown)
                InputHelper.PressMouseKey(MouseEventF.LeftDown);
                InputHelper.PressMouseKey(MouseEventF.LeftUp);
            }
            else if (mButton == "M2")
            {
                // Simulate right mouse button press (RightDown) 
                InputHelper.PressMouseKey(MouseEventF.RightDown);
                InputHelper.PressMouseKey(MouseEventF.RightUp);
            }
            else
            {
                throw new ArgumentException("Invalid mouse button. Use 'M1' for Left or 'M2' for Right.");
            }

            await Clients.Caller.SendAsync("CommandReceived", $"MBTN Clicked.");
        }

        public async Task MousePress(string mButton)
        {
            if (mButton == "M1")
            {
                // Simulate left mouse button press (LeftDown)
                InputHelper.PressMouseKey(MouseEventF.LeftDown);
            }
            else if (mButton == "M2")
            {
                // Simulate right mouse button press (RightDown)
                InputHelper.PressMouseKey(MouseEventF.RightDown);
            }
            else
            {
                throw new ArgumentException("Invalid mouse button. Use 'M1' for Left or 'M2' for Right.");
            }

            await Clients.Caller.SendAsync("CommandReceived", $"MBTN Pressed.");
        }

        public async Task MouseRelease(string mButton)
        {
            if (mButton == "M1")
            {
                // Simulate left mouse button release (LeftUp)
                InputHelper.PressMouseKey(MouseEventF.LeftUp);
            }
            else if (mButton == "M2")
            {
                // Simulate right mouse button release (RightUp)
                InputHelper.PressMouseKey(MouseEventF.RightUp);
            }
            else
            {
                throw new ArgumentException("Invalid mouse button. Use 'M1' for Left or 'M2' for Right.");
            }

            await Clients.Caller.SendAsync("CommandReceived", $"MBTN Released.");
        }

        // Directly sends the key as press and release
        public async Task SendKey(string key) 
        {
            Console.WriteLine($"Key sent: {key}");
            bool isUppercase = ((key == key.ToUpper() && !Regex.IsMatch(key, @"[^a-zA-Z0-9]") && !(key == "ENTER" || key == "BACK")) ||
                key == "(" ||
                key == ")" ||
                key == "@" ||
                key == "#" ||
                key == "$" ||
                key == "%" ||
                key == "^" ||
                key == "&" ||
                key == "*" ||
                key == "+" ||
                key == "{" ||
                key == "}" ||
                key == "|" ||
                key == ":" || 
                key == "<" ||
                key == ">" ||
                key == "_" ||
                key == "~" ||
                key == "?" ||
                key == "!");
            Keys keyCode = this.GetKeysKey(key);

            Console.WriteLine($"{isUppercase}", key);

            if (isUppercase)
            {
                // Simulate pressing Shift key if the character is uppercase
                this.SimulateKeyDown(Keys.ShiftKey);
            }

            this.SimulateKeyDown(keyCode);
            this.SimulateKeyUp(keyCode);
                        if (isUppercase)
            {
                // Simulate pressing Shift key if the character is uppercase
                this.SimulateKeyUp(Keys.ShiftKey);
            }
            Console.WriteLine($"Keypressed {key}");
            await Clients.Caller.SendAsync("CommandReceived", $"Key send recieved");
        }

        public async Task SendKeyPress(string key)
        {                   
            Console.WriteLine($"Key pressed: {key}");

            // Simulate the key press (key down)
            //SimulateKeyDown(key);

            await Clients.Caller.SendAsync("CommandReceived", $"Key '{key}' pressed.");
        }

        // This method handles the key release (key up)
        public async Task SendKeyRelease(string key)
        {
            Console.WriteLine($"Key released: {key}");

            // Simulate the key release (key up)
            //SimulateKeyUp(key);

            await Clients.Caller.SendAsync("CommandReceived", $"Key '{key}' released.");
        }

        // Method to simulate key press (key down)
        private void SimulateKeyDown(Keys key)
        {
            InputHelper.PressKey(key);
            Console.WriteLine($"key Pressed {key}");
        }

        // Method to simulate key release (key up)
        private void SimulateKeyUp(Keys key)
        {
            InputHelper.ReleaseKey(key);
            Console.WriteLine($"key Released {key}");
        }

        private Keys GetKeysKey(string key)
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

    }
}
