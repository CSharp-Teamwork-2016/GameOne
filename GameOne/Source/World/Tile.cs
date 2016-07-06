namespace GameOne.Source.World
{
    using Enumerations;

    public class Tile
    {
        // 1x1 square from the playable level
        // Can be either transparent (floor, no collisions), opaque (walls) or clip (mobs collide, projectiles and effects pass trough)
        // Must implement IRenderable, for z-level sorting by the same method as Entities
        // Chlid classes may include active level elements, like doors, buttons or traps

        #region Fields

        private static int nextId; // needs to be initialized to 0
        private int id;

        private bool transparent;
        private string texture; // TODO

        #endregion Fields

        //===================================================================

        #region Constructors

        public Tile(double x, double y, TileType tileType, bool transparent = false)
        {
            this.X = x;
            this.Y = y;
            this.id = nextId++;
            this.TileType = tileType;
            this.transparent = transparent;
            // this.texture = textureName;
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        public double X { get; }

        public double Y { get; }

        public TileType TileType { get; set; }

        #endregion Properties

        //===================================================================

        #region Methods

        public void Overlay() // if overlay - change transparency
        {
            this.transparent = true;
        }

        public bool IsTransparent()
        {
            return this.transparent;
        }

        #endregion Methods
    }
}
