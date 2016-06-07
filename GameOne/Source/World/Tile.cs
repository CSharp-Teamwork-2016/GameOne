namespace GameOne.Source.World
{
    using GameOne.Source.Enumerations;

    public class Tile
    {
        // 1x1 square from the playable level
        // Can be either transparent (floor, no collisions), opaque (walls) or clip (mobs collide, projectiles and effects pass trough)
        // Must implement IRenderable, for z-level sorting by the same method as Entities
        // Chlid classes may include active level elements, like doors, buttons or traps

        private static int nextId = 0; // needs to be initialized to 0
        private int id;

        private double x, y;
        private TileType tileType;
        private bool transparent;
        private string texture; // TODO

        public Tile(double x, double y, TileType tileType, bool transparent = false)
        {
            this.x = x;
            this.y = y;
            this.id = nextId++;
            this.tileType = tileType;
            this.transparent = transparent;
            // this.texture = textureName;
        }

        public double X
        {
            get
            {
                return this.x;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
        }

        public TileType TileType
        {
            get
            {
                return this.tileType;
            }
            set
            {
                this.tileType = value;
            }
        }

        public void Overlay() // if overlay - change transparency
        {
            this.transparent = true;
        }

        public bool IsTransparent()
        {
            return this.transparent;
        }
    }
}
