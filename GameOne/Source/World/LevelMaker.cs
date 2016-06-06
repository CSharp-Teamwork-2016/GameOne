namespace GameOne.Source.Level
{
	using System.Collections.Generic;

	/// <summary>
	/// Temporary geometry generator; to be replaced with file loader
	/// <para>/!\ DO NOT MODIFY /!\</para>
	/// </summary>
	internal class LevelMaker
	{
		internal static List<Tile> Generate()
		{
			int width = 21;
			int height = 15;
			List<Tile> result = new List<Tile>();
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
						result.Add(new Tile(j, i, Enumerations.TileType.Wall));
					else
						result.Add(new Tile(j, i, Enumerations.TileType.Floor));
				}
			}

			return result;
		}
	}
}
