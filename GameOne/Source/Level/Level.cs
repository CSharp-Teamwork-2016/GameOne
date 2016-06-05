namespace GameOne.Source.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GameOne.Source.Entities;
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Level
    {
        // Contains a collection of Tiles that define the geometry (collision map) and a collection of entities, including the player
        private Player player;

        private List<Entity> entities;

        private List<Tile> geometry;

        private Dictionary<long, Tile> geometryMap;

        private static int currentLevel = 1;

        public Level()
        {
            this.player = new Player(0, 0, 0f);
            this.entities = new List<Entity>();
            this.geometry = new List<Tile>();
            this.geometryMap = new Dictionary<long, Tile>();
            this.entities.Add(this.player);
        }

        public void NextLevel()
        {
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

            var onlyValidTiles = this.geometry.Where(tile => tile.GetTileType() == TileType.Floor).ToArray();

            Tile currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];

            // produce EndKey
            Item itemEndKey = new Item(ItemType.EndKey, currentTile.GetX(), currentTile.GetY(), 0, 1, new Spritesheet());
            this.entities.Add(itemEndKey);

            for (int i = 0; i < items; i++)
            {
                // produce other items "no EndKey"
                currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];
                int enumItemValue = rnd.Next(1, 3);
                Item item = new Item((ItemType)enumItemValue, currentTile.GetX(), currentTile.GetY(), 0, 1, new Spritesheet());
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

            var onlyValidTiles = this.geometry.Where(tile => tile.GetTileType() == TileType.Floor).ToArray();

            for (int i = 0; i < enemies; i++)
            {
                // produce other items "no EndKey" randomly
                Tile currentTile = onlyValidTiles[rnd.Next(0, this.geometry.Count)];
                int enumEnemyValue = rnd.Next(1, 5);
                Enemy enemy = new Enemy((EnemyType)enumEnemyValue, currentTile.GetX(), currentTile.GetY(), 0, 2, new Spritesheet(), 50, 5, 0); // hardcoded values for enemy
                this.entities.Add(enemy);
            }
        }
    }
}
