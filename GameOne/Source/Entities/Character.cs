namespace GameOne.Source.Entities
{
    using System;
    using System.Windows;

    using Enumerations;
    using EventArgs;
    using Interfaces;
    using World.Physics;

    [Serializable]
    public abstract class Character : Model, IControllable, ICharacter, IUpdatable
    {
        #region Fields

        protected double timeToNextAction;
        private Vector velocity;
        private double attackTime;
        private double damageTime;

        #endregion Fields

        #region Constructors

        protected Character(double x, double y, double direction, double radius, IRenderingStrategy sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.velocity = new Vector(0, 0);

            this.Health = health;
            this.MaxHealth = health;
            this.Damage = damage;
            this.AttackType = attackType;
            this.timeToNextAction = 0;

            this.IsSolid = true;
        }

        #endregion Constructors

        #region Events

        [field: NonSerialized]
        public event EventHandler<ProjectileEventArgs> FireProjectileEvent;
        [field: NonSerialized]
        public event EventHandler<MeleeAttackEventArgs> AttackEvent;

        #endregion

        #region Properties

        public int Health { get; protected set; }

        public int MaxHealth { get; protected set; }

        public int Damage { get; protected set; }

        public AttackType AttackType { get; }

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

        public MovementType MovementType
        {
            get
            {
                return MovementType.Normal;
            }
        }

        public bool IsSolid { get; private set; }

        public CollisionResponse CollisionResponse
        {
            get
            {
                return CollisionResponse.Project;
            }
        }

        public Shape CollisionShape
        {
            get
            {
                return Shape.Circle;
            }
        }

        protected virtual double VelocityModifier => 1;

        #endregion Properties

        #region Methods

        #region Methods/Movement

        public void MoveUp()
        {
            this.Direction = PhysicsEngine.UpDirection;
            this.velocity.Y = -PhysicsEngine.NominalVelocity * this.VelocityModifier;
        }

        public void MoveDown()
        {
            this.Direction = PhysicsEngine.DownDirection;
            this.velocity.Y = PhysicsEngine.NominalVelocity * this.VelocityModifier;
        }

        public void MoveLeft()
        {
            this.Direction = PhysicsEngine.LeftDirection;
            this.velocity.X = -PhysicsEngine.NominalVelocity * this.VelocityModifier;
        }

        public void MoveRight()
        {
            this.Direction = PhysicsEngine.RightDirection;
            this.velocity.X = PhysicsEngine.NominalVelocity * this.VelocityModifier;
        }

        public void MoveForward()
        {
            this.velocity = PhysicsEngine.NominalVelocity * PhysicsEngine.GetDirectedVector(this.Direction) * this.VelocityModifier;
        }

        public void TurnTo(double direction)
        {
            this.Direction = direction;
        }

        public void Accelerate(Vector acceleration)
        {
            this.velocity += acceleration;
        }

        public void Stop()
        {
            this.velocity = new Vector(0, 0);
        }

        #endregion Methods/Movement

        public virtual void TakeDamage(int damage)
        {
            if (this.state == State.DEAD)
            {
                return;
            }

            if ((this.state & State.HURT) == State.HURT)
            {
                return;
            }

            this.state |= State.HURT;
            this.damageTime = 0;
            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.Die();
            }
        }

        public void Attack()
        {
            if (this.timeToNextAction > 0)
            {
                return;
            }

            if (!this.state.HasFlag(State.ATTACK))
            {
                this.state |= State.ATTACK;
                this.attackTime = 0;
                this.timeToNextAction = 0.6;
                // Raise event
                MeleeAttackEventArgs args = new MeleeAttackEventArgs(this);
                AttackEvent(this, args);
            }
        }

        public void FireProjectile()
        {
            if (this.timeToNextAction > 0)
            {
                return;
            }

            if ((this.state & State.ATTACK) != State.ATTACK)
            {
                this.attackTime = 0;
                this.timeToNextAction = 0.3;

                // Raise event
                ProjectileEventArgs args = new ProjectileEventArgs();
                args.Type = ProjectileType.Bullet;
                this.FireProjectileEvent(this, args);
            }
        }

        public virtual void Update(double time)
        {
            // Don't update if not active
            if (this.state == State.DEAD)
            {
                return;
            }

            // Attack animation
            if (this.state.HasFlag(State.ATTACK))
            {
                this.attackTime += time;

                if (this.attackTime >= 0.2)
                {
                    this.state ^= State.ATTACK;
                    this.attackTime = 0;
                }
            }

            // Invincibility frames
            if ((this.state & State.HURT) == State.HURT)
            {
                this.damageTime += time;

                if (this.damageTime >= 0.4)
                {
                    this.damageTime = 0;
                    this.state ^= State.HURT;
                }
            }

            // Ability cooldown
            this.timeToNextAction -= time;
            if (this.timeToNextAction < 0)
            {
                this.timeToNextAction = 0;
            }
        }

        public void Heal(int amount)
        {
            this.Health += amount;
            if (this.Health > this.MaxHealth)
            {
                this.Health = this.MaxHealth;
            }
        }

        public void Knockback()
        {
            // TODO: knockback should be opposite source position, not based on target direction
            this.velocity = -PhysicsEngine.NominalVelocity * PhysicsEngine.GetDirectedVector(this.Direction);
        }

        public abstract void Respond(ICollidable model);

        #endregion Methods
    }
}