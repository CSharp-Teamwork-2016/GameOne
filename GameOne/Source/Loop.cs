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
			this.input = new Input(keyboardState, mouseState);
		}

	    public static string DebugInfo
	    {
	        get
	        {
	            return debugInfo;
	        }
	        set
	        {
	            debugInfo = value;
	        }
	    }

	    public static string Console
	    {
	        get
	        {
	            return console;
	        }
	        set
	        {
	            console = value;
	        }
	    }

		internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
		{
			this.input.Update(keyboardState, mouseState);

#if DEBUG
			// Debug info
			debugInfo = string.Format($"Framerate {(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
			debugInfo += string.Format($"Mouse position: {mouseState.X}, {mouseState.Y}{Environment.NewLine}");
#endif
		}

		internal void Render()
		{
#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.activeTests)
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
