namespace GameOne.Source.Renderer
{
	using Microsoft.Xna.Framework;
	using Level;

	/// <summary>
	/// Temporary class for outputting game objects without assets
	/// <para>/!\ DO NOT MODIFY /!\</para>
	/// </summary>
	public class Primitive
	{
		private const int gridSize = 20;

		public static void DrawTile(Tile tile)
		{
			double left = (tile.GetX() + 0.5) * gridSize + 1;
			double top = (tile.GetY() + 0.5) * gridSize + 1;
			double width = gridSize - 2;
			double height = gridSize - 2;
			Color color = tile.GetTileType() == Enumerations.TileType.Floor ? Color.Gray : Color.White;

			Output.FillRect(left, top, width, height, color);
		}
	}
}
