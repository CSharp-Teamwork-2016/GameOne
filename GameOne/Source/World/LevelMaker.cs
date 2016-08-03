namespace GameOne.Source.World
{
    using System;
    using System.Collections.Generic;
    using Enumerations;
    using Factories;

    /// <summary>
    /// Temporary geometry generator; to be replaced with file loader
    /// <para>/!\ DO NOT MODIFY /!\</para>
    /// </summary>
    [Serializable]
    internal class LevelMaker
    {
        #region Fields

        public static int Minsize = 7;
        public static int Maxsize = 15;
        public const float Minratio = 0.33f;
        public const float Maxratio = 0.66f;
        public const int Hallsize = 5;
        private static Random rnd;
        private const int Refsize = 15;

        private int width;
        private int height;
        private Queue<Partition> queue;

        #endregion Fields

        #region Constructors

        public LevelMaker(int depth)
        {
            this.Depth = depth;
            this.CalcSize();
            this.MakeLevel();
        }

        #endregion Constructors

        #region Properties

        public int Depth { get; private set; }

        public Partition Root { get; private set; }

        public Dictionary<long, Tile> Tiles { get; private set; }

        #endregion Properties

        #region Methods

        public static void Init()
        {
            rnd = new Random();
        }

        public static void Init(int seed)
        {
            rnd = new Random(seed);
        }

        public static int Rand(int bound)
        {
            return rnd.Next(bound);
        }

        public static double RandDouble()
        {
            return rnd.NextDouble();
        }

        public static double RandDouble(double bound)
        {
            return rnd.NextDouble() * bound;
        }

        public static double RandDouble(double lower, double upper)
        {
            return lower + (rnd.NextDouble() * (upper - lower));
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
        public static long GetIndex(int x, int y, int width)
        {
            return ((long)y * width) + x;
        }
        // Initialization
        public int NextLevel()
        {
            this.Depth += (int)Math.Max(2, this.Depth * 0.2);
            this.CalcSize();
            if (this.Depth == 10)
            {
                this.MakeBossLevel();
            }
            else
            {
                this.MakeLevel();
            }
            return this.Depth;
        }

        public void MakeLevel()
        {
            this.Root = new Partition(0, 0, this.width, this.height, null);
            this.queue = new Queue<Partition>();
            this.queue.Enqueue(this.Root);
            for (int i = 0; i < this.Depth; i++)
            {
                Partition current = this.queue.Dequeue();
                if (current.TrySplit())
                {
                    this.queue.Enqueue(current.LeftLeaf);
                    this.queue.Enqueue(current.RightLeaf);
                }
            }

            this.Root.MakeRoom();
            this.Root.MakeHallway();
            this.ProcessTiles();
            // SetTransparancy();
        }

        public void MakeBossLevel()
        {
            this.Root = new Partition(0, 0, 20, 35, null);
            Minsize = 12;
            Maxsize = 15;
            this.Root.TrySplit();
            this.Root.MakeRoom();
            this.Root.MakeHallway();
            this.ProcessTiles();
            Minsize = 7;
            Maxsize = 15;
        }


        // Extract geometry
        public List<Partition> GetBsp()
        {
            var partitions = new List<Partition>();
            var q = new Queue<Partition>();

            q.Enqueue(this.Root);

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

            q.Enqueue(this.Root);
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

            q.Enqueue(this.Root);
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
            this.Tiles = new Dictionary<long, Tile>();

            foreach (var room in this.GetRooms())
            {
                var startX = room.X;
                var startY = room.Y;
                var endX = startX + room.Width;
                var endY = startY + room.Height;

                for (int col = startX; col < endX; col++)
                {
                    for (int row = startY; row < endY; row++)
                    {
                        long currentIndex = GetIndex(col, row, this.width);
                        if (row == startY || row == endY - 1 || col == startX || col == endX - 1)
                        {
                            if (!this.Tiles.ContainsKey(currentIndex))
                            {
                                this.Tiles.Add(currentIndex, TileFactory.GetTile(col, row, 1));
                            }
                        }
                        else
                        {
                            this.Tiles[currentIndex] = TileFactory.GetTile(col, row, 2);
                        }
                    }
                }
            }

            foreach (var hallway in this.GetHallways())
            {
                var startX = hallway.X;
                var startY = hallway.Y;
                var endX = startX + hallway.Width;
                var endY = startY + hallway.Height;

                for (int col = startX; col < endX; col++)
                {
                    for (int row = startY; row < endY; row++)
                    {
                        long currentIndex = GetIndex(col, row, this.width);
                        if (row == startY || row == endY - 1 || col == startX || col == endX - 1)
                        {
                            if (!this.Tiles.ContainsKey(currentIndex))
                            {
                                this.Tiles.Add(currentIndex, TileFactory.GetTile(col, row, 1));
                            }
                        }
                        else
                        {
                            this.Tiles[currentIndex] = TileFactory.GetTile(col, row, 2);
                        }
                    }
                }
            }
        }

        public long GetIndex(int x, int y)
        {
            return ((long)y * this.width) + x;
        }

        private void CalcSize()
        {
            this.width = Refsize * (int)Math.Sqrt(this.Depth + 1);
            this.height = (int)(this.width * 0.67);
        }

        private void SetTransparancy()
        {
            var levelMatrixValues = new List<Tile>(this.Tiles.Values);
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
                long currentIndex = GetIndex((int)tile.X, (int)tile.Y, this.width);
                if (this.Tiles.ContainsKey(currentIndex - this.width) &&
                        this.Tiles[currentIndex - this.width].TileType == TileType.Floor)
                {
                    tile.Overlay();
                }
                else if (this.Tiles.ContainsKey(currentIndex - (this.width * 2)) &&
                      this.Tiles[currentIndex - (this.width * 2)].TileType == TileType.Floor)
                {
                    tile.Overlay();
                }
            }
        }
        #endregion Methods
    }
}