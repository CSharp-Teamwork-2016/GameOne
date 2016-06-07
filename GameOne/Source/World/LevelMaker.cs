using System;
using System.IO;
using System.Linq;
using GameOne.Source.Enumerations;
using GameOne.Source.Factories;

namespace GameOne.Source.Level
{
    using System.Collections.Generic;
    using World;
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
        private LinkedList<Partition> queue;
        private Dictionary<long, Tile> levelMatrix;

        //========================================

        //region Properties

        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {

            }
        }

        public Partition Root
        {
            get
            {
                return bsp;
            }
            set
            {

            }
        }

        public Dictionary<long, Tile> Tiles
        {
            get
            {
                return levelMatrix;
            }
            set
            {

            }
        }

        //endregion Properties ===================

        //========================================

        //region Initialization ==================

        public LevelMaker(int depth)
        {
            this.depth = depth;
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
            queue = new LinkedList<Partition>();
            queue.AddLast(bsp);
            for (int i = 0; i < depth; i++)
            {
                Partition current = queue.RemoveFirst();
                if (current.TrySplit())
                {
                    queue.AddLast(current.LeftLeaf);
                    queue.AddLast(current.RightLeaf);
                }
            }

            bsp.MakeRoom();
            bsp.MakeHallway();
            ProcessTiles();
            SetTransparancy();
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
            var q = new LinkedList<Partition>();

            q.AddLast(bsp);

            while (q.Count > 0)
            {
                Partition current = q.RemoveFirst();
                if (current.HasLeaves)
                {
                    q.AddLast(current.LeftLeaf);
                    q.AddLast(current.RightLeaf);
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
            var q = new LinkedList<Partition>();

            q.AddLast(bsp);
            while (q.Count > 0)
            {
                Partition current = q.RemoveFirst();
                if (current.HasLeaves)
                {
                    q.AddLast(current.LeftLeaf);
                    q.AddLast(current.RightLeaf);
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
            var q = new LinkedList<Partition>();

            q.AddLast(bsp);
            while (q.Count > 0)
            {
                Partition current = q.RemoveFirst();
                if (current.HasLeaves)
                {
                    q.AddLast(current.LeftLeaf);
                    q.AddLast(current.RightLeaf);
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
                            levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 2));
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
                            levelMatrix.Add(currentIndex, TileFactory.getTile(col, row, 2));
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

        public static void init()
        {
            rnd = new Random();
        }

        public static void init(long seed)
        {
            rnd = new Random(seed);
        }

        public static int rand(int bound)
        {
            return rnd.Next(bound);
        }

        /**
         * Turn x,y coordinates in ID to be used in a linear structure. This means we can access a specific tile directly,
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

