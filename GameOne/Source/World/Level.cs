namespace GameOne.Source.World
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GameOne.Source.Entities;
    using GameOne.Source.Enumerations;
    using GameOne.Source.Factories;
    using GameOne.Source.Renderer;

    public class Level
    {
        // Contains a collection of Tiles that define the geometry (collision map) and a collection of entities, including the player

        private static int currentLevel = 4;

        private Player player;
        private Item exitPortal;
        public bool exitOpen = false;
        public int enemyCount = 0;
        public bool exitTriggered = false;
        private List<Entity> entities;
        private List<Tile> geometry;
        private List<Tile> validFloor;
        private Dictionary<long, Tile> geometryMap;
        LevelMaker generator;

        public Level()
        {
            LevelMaker.Init();
            this.generator = new LevelMaker(currentLevel);
            this.GenerateGeometry();
            this.player = new Player(5, 5, 0.0);
            this.entities = new List<Entity>();
            this.entities.Add(this.player);
            SetStart();
            SpawnItems();
        }

        public List<Tile> Geometry => this.geometry;

        public List<Entity> Entities => this.entities;

        public Player Player => this.player;

        private void GenerateGeometry()
        {
            this.geometry = this.generator.Tiles.Values.ToList();
            this.geometryMap = this.generator.Tiles;

            validFloor = new List<Tile>();
            foreach (Tile tile in this.geometry.Where(t => t.TileType == TileType.Floor))
            {
                validFloor.Add(tile);
            }
        }

        public void NextLevel()
        {
            exitOpen = false;
            exitTriggered = false;
            enemyCount = 0;
            generator.NextLevel();
            GenerateGeometry();
            this.entities = new List<Entity>();
            this.entities.Add(this.player);
            SetStart();
            SpawnItems();
        }

        /// <summary>
        /// Method spawn items in random places on the map. Key is always produced for every level.
        /// ItemsType has 2 other enum values, so rnd is in range 1-3
        /// </summary>
        private void SpawnItems()
        {
            int items = 1 + (int)Math.Sqrt(currentLevel);

            for (int i = 0; i < items; i++)
            {
                Tile currentTile = GetRandomTile();
                // 30% chance for Quartz Flask (inventory potion)
                ItemType type = LevelMaker.RandDouble() > 0.7 ? ItemType.QuartzFlask : ItemType.PotionHealth;
                Item item = new Item(currentTile.X, currentTile.Y, 0, 0.2, new Spritesheet(), type);
                this.entities.Add(item);
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
            int enemies = 1 + (int)Math.Sqrt(currentLevel);

            for (int i = 0; i < enemies; i++)
            {
                Tile currentTile = GetRandomTile(validTiles);
                double direction = Math.PI / 2 * LevelMaker.Rand(4);
                Enemy enemy = new Enemy(currentTile.X, currentTile.Y, direction, 0.3, new Spritesheet(),
                    50, 5, AttackType.Melee, EnemyType.Zombie, 50); // hardcoded values for enemy
                this.entities.Add(enemy);
                enemyCount++;
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
                    startLeaf.Enqueue(start.LeftLeaf);
                    startLeaf.Enqueue(start.RightLeaf);
                }
                else if (startLeaf.Count > 0)
                {
                    rooms.Add(start.Room);
                }
            }
            player.Position = new System.Windows.Vector(start.Room.OriginX, start.Room.OriginY);

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
            enemyCount--;
            if (enemyCount == 0 && !exitOpen)
            {
                exitOpen = true;
            }
        }

        public void SetExit()
        {
            this.entities.Add(exitPortal);
        }
    }
}