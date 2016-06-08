namespace GameOne.Source
{
	using System;
	using System.Linq;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;

	using Renderer;
	using Enumerations;
    using World;
    using Entities;

    // Game contents
    // Level
    // Entities
    // Parameters
    // Main loop
    public class Loop
	{
		// Game objects
		Level level;

		// Debug
		public static string debugInfo;
		public static string console;

		private Input input;

		public Loop(KeyboardState keyboardState, MouseState mouseState)
		{
			level = new Level();

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

		internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState) // ??
		{
			debugInfo = "";
			level.Player.Input(this.input.Update(keyboardState, mouseState));

			// TODO update objects
			foreach (Entity entity in level.Entities)
			{
				debugInfo += "Updating...\n";
				entity.Update(time.ElapsedGameTime.Milliseconds / 1000.0);
			}
			Physics.CollisionResolution(level.Entities
                    .Where(entity => entity is Model)
                    .Select(entity => (Model)entity)
                    .ToList());
			Physics.BoundsCheck(level.Entities
                    .Where(entity => entity is Model)
                    .Select(entity => (Model)entity)
                    .ToList(), 
                    level.Geometry
                    .Where(tile => tile.TileType == TileType.Wall)
                    .ToList());

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
			level.Geometry.ForEach(Primitive.DrawTile);
			foreach (var entity in level.Entities.Where(e => e is Entities.Model))
			{
			    var model = (Model)entity;
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
