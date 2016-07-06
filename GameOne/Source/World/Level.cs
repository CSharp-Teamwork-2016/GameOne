namespace GameOne.Source.World
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Entities;
    using Enumerations;
    using Renderer;

    public class Level
    {
        // Contains a collection of Tiles that define the geometry (collision map) and a collection of entities, including the player

        #region Fields

        public static int CurrentLevel = 4;

        private Item exitPortal;
        private List<Tile> validFloor;
        private Dictionary<long, Tile> geometryMap;
        private LevelMaker generator;

        #endregion Fields

        //===================================================================

        #region Constructors

        public Level()
        {
            LevelMaker.Init();
            this.generator = new LevelMaker(CurrentLevel);
            this.GenerateGeometry();
            this.Player = new Player(5, 5, 0.0);
            this.Entities = new List<Entity>();
            this.Entities.Add(this.Player);
            SetStart();
            SpawnItems();
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        public bool ExitOpen { get; set; }

        public int EnemyCount { get; set; }

        public bool ExitTriggered { get; set; }

        public List<Tile> Geometry { get; private set; }

        public List<Entity> Entities { get; private set; }

        public Player Player { get; }

        public Item ExitPortal => this.exitPortal;

        #endregion Properties

        //===================================================================

        #region Methods

        private void GenerateGeometry()
        {
            this.Geometry = this.generator.Tiles.Values.ToList();
            this.geometryMap = this.generator.Tiles;

            validFloor = new List<Tile>();
            foreach (Tile tile in this.Geometry.Where(t => t.TileType == TileType.Floor))
            {
                validFloor.Add(tile);
            }
        }

        public void NextLevel()
        {
            ExitOpen = false;
            ExitTriggered = false;
            EnemyCount = 0;
            CurrentLevel = generator.NextLevel();
            GenerateGeometry();
            this.Entities = new List<Entity>();
            this.Entities.Add(this.Player);
            SetStart();
            SpawnItems();
        }

        /// <summary>
        /// Method spawn items in random places on the map. Key is always produced for every level.
        /// ItemsType has 2 other enum values, so rnd is in range 1-3
        /// </summary>
        private void SpawnItems()
        {
            int items = 1 + (int)Math.Sqrt(CurrentLevel);

            for (int i = 0; i < items; i++)
            {
                Tile currentTile = GetRandomTile();
                // 30% chance for Quartz Flask (inventory potion)
                ItemType type = LevelMaker.RandDouble() > 0.7 ? ItemType.QuartzFlask : ItemType.PotionHealth;
                Item item = new Item(currentTile.X, currentTile.Y, 0, 0.2, new Spritesheet(), type);
                this.Entities.Add(item);
            }
        }

        /// <summary>
        /// Method spawn Enemies in random places on the map. 
        /// EnemyType has 4 enum values, so rnd is in range 1-5
        /// Value properties for enemies are hardcoded
        /// </summary>
        private void SpawnEnemies(Room room)
        {
            List<Tile> validTiles = new List<Tile>();
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    validTiles.Add(geometryMap[generator.GetIndex(x, y)]);
                }
            }
            validTiles = validTiles.Where(tile => tile.TileType == TileType.Floor).ToList();
            int enemies = 1 + (int)Math.Sqrt(CurrentLevel);
            int damage = 1 + CurrentLevel;
            int HP = 50 + CurrentLevel * 2;
            for (int i = 0; i < enemies; i++)
            {
                Tile currentTile = GetRandomTile(validTiles);
                double direction = Math.Round(Math.PI / 2 * LevelMaker.Rand(4), 2);
                Enemy enemy = new Enemy(currentTile.X, currentTile.Y, direction, 0.3, new Spritesheet(),
                    HP, damage, AttackType.Melee, EnemyType.Zombie, HP); // hardcoded values for enemy
                this.Entities.Add(enemy);
                EnemyCount++;
            }
        }

        private Tile GetRandomTile()
        {
            int next = LevelMaker.Rand(validFloor.Count);
            Tile result = validFloor[next];
            validFloor.Remove(result);
            return result;
        }

        private Tile GetRandomTile(List<Tile> tiles)
        {
            int next = LevelMaker.Rand(tiles.Count);
            Tile result = tiles[next];
            tiles.Remove(result);
            return result;
        }

        // Pick starting position inside the maze and place player there
        private void SetStart()
        {
            Partition start = generator.Root.LeftLeaf;
            Partition end = generator.Root.RightLeaf;
            List<Room> rooms = new List<Room>();

            Queue<Partition> startLeaf = new Queue<Partition>();
            startLeaf.Enqueue(start);
            while (startLeaf.Count > 0)
            {
                start = startLeaf.Dequeue();
                if (start.HasLeaves)
                {
                    startLeaf.Enqueue(start.RightLeaf);
                    startLeaf.Enqueue(start.LeftLeaf);
                }
                else if (startLeaf.Count > 0)
                {
                    rooms.Add(start.Room);
                }
            }
            Player.Position = new System.Windows.Vector(start.Room.OriginX, start.Room.OriginY);

            Queue<Partition> endLeaf = new Queue<Partition>();
            endLeaf.Enqueue(end);
            while (endLeaf.Count > 0)
            {
                end = endLeaf.Dequeue();
                if (end.HasLeaves)
                {
                    endLeaf.Enqueue(end.LeftLeaf);
                    endLeaf.Enqueue(end.RightLeaf);
                }
                else if (endLeaf.Count > 0)
                {
                    rooms.Add(end.Room);
                }
            }
            // produce EndKey
            exitPortal = new Item(end.Room.OriginX, end.Room.OriginY, 0, 0.3, new Spritesheet(), ItemType.EndKey);

            foreach (Room room in rooms)
            {
                SpawnEnemies(room);
            }
        }

        public void EnemySlain()
        {
            EnemyCount--;
            if (EnemyCount == 0 && !ExitOpen)
            {
                ExitOpen = true;
            }
        }

        public void SetExit()
        {
            this.Entities.Add(exitPortal);
        }

        #endregion Methods
    }
}