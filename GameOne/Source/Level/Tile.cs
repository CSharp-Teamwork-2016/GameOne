namespace GameOne.Source.Level
{
    using GameOne.Source.Enumerations;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Tile
	{
        // 1x1 square from the playable level
        // Can be either transparent (floor, no collisions), opaque (walls) or clip (mobs collide, projectiles and effects pass trough)
        // Must implement IRenderable, for z-level sorting by the same method as Entities
        // Chlid classes may include active level elements, like doors, buttons or traps

        private int id;

        private static int nextId;

        private double x, y;

        private TileType tileType;

        private bool transparent = false;

        private string texture;

        public Tile(double x, double y, TileType tileType, string textureName)
        {
            this.x = x;
            this.y = y;
            this.id = nextId++;
            this.tileType = tileType;
            this.texture = textureName;
        }

        public void SetTileType(TileType tileType)
        {
            this.tileType = tileType;
        }

        public TileType GetTileType()
        {
            return this.tileType;
        }

        public void SetTransparancy()
        {
            this.transparent = true;
        }

        public double GetX()
        {
            return this.x;
        }

        public double GetY()
        {
            return this.y;
        }

        public bool IsTransparent()
        {
            return this.transparent;
        }

    }
}
