using System.Drawing;

namespace MidiUWPRouter.FormUtils
{
    public enum MoveStates
    {
        MouseDown = 1,
        MouseDownRejected = 2,
        MouseDownAccepted = 0
    }

    public class ClickPosition
    {
        private Point Pos { get; }

        public bool InResizing { get; set; }

        public ClickPosition()
        {
            Pos = new Point();
            MoveState = MoveStates.MouseDownRejected;
            InResizing = false;
        }

        public ClickPosition(Point p, MoveStates moveState)
        {
            Pos = p;
            MoveState = moveState;
            InResizing = false;
        }

        public MoveStates MoveState { get; set; }

        public int X { get { return Pos.X; } }
        public int Y { get { return Pos.Y; } }
    }

}
