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
        private List<Entity> entities;
        private List<Tile> geometry;
        private List<Tile> validFloor;
        private Dictionary<long, Tile> geometryMap;
        LevelMaker generator;

        public Level()
        {
            this.GenerateGeometry();
            this.player = new Player(5, 5, 0.0);
            SetStart();
            this.entities = new List<Entity>();
            this.entities.Add(this.player);
            this.SpawnEnemies();
            SpawnItems();
            this.geometryMap = new Dictionary<long, Tile>();
        }

        public List<Tile> Geometry => this.geometry;

        public List<Entity> Entities => this.entities;

        public Player Player => this.player;

        private void GenerateGeometry()
        {
            LevelMaker.Init();
            this.generator = new LevelMaker(currentLevel);
            this.geometry = this.generator.Tiles.Values.ToList();

            validFloor = new List<Tile>();
            foreach (Tile tile in this.geometry.Where(t => t.TileType == TileType.Floor))
            {
                validFloor.Add(tile);
            }
        }

        public void NextLevel()
        {
            currentLevel++;

            // TODO
        }

        /// <summary>
        /// Method spawn items in random places on the map. Key is always produced for every level.
        /// ItemsType has 2 other enum values, so rnd is in range 1-3
        /// </summary>
        private void SpawnItems()
        {
            int items = 1 + (int)Math.Sqrt(currentLevel);

            Tile currentTile = GetRandomTile();
            // produce EndKey
            Item itemEndKey =
                new Item(currentTile.X, currentTile.Y, 0, 0.3, new Spritesheet(), ItemType.EndKey);
            this.entities.Add(itemEndKey);

            for (int i = 0; i < items; i++)
            {
                // produce other items "no EndKey"
                currentTile = GetRandomTile();
                ItemType type = ItemType.PotionHealth;
                Item item = new Item(currentTile.X, currentTile.Y, 0, 0.2, new Spritesheet(), type);
                this.entities.Add(item);
            }
        }

        /// <summary>
        /// Method spawn Enemies in random places on the map. 
        /// EnemyType has 4 enum values, so rnd is in range 1-5
        /// Value properties for enemies are hardcoded
        /// </summary>
        private void SpawnEnemies()
        {
            int enemies = 1 + (int)Math.Sqrt(currentLevel);

            for (int i = 0; i < enemies; i++)
            {
                Tile currentTile = GetRandomTile();
                Enemy enemy = new Enemy(currentTile.X, currentTile.Y, 0, 0.3, new Spritesheet(),
                    50, 5, AttackType.Melee, EnemyType.Zombie, 0); // hardcoded values for enemy
                this.entities.Add(enemy);
            }
        }

        private Tile GetRandomTile()
        {
            int next = LevelMaker.Rand(validFloor.Count);
            Tile result = validFloor[next];
            validFloor.Remove(result);
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
        }
    }
}