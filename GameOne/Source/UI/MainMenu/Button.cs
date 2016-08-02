namespace GameOne.Source.UI.MainMenu
{
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework;
    using Renderer;

    public class Button : IButton
    {
        private const int Width = 300;
        private const int Height = 45;

        private string name;
        private int x;
        private int y;

        //private int width;
        //private int height;

        public Button(string name, int x, int y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
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

        public void Draw()
        {
            Output.FillRect(this.x, this.y, Width, Height, Color.Chocolate);
            Output.DrawText(this.name, this.x, this.y);
        }
    }
}
 