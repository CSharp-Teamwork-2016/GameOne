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

        private static int currentLevel = 1;

        private Player player;
        private List<Entity> entities;
        private List<Tile> geometry;
        private Dictionary<long, Tile> geometryMap;
        LevelMaker generator;

        public Level()
        {
            this.GenerateGeometry();
            this.player = new Player(5, 5, 0.0);

            this.entities = new List<Entity>();
            this.entities.Add(this.player);
            SpawnEnemies();
            this.geometryMap = new Dictionary<long, Tile>();
        }

        public List<Tile> Geometry => this.geometry;

        public List<Entity> Entities
        {
            get
            {
                return entities;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }
        }

        private void GenerateGeometry()
        {
            LevelMaker.Init();
            generator = new LevelMaker(4);
            geometry = generator.Tiles.Values.ToList();
            /*
            foreach (Tile tile in this.geometry)
            {
                long uniqueKey = this.GetUniqueKey(tile.GetX(), tile.GetY());
                this.geometryMap.Add(uniqueKey, tile);
            }
			*/
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
            Random rnd = new Random();

            var onlyValidTiles =
                this.geometry.Where(tile => tile.TileType == TileType.Floor).ToArray();

            Tile currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];

            // produce EndKey
            Item itemEndKey =
                new Item(
                    currentTile.X, currentTile.Y, 0, 1, new Spritesheet(), ItemType.EndKey);
            this.entities.Add(itemEndKey);

            for (int i = 0; i < items; i++)
            {
                // produce other items "no EndKey"
                currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];
                int enumItemValue = rnd.Next(1, 3);
                Item item = new Item(currentTile.X, currentTile.Y, 0, 1, new Spritesheet(), (ItemType)enumItemValue);
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
            Random rnd = new Random();

            var onlyValidTiles = this.geometry.Where(tile => tile.TileType == TileType.Floor).ToArray();

            for (int i = 0; i < enemies; i++)
            {
                Tile currentTile = onlyValidTiles[rnd.Next(0, onlyValidTiles.Length)];
                int enumEnemyValue = rnd.Next(1, 5);
                Enemy enemy = new Enemy(currentTile.X, currentTile.Y, 0, 0.3, new Spritesheet(),
                    50, 5, AttackType.Melee, (EnemyType)enumEnemyValue, 0); // hardcoded values for enemy
                this.entities.Add(enemy);
            }
        }
    }
}