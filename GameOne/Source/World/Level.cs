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

        private int width;
        private int height;
        
        private Player player;
        private List<Entity> entities;

        private List<Tile> geometry;
        private Dictionary<long, Tile> geometryMap;

        public Level(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.player = new Player(5, 5, 0.0);
            this.entities = new List<Entity>();
            this.geometry = new List<Tile>();
            this.entities.Add(this.player);
            this.entities.Add(new Enemy(8, 8, 0.0, 0.3, new Spritesheet(), 100, 10, AttackType.Melee, EnemyType.Zombie, 0));
            this.entities.Add(new Enemy(15, 12, 0, 0.3, new Spritesheet(), 100, 10, AttackType.Melee, EnemyType.Zombie, 0));
            this.GenerateGeometry();
            this.geometryMap = new Dictionary<long, Tile>();
        }

        public List<Tile> Geometry
		{
			get
			{
				return geometry;
			}
		}

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
			geometry = LevelMaker.Generate();
			/*
            foreach (Tile tile in this.geometry)
            {
                long uniqueKey = this.GetUniqueKey(tile.GetX(), tile.GetY());
                this.geometryMap.Add(uniqueKey, tile);
            }
			*/
        }
       
        public void ProcessWalls()
        {
            for (int col = 0; col < this.width; col++)
            {
                for (int row = 0; row < this.height; row++)
                {
                    if (col == 0 || col == this.width - 1 || row == 0 || row == this.height - 1)
                    {
                        long uniqueKey = this.GetUniqueKey(col, row);
                        if (!this.geometryMap.ContainsKey(uniqueKey))
                        {
                            this.geometryMap.Add(uniqueKey, TileFactory.getTile(col, row, TileType.Wall));
                        }
                    }
                    else
                    {
                        long uniqueKey = this.GetUniqueKey(col, row);
                        if (!this.geometryMap.ContainsKey(uniqueKey))
                        {
                            this.geometryMap.Add(uniqueKey, TileFactory.getTile(col, row, TileType.Floor));
                        }
                    }
                }
            }
        }

        private long GetUniqueKey(double col, double row)
        {
            return (long)((this.width * row + col));
        }

        public void NextLevel()
        {
            currentLevel++;

            // TODO
        }

        public Player GetPlayer()
        {
            return this.player;
        }

        public List<Entity> GetEntities()
        {
            return this.entities;
        }

        public List<Tile> GetGeometry()
        {
            return this.geometry;
        }

        /// <summary>
        /// Method spawn items in random places on the map. Key is always produced for every level.
        /// ItemsType has 2 other enum values, so rnd is in range 1-3
        /// </summary>
        private void SpawnItems()
        {
            int items = 1 + (int)Math.Sqrt(currentLevel);
            Random rnd = new Random();

            var onlyValidTiles = this.geometry.Where(tile => tile.TileType == TileType.Floor).ToArray();

            Tile currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];

            // produce EndKey
            Item itemEndKey = new Item(currentTile.X, currentTile.Y, 0, 1, new Spritesheet(), ItemType.EndKey);
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
                // produce other items "no EndKey" randomly
                Tile currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];
                int enumEnemyValue = rnd.Next(1, 5);
                Enemy enemy = new Enemy(currentTile.X, currentTile.Y, 0, 2, new Spritesheet(), 
                    50, 5, AttackType.Melee, (EnemyType)enumEnemyValue, 0); // hardcoded values for enemy
                this.entities.Add(enemy);
            }
        }
    }
}
