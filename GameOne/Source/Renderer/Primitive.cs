namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;

    using World;
    using Entities;
    using Enumerations;
    using System;

    /// <summary>
    /// Temporary class for outputting game objects without assets
    /// <para>/!\ DO NOT MODIFY /!\</para>
    /// </summary>
    public class Primitive
    {
        #region Fields

        private const int CAMERAXMODIFIER = 300;
        private const int CAMERAYMODIFIER = 200;

        /// <summary>
        /// Width and height of one world tile
        /// </summary>
        private static int gridSize = 40;
        private static int miniMapSize = 3;
        private static double _cameraX;
        private static double _cameraY;

        #endregion Fields

        //===================================================================

        #region Properties

        public static double CameraX
        {
            get
            {
                return _cameraX;
            }
            set
            {
                _cameraX = CAMERAXMODIFIER - value * gridSize;
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
                _cameraY = CAMERAYMODIFIER - value * gridSize;
            }
        }

        public static Microsoft.Xna.Framework.Graphics.Texture2D FloorTile { get; set; }
        public static Microsoft.Xna.Framework.Graphics.Texture2D WallTile { get; set; }

        #endregion Properties

        //===================================================================

        //All static
        #region Methods

        public static void DrawTile(Tile tile)
        {
            double left = (tile.X - 0.5) * gridSize;
            double top = (tile.Y - 0.5) * gridSize;
            double width = gridSize;
            double height = gridSize;
            //Color color = tile.TileType == TileType.Floor ? Color.Gray : Color.White; // change

            //Output.FillRect(left, top, width, height, color);
            Output.Draw(tile.TileType == TileType.Floor ? FloorTile : WallTile,
                new Rectangle((int)left, (int)top, (int)width, (int)height));
        }

        public static void DrawModel(Model model)
        {
            if (model.State == State.DEAD)
            {
                return;
            }

            if (model is Character)
            {
                DrawCharacter((Character)model);
            }
            else if (model is Item)
            {
                DrawItem((Item)model);
            }
            else if (model is Projectile)
            {
                DrawProjectile((Projectile)model);
            }
        }

        private static void DrawItem(Item model)
        {
            double left = (model.Position.X - model.Radius) * gridSize;
            double top = (model.Position.Y - model.Radius) * gridSize;
            double width = model.Radius * 2 * gridSize;
            double height = model.Radius * 2 * gridSize;

            Color color = Color.Purple;
            if (model.Type == ItemType.EndKey)
            {
                Output.StrokeOval(left + 2, top + 2, width - 4, height - 4, Color.Black, 2);
                Output.FillRect(left, top, width / 3, height / 3, Color.Gold);
                Output.FillRect(left + width / 3 * 2, top, width / 3, height / 3, Color.Gold);
                Output.FillRect(left, top + height / 3 * 2, width / 3, height / 3, Color.Gold);
                Output.FillRect(left + width / 3 * 2, top + height / 3 * 2, width / 3, height / 3, Color.Gold);
            }
            else if (model.Type == ItemType.PotionHealth)
            {
                width /= 2;
                left += width / 2;
                Output.FillRect(left, top, width, height, Color.Red);
                Output.FillRect(left + 2, top - 4, width - 4, 4, Color.White);
                Output.StrokeRect(left, top, width, height, Color.White, 1);
            }
            else if (model.Type == ItemType.QuartzFlask)
            {
                Output.FillRect(left, top, width, height, Color.Purple);
                Output.FillRect(left + 4, top - 4, width - 8, 4, Color.White);
                Output.StrokeRect(left, top, width, height, Color.White, 1);
            }
            else
            {
                Output.FillOval(left, top, width, height, color);
                Output.StrokeOval(left, top, width, height, Color.White, 2);
            }
        }

        private static void DrawCharacter(Character model)
        {
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

            if (model.State == State.HURT) color = Color.Red;
            if (model is Enemy && ((Enemy)model).Type == EnemyType.Sentry)
            {
                Output.FillRect(left, top, width / 3, height / 3, color);
                Output.FillRect(left + width / 3 * 2, top, width / 3, height / 3, color);
                Output.FillRect(left, top + height / 3 * 2, width / 3, height / 3, color);
                Output.FillRect(left + width / 3 * 2, top + height / 3 * 2, width / 3, height / 3, color);
                left += 4;
                top += 4;
                width -= 8;
                height -= 8;
            }
            Output.FillOval(left, top, width, height, color);
            Output.StrokeOval(left, top, width, height, Color.White, 2);
            Output.DrawLine((int)(model.Position.X * gridSize), (int)(model.Position.Y * gridSize),
                    (int)(dirX * gridSize), (int)(dirY * gridSize), Color.White, 2);

            if (model.State == State.ATTACK)
            {
                double p1x = model.Position.X + Math.Cos(model.Direction + Math.PI / 2) * 0.2;
                double p1y = model.Position.Y + Math.Sin(model.Direction + Math.PI / 2) * 0.2;
                double pw = 0.9 * Math.Cos(model.Direction);
                double ph = 0.9 * Math.Sin(model.Direction);
                Output.StrokeRect(p1x * gridSize, p1y * gridSize, pw * gridSize, ph * gridSize, Color.Red, 5);
            }
        }

        private static void DrawProjectile(Projectile model)
        {
            double left = (model.Position.X - model.Radius) * gridSize;
            double top = (model.Position.Y - model.Radius) * gridSize;
            double width = model.Radius * 2 * gridSize;
            double height = model.Radius * 2 * gridSize;

            Color color = Color.Red;

            Output.FillOval(left, top, width, height, color);
            //Output.StrokeOval(left, top, width, height, Color.White, 2);
        }

        // Minimap projection
        public static void DrawTileMini(Tile tile)
        {
            if (tile.TileType == TileType.Wall)
            {
                double left = (tile.X) * miniMapSize + 610;
                double top = (tile.Y) * miniMapSize + 300;
                double width = miniMapSize;
                double height = miniMapSize;
                Output.FillRect(left, top, width, height, Color.Black);
            }
        }
        public static void DrawModelMini(Model model)
        {
            double left = (model.Position.X - model.Radius) * miniMapSize + 610;
            double top = (model.Position.Y - model.Radius) * miniMapSize + 300;
            double width = 2 * miniMapSize;
            double height = 2 * miniMapSize;
            Color color = Color.Red;
            if (model is Item) color = Color.Gold;
            Output.FillOval(left, top, width, height, color);
        }

        #endregion Methods
    }
}