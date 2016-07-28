namespace GameOne.Source.Entities
{
    using System.Windows;
    using Enumerations;
    using Interfaces;
    using Renderer;

    public class Projectile : Model, IMovable
    {
        #region Fields

        private readonly Vector velocity;
        private ProjectileType type; // for later use

        #endregion Fields

        #region Constructors

        public Projectile(double x, double y, double direction, double radius, Spritesheet sprite, Character source, Vector velocity)
            : base(x, y, direction, radius, sprite)
        {
            this.Source = source;
            this.velocity = velocity;
        }

        #endregion Constructors
        
        #region Properties

        public Character Source { get; }

        #endregion Properties

        #region Methods

        public override void Update(double time)
        {
            if (this.Alive)
            {
                this.Position += this.velocity * time;
            }
        }

        #endregion Methods
    }
}