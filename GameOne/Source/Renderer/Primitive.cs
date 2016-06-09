namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;
    
    using Entities;
    using World;

    /// <summary>
    /// Temporary class for outputting game objects without assets
    /// <para>/!\ DO NOT MODIFY /!\</para>
    /// </summary>
    public class Primitive
    {
        private const int GridSize = 20;

        public static void DrawTile(Tile tile)
        {
            double left = (tile.X - 0.5) * GridSize + 1;
            double top = (tile.Y - 0.5) * GridSize + 1;
            double width = GridSize - 2;
            double height = GridSize - 2;
            Color color = tile.TileType == Enumerations.TileType.Floor ? Color.Gray : Color.White; // change

            Output.FillRect(left, top, width, height, color);
        }

        public static void DrawModel(Model model)
        {
            double left = (model.Position.X - model.Radius) * GridSize;
            double top = (model.Position.Y - model.Radius) * GridSize;
            double width = model.Radius * 2 * GridSize;
            double height = model.Radius * 2 * GridSize;
            double dirX = model.Position.X + model.Radius * 1.3 * System.Math.Cos(model.Direction);
            double dirY = model.Position.Y + model.Radius * 1.3 * System.Math.Sin(model.Direction);

            Color color = model is Player ? Color.Green : Color.LightGray;
            Output.FillOval(left, top, width, height, color);
            Output.StrokeOval(left, top, width, height, Color.White, 2);
            Output.DrawLine((int)(model.Position.X * GridSize), (int)(model.Position.Y * GridSize),
                    (int)(dirX * GridSize), (int)(dirY * GridSize), Color.White, 2);
        }
    }
}