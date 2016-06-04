namespace GameOne.Source
{
	using Microsoft.Xna.Framework.Input;
	using System.Collections.Generic;
	using System;

	public class Input
	{
		KeyboardState kbPrevious;
		MouseState msPrevious;

		public Input(KeyboardState keyboardState, MouseState mouseState)
		{
			kbPrevious = keyboardState;
			msPrevious = mouseState;
#if DEBUG
			Tests.ListOf.Init();
#endif
		}

		internal void Update(KeyboardState keyboardState, MouseState mouseState)
		{
			foreach (Keys key in keyboardState.GetPressedKeys())
			{
				if (kbPrevious.IsKeyUp(key))
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
							if (Tests.ListOf.tests.ContainsKey(Loop.Console))
							{
								Tests.ListOf.activeTests.Add(Tests.ListOf.tests[Loop.Console]);
							}
							Loop.Console = "";
							break;
#endif
						default:
							if (key.ToString().Length == 1)
								Loop.Console += key.ToString();
							break;
					}
				}
			}
			kbPrevious = keyboardState;
			msPrevious = mouseState;
		}
	}
}