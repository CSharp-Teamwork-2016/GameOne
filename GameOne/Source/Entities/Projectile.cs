namespace GameOne.Source.Entities
{
    using System.Windows;
    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public class Projectile : Model, IMovable
    {
        #region Fields

        private Vector velocity;
        private ProjectileType type; // for later use

        #endregion Fields

        #region Constructors

        public Projectile(double x, double y, double direction, double radius, IRenderingStrategy sprite, ICharacter source, Vector velocity)
            : base(x, y, direction, radius, sprite)
        {
            this.Source = source;
            this.velocity = velocity;
            this.IsSolid = false;
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
            set
            {
                this.velocity = value;
            }
        }

        public bool IsSolid { get; private set; }

        public CollisionResponse CollisionResponse
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

        public MovementType MovementType
        {
            get
            {
                return MovementType.Frictionless;
            }
        }

        #endregion Properties

        public void Respond(ICollidable model)
        {
            if (model is Enemy && this.Source is Enemy) return;
            if (model is Player && this.Source is Player) return;
            if (!model.IsSolid) return;
            Die();
        }
    }
}