namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Enumerations;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Renderer;

    public abstract class Button : IButton
    {
        protected const int Width = 300;
        protected const int Height = 45;
        protected Color DefaultColor = Color.Red;

        protected string name;
        protected int x;
        protected int y;
        protected Color color;
        protected GameState gameState;
        private bool isMousePressedOnButton;

        //private int width;
        //private int height;

        protected Button(string name, int x, int y, GameState gameState)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.color = this.DefaultColor;
            this.gameState = gameState;
        }

        public event EventHandler<OnButtonClickEventArgs> OnButtonClick;

        public void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            if (
                (args.X >= this.x) &&
                (args.X <= this.x + Width) &&
                (args.Y >= this.y) &&
                args.Y <= this.y + Height &&
                args.MouseState.LeftButton == ButtonState.Pressed)
            {
                this.isMousePressedOnButton = true;
                this.color = Color.Blue;
            }
            else if (
                (args.X >= this.x) &&
                (args.X <= this.x + Width) &&
                (args.Y >= this.y) &&
                args.Y <= this.y + Height &&
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
            if (
                (args.X >= this.x) && 
                (args.X <= this.x + Width) &&
                (args.Y >= this.y) &&
                args.Y <= this.y + Height)
            {
                this.color = Color.Yellow;
            }
            else
            {
                this.color = this.DefaultColor;
            }
        }

        public void Draw()
        {
            Output.FillRect(this.x, this.y, Width, Height, this.color);
            Output.DrawText(this.name, this.x, this.y);
        }
    }
}
 