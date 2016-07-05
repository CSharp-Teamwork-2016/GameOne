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
        #region Fields

        // Game objects
        public static Level level;
        private Input input;

        #endregion Fields

        //===================================================================

        #region Constructors

        public Loop(KeyboardState keyboardState, MouseState mouseState)
        {
            level = new Level();

            DebugInfo = string.Empty;
            Console = string.Empty;
            this.input = new Input(keyboardState, mouseState);
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        // Debug
        public static string DebugInfo { get; set; }
        // Debug
        public static string Console { get; set; }

        public static bool ShowFPS { get; set; }

        #endregion Properties

        //===================================================================

        #region Methods

        internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState) // ??
		{
            DebugInfo = string.Empty;
			level.Player.Input(this.input.Update(keyboardState, mouseState));

            if (level.ExitOpen)
            {
                level.ExitOpen = false;
                level.SetExit();
            }

            if (level.ExitTriggered)
            {
                level.ExitTriggered = false;
                level.NextLevel();
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
            {
                DebugInfo += string.Format($"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
            }
#endif
		}

		internal void Render()
		{
			level.Geometry.ForEach(Primitive.DrawTile);
			foreach (var entity in level.Entities.Where(e => e is Model))
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
            // Healthbar
            double hpx = 610;
            double hpy = 10;
            double hpw = 180;
            double hph = 10;
            double hpc = ((double)level.Player.Health / level.Player.MaxHealth) * hpw;
            Output.StrokeRect(hpx, hpy, hpw, hph, Color.Red);
            Output.FillRect(hpx, hpy, hpc, hph, Color.Red);
            // Flasks
            for (int i = 0; i < level.Player.HealthPotions; i++)
            {
                double left = 610 + i * 20;
                double top = 30;
                double width = 16;
                double height = 16;
                Output.FillRect(left, top, width, height, Color.Purple);
                Output.FillRect(left + 4, top - 4, width - 8, 4, Color.Gray);
                Output.StrokeRect(left, top, width, height, Color.Gray, 1);
            }
            level.Geometry.ForEach(Primitive.DrawTileMini);
            Primitive.DrawModelMini(level.Player);
            Output.DrawText(string.Format($"Depth: {Level.CurrentLevel}"), 610, 270, Color.Black);
            // Output debug info
            Output.DrawText(DebugInfo, 610, 50, Color.Black);
			Output.DrawText(string.Format($"~/> {Console}_"), 10, 450, Color.Black);
        }

        #endregion Methods
    }
}
