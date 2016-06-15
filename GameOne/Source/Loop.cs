﻿namespace GameOne.Source
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
		public static Level level;

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

            if (level.exitOpen)
            {
                level.exitOpen = false;
                level.SetExit();
            }
			foreach (Entity entity in level.Entities)
			{
				entity.Update(time.ElapsedGameTime.Milliseconds / 1000.0);
			}
			Physics.CollisionResolution(level.Entities
                    .OfType<Model>()
                    .Where(e => e.Alive)
                    .ToList());
			Physics.BoundsCheck(level.Entities
                    .OfType<Model>()
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
			level.Geometry.ForEach(Primitive.DrawTile);
			foreach (var entity in level.Entities.Where(e => e is Entities.Model))
			{
			    var model = (Model)entity;
				Primitive.DrawModel(model);
			}

#if DEBUG
			// Execute tests
			foreach (Action test in Tests.ListOf.OnDraw)
			{
				test();
			}
#endif
		}

        internal void RenderUI()
        {
            Output.FillRect(600, 0, 200, 480, Color.White);

            level.Geometry.ForEach(Primitive.DrawTileMini);
            Primitive.DrawModelMini(level.Player);
            // Output debug info
            Output.DrawText(debugInfo, 610, 10, Color.Black);
			Output.DrawText(string.Format($"~/> {console}_"), 10, 450, Color.Black);
        }
	}
}
