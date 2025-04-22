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

        public async Task ScrollAction(string direction) {
            InputHelper.ScrollAction(direction);
            await Clients.Caller.SendAsync("CommandReceived", $"Scrolling '{direction}' executed.");
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
            //Console.WriteLine($"currentPos (should not change b4 state end) : {currentPos}");
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
            bool isUppercase = ((key == key.ToUpper() && !Regex.IsMatch(key, @"[^a-zA-Z]") && !(key == "ENTER" || key == "BACK")) ||
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
            var keyCode = InputHelper.GetKeysKey(key);

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
    }
}
