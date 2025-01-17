
namespace PhoneMouse
{
    public class MouseController
    {
        //private Point savedMousePos = new Point(Cursor.Position.X, Cursor.Position.Y); 
        private Point savedMousePos = new Point(Cursor.Position.X, Cursor.Position.Y);
        public void SaveMouseState()
        {
            var currentPos = this.GetCurrentMousePoint();
            this.savedMousePos = new Point(currentPos.X, currentPos.Y);
            Console.WriteLine("SavedMouseState");
        }

        public Point GetSavedMouseState()
        {
            return this.savedMousePos;
        }

        private Point GetCurrentMousePoint()
        {
            return new Point(Cursor.Position.X, Cursor.Position.Y);
        }

    }
}
