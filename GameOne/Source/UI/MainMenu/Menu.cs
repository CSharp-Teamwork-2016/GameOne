namespace GameOne.Source.UI.MainMenu
{
    using System;
    using Containers;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework.Input;

    public abstract class Menu : IMenu
    {
        protected ButtonContainer buttons;

        public event EventHandler<MousePositionEventArgs> OnMouseHover;

        public event EventHandler<MousePositionEventArgs> OnMouseClick;

        public void Draw()
        {
            this.buttons.Draw();
        }

        public void Update(MouseState mouseState)
        {
            this.OnMouseHover?.Invoke(null, new MousePositionEventArgs(mouseState.X, mouseState.Y, mouseState));
            this.OnMouseClick?.Invoke(null, new MousePositionEventArgs(mouseState.X, mouseState.Y, mouseState));
        }
    }
}
