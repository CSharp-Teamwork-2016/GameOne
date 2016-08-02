namespace GameOne.Source.Interfaces.MainMenu
{
    using System;
    using EventArgs;
    using Microsoft.Xna.Framework.Input;

    public interface IMenu
    {
        event EventHandler<MousePositionEventArgs> OnMouseHover;

        event EventHandler<MousePositionEventArgs> OnMouseClick;

        void Draw();

        void Update(MouseState mouseState);
    }
}
