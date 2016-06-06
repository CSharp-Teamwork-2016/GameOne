namespace GameOne.Source
{
	using System;
	using System.Linq;
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
		// Game objects
		Level.Level level;

		// Debug
		static string debugInfo;
		static string console;
		private Input input;

		public Loop(KeyboardState keyboardState, MouseState mouseState)
		{
			level = new Level.Level(0, 0);

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

		public static bool ShowFPS { get; set; }

		internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
		{
			debugInfo = "";
			level.Player.Input(this.input.Update(keyboardState, mouseState));

			// TODO update objects
			foreach (Entities.Entity entity in level.Entities)
			{
				debugInfo += "Updating...\n";
				entity.Update(time.ElapsedGameTime.Milliseconds / 1000.0);
			}

#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.OnUpdate)
			{
				test();
			}
			// Debug info
			if (ShowFPS)
				debugInfo += string.Format($"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
#endif
		}

		internal void Render()
		{

			// TODO render objects
			level.Geometry.ForEach(tile => Primitive.DrawTile(tile));
			foreach (Entities.Model model in level.Entities.Where(e => e is Entities.Model))
			{
				debugInfo += "Rendering...\n";
				Primitive.DrawModel(model);
			}

#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.OnDraw)
			{
				test();
			}
			// Output debug info
			Output.DrawText(debugInfo, 660, 10, Color.Black);
			Output.DrawText(string.Format($"~/> {console}_"), 10, 450, Color.Black);
#endif
		}
	}
}
