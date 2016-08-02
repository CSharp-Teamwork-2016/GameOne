namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Containers;
    using Enumerations;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Renderer;

    public abstract class Button : IButton
    {
        private const int Width = 300;
        private const int Height = 45;
        private readonly Color DefaultColor = Color.Red;

        private string name;
        private Rectangle rectangle;
        private Color color;
        private readonly GameState gameState;
        private bool isMousePressedOnButton;

        protected Button(string name, int x, int y, GameState gameState)
        {
            this.name = name;
            this.rectangle = new Rectangle(x, y, Width, Height);
            this.color = this.DefaultColor;
            this.gameState = gameState;
        }

        public event EventHandler<OnButtonClickEventArgs> OnButtonClick;

        public void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            var point = new Point(args.X, args.Y);

            if (
                this.rectangle.Contains(point) &&
                args.MouseState.LeftButton == ButtonState.Pressed)
            {
                this.isMousePressedOnButton = true;
                this.color = Color.Blue;
            }
            else if (
                this.rectangle.Contains(point) &&
                args.MouseState.LeftButton == ButtonState.Released && 
                this.isMousePressedOnButton)
            {
                this.OnButtonClick?.Invoke(null, new OnButtonClickEventArgs(this.gameState, this.name));
                this.isMousePressedOnButton = false;
                // Sad
                if (this is NewGameButton)
                {
                    this.name = "Resume Game";
                }
            }
            else
            {
                this.isMousePressedOnButton = false;
            }
        }

        public void OnMouseHover(object sender, MousePositionEventArgs args)
        {
            var point = new Point(args.X, args.Y);

            this.color = this.rectangle.Contains(point) ? Color.Yellow : this.DefaultColor;
        }

        public void Draw()
        {
            Output.FillRect(this.rectangle.X, this.rectangle.Y, Width, Height, this.color);
            Output.DrawText(this.name, this.rectangle.X, this.rectangle.Y);
        }
    }
}
 