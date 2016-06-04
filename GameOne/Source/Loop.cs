namespace GameOne.Source
{
	using System;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	// Game contents
	// Level
	// Entities
	// Parameters
	// Main loop
	public class Loop
	{
		static string debugInfo;
		static string console;
		private Input input;
		private SpriteFont font;

		public Loop(KeyboardState keyboardState, MouseState mouseState)
		{
			debugInfo = "";
			console = "";
			input = new Input(keyboardState, mouseState);
		}

		public static string DebugInfo { get { return debugInfo; } set { debugInfo = value; } }

		public static string Console {  get { return console; } set { console = value; } }

		internal void SetFont(SpriteFont spriteFont)
		{
			font = spriteFont;
		}

		internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
		{
			input.Update(keyboardState, mouseState);

#if DEBUG
			// Debug info
			debugInfo = string.Format($"Framerate {(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
			debugInfo += string.Format($"Mouse position: {mouseState.X}, {mouseState.Y}{Environment.NewLine}");
			// Execute tests
			foreach (Action test in Tests.ListOf.activeTests)
			{
				test();
			}
#endif
		}

		internal void Render(SpriteBatch spriteBatch)
		{
#if DEBUG
			// Output debug info
			spriteBatch.DrawString(font, debugInfo, new Vector2(10, 10), Color.Black);
			spriteBatch.DrawString(font, string.Format($"~/> {console}_"), new Vector2(10, 450), Color.Black);
#endif
		}
	}
}
