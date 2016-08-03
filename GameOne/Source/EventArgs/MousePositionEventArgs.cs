namespace GameOne.Source.EventArgs
{
    using System;
    using Microsoft.Xna.Framework.Input;

    public class MousePositionEventArgs : EventArgs
    {
        public MousePositionEventArgs(int x, int y, MouseState mouseState)
        {
            this.X = x;
            this.Y = y;
            this.MouseState = mouseState;
        }

        public int X { get; }

        public int Y { get; }

        public MouseState MouseState { get; }
    }
}
