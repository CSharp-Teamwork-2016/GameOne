namespace GameOne.Source.World
{
    using Enumerations;
    using Interfaces;
    using System;
    using System.Windows;

    [Serializable]
    public class Tile : ICollidable
    {
        // 1x1 square from the playable level
        // Can be either transparent (floor, no collisions), opaque (walls) or clip (mobs collide, projectiles and effects pass trough)
        // Must implement IRenderable, for z-level sorting by the same method as Entities
        // Chlid classes may include active level elements, like doors, buttons or traps

        #region Fields

        private static int nextId; // needs to be initialized to 0
        private int id;

        private bool transparent;

        #endregion Fields

        #region Constructors

        public Tile(double x, double y, TileType tileType, bool transparent = false)
        {
            this.X = x;
            this.Y = y;
            this.id = nextId++;
            this.TileType = tileType;
            this.transparent = transparent;
        }

        #endregion Constructors

        #region Properties

        public double X { get; private set; }

        public double Y { get; private set; }

        public TileType TileType { get; set; }

        public bool IsSolid => true;

        public CollisionResponse CollisionResponse => CollisionResponse.Immovable;

        public Shape CollisionShape => Shape.Rectangle;

        public double Direction => 0;

        public Vector Position
        {
            get
            {
                return new Vector(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public double Radius => 0.5;

        public Rect BoundingBox
        {
            get
            {
                return new Rect(this.X - 0.5, this.Y - 0.5, 1, 1);
            }
        }

        public bool Alive => true;

        #endregion Properties

        #region Methods

        public void Overlay() // if overlay - change transparency
        {
            this.transparent = true;
        }

        public bool IsTransparent()
        {
            return this.transparent;
        }

        public void Respond(ICollidable model)
        {
        }

        public void Die()
        {
        }

        #endregion Methods
    }
}
