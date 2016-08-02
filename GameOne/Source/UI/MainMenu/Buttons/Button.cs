namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework;
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

        //private int width;
        //private int height;

        protected Button(string name, int x, int y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.color = this.DefaultColor;
        }

        /**
         * @return true: If a player enters the button with mouse
         */
        //public bool enterButton()
        //{
        //    if (MouseInput.getMouseX() < buttonX + Texture.Width &&
        //            MouseInput.getMouseX() > buttonX &&
        //            MouseInput.getMouseY() < buttonY + Texture.Height &&
        //            MouseInput.getMouseY() > buttonY)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public void Update(GameTime gameTime)
        //{
        //    if (enterButton() && MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
        //        MouseInput.MouseState.LeftButton == ButtonState.Pressed)
        //    {
        //        switch (Name)
        //        {
        //            case "buy_normal_fish": //the name of the button
        //                if (Player.Gold >= 10)
        //                {
        //                    ScreenManager.addFriendly("normal_fish", new Vector2(100, 100), 100, -3, 10, 100);
        //                    Player.Gold -= 10;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        public abstract void OnMouseClick(object sender, MousePositionEventArgs args);

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
 