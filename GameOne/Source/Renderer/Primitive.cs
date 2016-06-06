namespace GameOne.Source.Renderer
{
	using Microsoft.Xna.Framework;
	using Level;
	using Entities;

	/// <summary>
	/// Temporary class for outputting game objects without assets
	/// <para>/!\ DO NOT MODIFY /!\</para>
	/// </summary>
	public class Primitive
	{
		private const int gridSize = 30;

		public static void DrawTile(Tile tile)
		{
			double left = (tile.GetX() + 0.5) * gridSize + 1;
			double top = (tile.GetY() + 0.5) * gridSize + 1;
			double width = gridSize - 2;
			double height = gridSize - 2;
			Color color = tile.GetTileType() == Enumerations.TileType.Floor ? Color.Gray : Color.White;

			Output.FillRect(left, top, width, height, color);
		}

		public static void DrawModel(Model model)
		{
			double left = (model.X - model.Radius) * gridSize;
			double top = (model.Y - model.Radius) * gridSize;
			double width = model.Radius * 2 * gridSize;
			double height = model.Radius * 2 * gridSize;
			double dirX = model.X + model.Radius * 1.3 * System.Math.Cos(model.Direction);
			double dirY = model.Y + model.Radius * 1.3 * System.Math.Sin(model.Direction);

			Output.FillOval(left, top, width, height, Color.LightGray);
			Output.StrokeOval(left, top, width, height, Color.White, 2);
			Output.DrawLine((int)(model.X * gridSize), (int)(model.Y * gridSize), (int)(dirX * gridSize), (int)(dirY * gridSize), Color.White, 2);
		}
	}
}
