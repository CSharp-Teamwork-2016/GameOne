namespace GameOne.Source
{
    using Enumerations;
    using Microsoft.Xna.Framework.Input;
    using Containers;
    using World;

    public class Input
    {
        private KeyboardState kbPrevious;
        private MouseState msPrevious;

        public Input(KeyboardState keyboardState, MouseState mouseState, Level level)
        {
            this.kbPrevious = keyboardState;
            this.msPrevious = mouseState;
#if DEBUG
            Tests.Developer.Init(level);
#endif
        }

        public double MouseX => this.msPrevious.X;

        public double MouseY => this.msPrevious.Y;

        internal UserInput Update(KeyboardState keyboardState, MouseState mouseState)
        {
            UserInput result = UserInput.Empty;
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.Up:
                        result = UserInput.MoveUp;
                        break;
                    case Keys.Down:
                        result = UserInput.MoveDown;
                        break;
                    case Keys.Left:
                        result = UserInput.MoveLeft;
                        break;
                    case Keys.Right:
                        result = UserInput.MoveRight;
                        break;
                    case Keys.LeftControl:
                        result = UserInput.Attack;
                        break;
                    case Keys.LeftShift:
                        result = UserInput.Shoot;
                        break;
                    case Keys.D1:
                        if (this.kbPrevious.IsKeyUp(key))
                        {
                            result = UserInput.DrinkPotion;
                        }

                        break;
                }

                if (this.kbPrevious.IsKeyUp(key))
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (GameContainer.Console.Length > 0)
                            {
                                GameContainer.Console = GameContainer.Console.Substring(0, GameContainer.Console.Length - 1);
                            }

                            break;
                        case Keys.Space:
                            GameContainer.Console += " ";
                            break;
                        case Keys.D2:
                            GameContainer.Console += "2";
                            break;
                        case Keys.D3:
                            GameContainer.Console += "3";
                            break;
                        case Keys.D4:
                            GameContainer.Console += "4";
                            break;
                        case Keys.D5:
                            GameContainer.Console += "5";
                            break;
                        case Keys.D6:
                            GameContainer.Console += "6";
                            break;
                        case Keys.D7:
                            GameContainer.Console += "7";
                            break;
                        case Keys.D8:
                            GameContainer.Console += "8";
                            break;
                        case Keys.D9:
                            GameContainer.Console += "9";
                            break;
                        case Keys.D0:
                            GameContainer.Console += "0";
                            break;
#if DEBUG
                        case Keys.Enter:
                            Tests.Developer.Exec(GameContainer.Console);
                            GameContainer.Console = string.Empty;
                            break;
#endif
                        default:
                            if (key.ToString().Length == 1)
                            {
                                GameContainer.Console += key.ToString();
                            }

                            break;
                    }
                }
            }

            this.kbPrevious = keyboardState;
            this.msPrevious = mouseState;

            return result;
        }
    }
}