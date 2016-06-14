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
		private const int gridSize = 20;

		public static void DrawTile(Tile tile)
		{
			double left = (tile.X - 0.5) * gridSize + 1;
			double top = (tile.Y - 0.5) * gridSize + 1;
			double width = gridSize - 2;
			double height = gridSize - 2;
			Color color = tile.TileType == TileType.Floor ? Color.Gray : Color.White; // change

			Output.FillRect(left, top, width, height, color);
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

			Color color = model is Player ? Color.Green : Color.LightGray;
            if (model.State == State.HURT) color = Color.Red;
			Output.FillOval(left, top, width, height, color);
			Output.StrokeOval(left, top, width, height, Color.White, 2);
			Output.DrawLine((int)(model.Position.X * gridSize), (int)(model.Position.Y * gridSize), 
                    (int)(dirX * gridSize), (int)(dirY * gridSize), Color.White, 2);

            if (model.State == State.ATTACK)
            {
                double swordX = model.Position.X + model.Radius * 1.3 * System.Math.Cos(model.Direction);
                double swordY = model.Position.Y + model.Radius * 1.3 * System.Math.Sin(model.Direction);
                double tipX = model.Position.X + model.Radius * 3 * System.Math.Cos(model.Direction);
                double tipY = model.Position.Y + model.Radius * 3 * System.Math.Sin(model.Direction);

                Output.DrawLine((int) (swordX * gridSize), (int)(swordY * gridSize), (int)(tipX * gridSize), (int)(tipY * gridSize), Color.Red, 3);
            }
		}
	}
}