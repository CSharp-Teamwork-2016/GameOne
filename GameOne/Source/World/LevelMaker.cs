namespace GameOne.Source.World
{
	using System;
	using GameOne.Source.Enumerations;
	using GameOne.Source.Factories;
	using System.Collections.Generic;

	/// <summary>
	/// Temporary geometry generator; to be replaced with file loader
	/// <para>/!\ DO NOT MODIFY /!\</para>
	/// </summary>
	internal class LevelMaker
	{
		private const int REFSIZE = 15;
		private int depth;
		private int width;
		private int height;
		private Partition bsp;
		private Queue<Partition> queue;
		private Dictionary<long, Tile> levelMatrix;

		//========================================

		#region Properties

		public int Depth
		{
			get
			{
				return depth;
			}
		}

		public Partition Root
		{
			get
			{
				return bsp;
			}
		}

		public Dictionary<long, Tile> Tiles
		{
			get
			{
				return levelMatrix;
			}
		}

		#endregion Properties ===================

		//========================================

		//region Initialization ==================

		public LevelMaker(int depth)
		{
			this.depth = depth;
			CalcSize();
			MakeLevel();
		}

		public void NextLevel()
		{
			depth += (int)Math.Max(2, depth * 0.2);
			CalcSize();
			MakeLevel();
		}

		public void MakeLevel()
		{
			bsp = new Partition(0, 0, width, height, null);
			queue = new Queue<Partition>();
			queue.Enqueue(bsp);
			for (int i = 0; i < depth; i++)
			{
				Partition current = queue.Dequeue();
				if (current.TrySplit())
				{
					queue.Enqueue(current.LeftLeaf);
					queue.Enqueue(current.RightLeaf);
				}
			}

			bsp.MakeRoom();
			bsp.MakeHallway();
			ProcessTiles();
			//SetTransparancy();
		}

		private void CalcSize()
		{
			width = REFSIZE * (int)Math.Sqrt(depth + 1);
			height = (int)(width * 0.67);
		}

		//endregion Initialization ===============

		//========================================

		//region Extract geometry ================

		public List<Partition> GetBsp()
		{
			var partitions = new List<Partition>();
			var q = new Queue<Partition>();

			q.Enqueue(bsp);

			while (q.Count > 0)
			{
				Partition current = q.Dequeue();
				if (current.HasLeaves)
				{
					q.Enqueue(current.LeftLeaf);
					q.Enqueue(current.RightLeaf);
				}
				else
				{
					partitions.Add(current);
				}
			}

			return partitions;
		}

		public List<Room> GetRooms()
		{
			var rooms = new List<Room>();
			var q = new Queue<Partition>();

			q.Enqueue(bsp);
			while (q.Count > 0)
			{
				Partition current = q.Dequeue();
				if (current.HasLeaves)
				{
					q.Enqueue(current.LeftLeaf);
					q.Enqueue(current.RightLeaf);
				}
				else
				{
					rooms.Add(current.Room);
				}
			}

			return rooms;
		}


		public List<Hallway> GetHallways()
		{
			var hallways = new List<Hallway>();
			var q = new Queue<Partition>();

			q.Enqueue(bsp);
			while (q.Count > 0)
			{
				Partition current = q.Dequeue();
				if (current.HasLeaves)
				{
					q.Enqueue(current.LeftLeaf);
					q.Enqueue(current.RightLeaf);
				}
				if (current.Hallway != null)
				{
					hallways.Add(current.Hallway);
				}
			}

			return hallways;
		}

		public void ProcessTiles()
		{
			levelMatrix = new Dictionary<long, Tile>();

			foreach (var room in GetRooms())
			{
				var startX = room.X;
				var startY = room.Y;
				var endX = startX + room.Width;
				var endY = startY + room.Height;

				for (int col = startX; col < endX; col++)
				{
					for (int row = startY; row < endY; row++)
					{
						long currentIndex = getIndex(col, row, width);
						if (row == startY || row == endY - 1 || col == startX || col == endX - 1)
						{
							if (!levelMatrix.ContainsKey(currentIndex))
							{
								levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 1));
							}
						}
						else
						{
							levelMatrix[currentIndex] = TileFactory.getTile(col, row, 2);
						}
					}
				}
			}

			foreach (var hallway in GetHallways())
			{
				var startX = hallway.X;
				var startY = hallway.Y;
				var endX = startX + hallway.Width;
				var endY = startY + hallway.Height;

				for (int col = startX; col < endX; col++)
				{
					for (int row = startY; row < endY; row++)
					{
						long currentIndex = getIndex(col, row, width);
						if (row == startY || row == endY - 1 || col == startX || col == endX - 1)
						{
							if (!levelMatrix.ContainsKey(currentIndex))
							{
								levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 1));
							}
						}
						else
						{
							levelMatrix[currentIndex] = TileFactory.getTile(col, row, 2);
						}
					}
				}
			}
		}

		private void SetTransparancy()
		{
			var levelMatrixValues = new List<Tile>(levelMatrix.Values);
			var walls = new List<Tile>();

			foreach (var tile in levelMatrixValues)
			{
				if (tile.TileType == TileType.Wall)
				{
					walls.Add(tile);
				}
			}

			foreach (var tile in walls)
			{
				long currentIndex = getIndex((int)tile.X, (int)tile.Y, width);
				if (levelMatrix.ContainsKey(currentIndex - width) &&
						levelMatrix[currentIndex - width].TileType == TileType.Floor)
				{
					tile.Overlay();
				}
				else if (levelMatrix.ContainsKey(currentIndex - width * 2) &&
					  levelMatrix[currentIndex - width * 2].TileType == TileType.Floor)
				{
					tile.Overlay();
				}
			}
		}

		//endregion Extract geometry =============

		//========================================

		//region Static parameters ===============

		public const int MINSIZE = 7;
		public const int MAXSIZE = 15;
		public const float MINRATIO = 0.33f;
		public const float MAXRATIO = 0.66f;
		public const int HALLSIZE = 5;
		private static Random rnd;

		public static void Init()
		{
			rnd = new Random();
		}

		public static void Init(int seed)
		{
			rnd = new Random(seed);
		}

		public static int rand(int bound)
		{
			return rnd.Next(bound);
		}

		/**
         * Turn x,y coordinates in Id to be used in a linear structure. This means we can access a specific tile directly,
         * without scanning the entire level.
         *
         * @param x     Horizontal coordinate (zero-based)
         * @param y     Vertical coordinate (zero-based)
         * @param width Width of the simulated matrix
         * @return Unique zero-based index of the tile at the given coordinate x,y.
         */
		public static long getIndex(int x, int y, int width)
		{
			return (long)y * width + x;
		}

		//endregion Static parameters ============

		//========================================


	}
}

