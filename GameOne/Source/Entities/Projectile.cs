namespace GameOne.Source.Entities
{
    using System.Windows;
    using Enumerations;
    using Interfaces;

    public class Projectile : Model, IMovable, IUpdatable
    {
        #region Fields

        private readonly Vector velocity;
        private ProjectileType type; // for later use

        #endregion Fields

        #region Constructors

        public Projectile(double x, double y, double direction, double radius, IRenderingStrategy sprite, ICharacter source, Vector velocity)
            : base(x, y, direction, radius, sprite)
        {
            this.Source = source;
            this.velocity = velocity;
            this.IsSolid = true;
        }

        #endregion Constructors

        #region Properties

        public ICharacter Source { get; }

        public Vector Velocity
        {
            get
            {
                return this.velocity;
            }
        }

        public bool IsSolid { get; private set; }

        public CollisionResponse Response
        {
            get
            {
                return CollisionResponse.DestroyOnImpact;
            }
        }

        public Shape CollisionShape
        {
            get
            {
                return Shape.Circle;
            }
        }

        #endregion Properties

        #region Methods

        public void Update(double time)
        {
            if (this.Alive)
            {
                this.Position += this.velocity * time;
            }
        }

        #endregion Methods
    }
}