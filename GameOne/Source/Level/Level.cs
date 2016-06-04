namespace GameOne.Source.Level
{
    using System;
    using System.Collections.Generic;

    using GameOne.Source.Entities;
    using GameOne.Source.Enumerations;

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
            this.entities = new List<Entity>();
            this.player = new Player(0, 0, 0f);
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

        private void SpawnItems()
        {
            int items = 1 + (int)Math.Sqrt(currentLevel);
            Random rnd = new Random();
            Tile a = new Tile(1,2, TileType.Door);

            // TODO
        }
    }
}
