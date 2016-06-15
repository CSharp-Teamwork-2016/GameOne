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

        public int Depth => this.depth;

        public Partition Root => this.bsp;

        public Dictionary<long, Tile> Tiles => this.levelMatrix;

        #endregion Properties ===================

        //========================================

        #region Initialization ==================

        public LevelMaker(int depth)
        {
            this.depth = depth;
            this.CalcSize();
            this.MakeLevel();
        }

        public void NextLevel()
        {
            this.depth += (int)Math.Max(2, this.depth * 0.2);
            this.CalcSize();
            this.MakeLevel();
        }

        public void MakeLevel()
        {
            this.bsp = new Partition(0, 0, this.width, this.height, null);
            this.queue = new Queue<Partition>();
            this.queue.Enqueue(this.bsp);
            for (int i = 0; i < this.depth; i++)
            {
                Partition current = this.queue.Dequeue();
                if (current.TrySplit())
                {
                    this.queue.Enqueue(current.LeftLeaf);
                    this.queue.Enqueue(current.RightLeaf);
                }
            }

            this.bsp.MakeRoom();
            this.bsp.MakeHallway();
            this.ProcessTiles();
            //SetTransparancy();
        }

        private void CalcSize()
        {
            this.width = REFSIZE * (int)Math.Sqrt(this.depth + 1);
            this.height = (int)(this.width * 0.67);
        }

        #endregion Initialization ===============

        //========================================

        #region Extract geometry ================

        public List<Partition> GetBsp()
        {
            var partitions = new List<Partition>();
            var q = new Queue<Partition>();

            q.Enqueue(this.bsp);

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

            q.Enqueue(this.bsp);
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

            q.Enqueue(this.bsp);
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
            this.levelMatrix = new Dictionary<long, Tile>();

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
                            if (!this.levelMatrix.ContainsKey(currentIndex))
                            {
                                this.levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 1));
                            }
                        }
                        else
                        {
                            this.levelMatrix[currentIndex] = TileFactory.getTile(col, row, 2);
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
                            if (!this.levelMatrix.ContainsKey(currentIndex))
                            {
                                this.levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 1));
                            }
                        }
                        else
                        {
                            this.levelMatrix[currentIndex] = TileFactory.getTile(col, row, 2);
                        }
                    }
                }
            }
        }

        private void SetTransparancy()
        {
            var levelMatrixValues = new List<Tile>(this.levelMatrix.Values);
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
                if (this.levelMatrix.ContainsKey(currentIndex - this.width) &&
                        this.levelMatrix[currentIndex - this.width].TileType == TileType.Floor)
                {
                    tile.Overlay();
                }
                else if (this.levelMatrix.ContainsKey(currentIndex - this.width * 2) &&
                      this.levelMatrix[currentIndex - this.width * 2].TileType == TileType.Floor)
                {
                    tile.Overlay();
                }
            }
        }

        public long GetIndex(int x, int y)
        {
            return (long)y * width + x;
        }

        #endregion Extract geometry =============

        //========================================

        #region Static parameters ===============

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
            return lower + rnd.NextDouble() * (upper - lower);
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
            return (long)y * width + x;
        }

        #endregion Static parameters ============

        //========================================


    }
}

