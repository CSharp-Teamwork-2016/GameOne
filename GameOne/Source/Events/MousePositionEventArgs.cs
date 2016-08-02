namespace GameOne.Source.Events
{
    using System;
    using Microsoft.Xna.Framework.Input;

    public class MousePositionEventArgs : EventArgs
    {
        public MousePositionEventArgs(int x, int y, bool leftButtonPressed)
        {
            this.X = x;
            this.Y = y;
            this.LeftButtonPressed = leftButtonPressed;
        }

        public int X { get; }

        public int Y { get; }

        public bool LeftButtonPressed { get; }
    }
}
