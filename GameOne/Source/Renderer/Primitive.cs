namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;

    using World;
    using Entities;
    using Enumerations;

    /// <summary>
    /// Temporary class for outputting game objects without assets
    /// <para>/!\ DO NOT MODIFY /!\</para>
    /// </summary>
    public class Primitive
    {
        /// <summary>
        /// Width and height of one world tile
        /// </summary>
		private static int gridSize = 40;
        private static int miniMapSize = 3;
        private static double _cameraX = 0, _cameraY = 0;

        public static double CameraX
        {
            get
            {
                return _cameraX;
            }
            set
            {
                _cameraX = 300 - value * gridSize;
            }
        }

        public static double CameraY
        {
            get
            {
                return _cameraY;
            }
            set
            {
                _cameraY = 200 - value * gridSize;
            }
        }

        public static void DrawTile(Tile tile)
        {
            double left = (tile.X - 0.5) * gridSize + 1;
            double top = (tile.Y - 0.5) * gridSize + 1;
            double width = gridSize - 2;
            double height = gridSize - 2;
            Color color = tile.TileType == Enumerations.TileType.Floor ? Color.Gray : Color.White; // change

            Output.FillRect(left, top, width, height, color);
        }

        // Minimap projection
        public static void DrawTileMini(Tile tile)
        {
            if (tile.TileType == TileType.Wall)
            {
                double left = (tile.X - 0.5) * miniMapSize + 610;
                double top = (tile.Y - 0.5) * miniMapSize + 300;
                double width = miniMapSize;
                double height = miniMapSize;
                Output.FillRect(left, top, width, height, Color.Black);
            }
        }

        public static void DrawModel(Model model)
        {
            if (model.State == State.DEAD) return;
            double left = (model.Position.X - model.Radius) * gridSize;
            double top = (model.Position.Y - model.Radius) * gridSize;
            double width = model.Radius * 2 * gridSize;
            double height = model.Radius * 2 * gridSize;
            double dirX = model.Position.X + model.Radius * 1.3 * System.Math.Cos(model.Direction);
            double dirY = model.Position.Y + model.Radius * 1.3 * System.Math.Sin(model.Direction);

            Color color = Color.LightGray;
            if (model is Player)
            {
                color = Color.Green;
            }
            if (model is Item)
            {
                color = Color.Purple;
            }
            if (model.State == State.HURT) color = Color.Red;
            Output.FillOval(left, top, width, height, color);
            Output.StrokeOval(left, top, width, height, Color.White, 2);
            if (model is Character)
            {
                Output.DrawLine((int)(model.Position.X * gridSize), (int)(model.Position.Y * gridSize),
                        (int)(dirX * gridSize), (int)(dirY * gridSize), Color.White, 2);
            }

            if (model.State == State.ATTACK)
            {
                double swordX = model.Position.X + model.Radius * 1.3 * System.Math.Cos(model.Direction);
                double swordY = model.Position.Y + model.Radius * 1.3 * System.Math.Sin(model.Direction);
                double tipX = model.Position.X + model.Radius * 3 * System.Math.Cos(model.Direction);
                double tipY = model.Position.Y + model.Radius * 3 * System.Math.Sin(model.Direction);

                Output.DrawLine((int)(swordX * gridSize), (int)(swordY * gridSize), (int)(tipX * gridSize), (int)(tipY * gridSize), Color.Red, 3);
            }
        }

        // Minimap projection
        public static void DrawModelMini(Model model)
        {
            double left = (model.Position.X - model.Radius) * miniMapSize + 610;
            double top = (model.Position.Y - model.Radius) * miniMapSize + 300;
            double width = 2 * miniMapSize;
            double height = 2 * miniMapSize;
            Output.FillOval(left, top, width, height, Color.Red);
        }
    }
}