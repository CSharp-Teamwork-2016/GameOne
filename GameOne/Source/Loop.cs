namespace GameOne.Source
{
	using System;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;
	using Renderer;

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

		public Loop(KeyboardState keyboardState, MouseState mouseState)
		{
			debugInfo = "";
			console = "";
			input = new Input(keyboardState, mouseState);
		}

		public static string DebugInfo { get { return debugInfo; } set { debugInfo = value; } }

		public static string Console {  get { return console; } set { console = value; } }

		public static bool ShowFPS { get; set; }

		internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
		{
			input.Update(keyboardState, mouseState);

			// TODO update objects

#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.OnUpdate)
			{
				test();
			}
			// Debug info
			debugInfo = "";
			if (ShowFPS)
				debugInfo += string.Format($"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
#endif
		}

		internal void Render()
		{

			// TODO render objects

#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.OnDraw)
			{
				test();
			}
			// Output debug info
			Output.DrawText(debugInfo, 10, 10, Color.Black);
			Output.DrawText(string.Format($"~/> {console}_"), 10, 450, Color.Black);
#endif
		}
	}
}
