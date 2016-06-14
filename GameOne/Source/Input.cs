﻿namespace GameOne.Source
{
    using Microsoft.Xna.Framework.Input;

    using Enumerations;

    public class Input
    {
        private KeyboardState kbPrevious;
        private MouseState msPrevious;

        public Input(KeyboardState keyboardState, MouseState mouseState)
        {
            this.kbPrevious = keyboardState;
            this.msPrevious = mouseState;
#if DEBUG
            Tests.Developer.Init();
#endif
        }

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
				}

                if (this.kbPrevious.IsKeyUp(key))
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (Loop.Console.Length > 0)
                            {
                                Loop.Console = Loop.Console.Substring(0, Loop.Console.Length - 1);
                            }
                            break;
                        case Keys.Space:
                            Loop.Console += " ";
                            break;
                        case Keys.D2:
                            Loop.Console += "2";
                            break;
                        case Keys.D3:
                            Loop.Console += "3";
                            break;
                        case Keys.D4:
                            Loop.Console += "4";
                            break;
                        case Keys.D5:
                            Loop.Console += "5";
                            break;
                        case Keys.D6:
                            Loop.Console += "6";
                            break;
                        case Keys.D7:
                            Loop.Console += "7";
                            break;
                        case Keys.D8:
                            Loop.Console += "8";
                            break;
                        case Keys.D9:
                            Loop.Console += "9";
                            break;
                        case Keys.D0:
                            Loop.Console += "0";
                            break;
#if DEBUG
                        case Keys.Enter:
                            Tests.Developer.Exec(Loop.Console);
                            Loop.Console = string.Empty;
                            break;
#endif
                        default:
                            if (key.ToString().Length == 1)
                            {
                                Loop.Console += key.ToString();
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