namespace GameOne.Source.Entities
{
    using System;
    using System.Linq;
    using System.Windows;
    using Enumerations;
    using Events;
    using Interfaces;
    using Renderer;
    using World;

    public abstract class Character : Model, IControlable, IMovable
    {
        #region Fields
        
        protected double timeToNextAction;
        private Vector velocity;
        private double attackTime;
        private double damageTime;

        #endregion Fields

        #region Constructors

        protected Character(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.velocity = new Vector(0, 0);

            this.Health = health;
            this.MaxHealth = health;
            this.Damage = damage;
            this.AttackType = attackType;
            this.timeToNextAction = 0;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ProjectileEventArgs> FireProjectileEvent;

        #endregion

        #region Properties

        public int Health { get; protected set; }

        public int MaxHealth { get; protected set; }

        public int Damage { get; protected set; }

        public AttackType AttackType { get; }

        #endregion Properties

        #region Methods

        #region Methods/Movement

        public void MoveUp()
        {
            this.Direction = Physics.UpDirection;
            this.velocity.Y = -3;
        }

        public void MoveDown()
        {
            this.Direction = Physics.DownDirection;
            this.velocity.Y = 3;
        }

        public void MoveLeft()
        {
            this.Direction = Physics.LeftDirection;
            this.velocity.X = -3;
        }

        public void MoveRight()
        {
            this.Direction = Physics.RightDirection;
            this.velocity.X = 3;
        }

        public void MoveForward()
        {
            double x = 3 * Math.Cos(this.Direction);
            double y = 3 * Math.Sin(this.Direction);
            this.velocity.X = x;
            this.velocity.Y = y;
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

            if ((this.state & State.ATTACK) != State.ATTACK)
            {
                this.state |= State.ATTACK;
                this.attackTime = 0;
                this.timeToNextAction = 0.6;
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

                ProjectileEventArgs args = new ProjectileEventArgs();
                args.Type = ProjectileType.Bullet;
                this.FireProjectileEvent(this, args);
            }
        }

        public override void Update(double time)
        {
            if (this.state == State.DEAD)
            {
                return;
            }

            if ((this.state & State.ATTACK) == State.ATTACK)
            {
                this.attackTime += time;

                if (this.attackTime >= 0.2)
                {
                    this.state ^= State.ATTACK;
                    this.attackTime = 0;
                    return;
                }

                foreach (Character entity in Loop.level.Entities.OfType<Character>().Where(e => e != this && (this.Position - e.Position).Length < 2))
                {
                    double p1x = this.Position.X + (Math.Cos(this.Direction + Math.PI / 2) * 0.5);
                    double p1y = this.Position.Y + (Math.Sin(this.Direction + Math.PI / 2) * 0.5);

                    double pw = (1.2 * Math.Cos(this.Direction)) + (1 * Math.Sin(this.Direction));
                    double ph = (1.2 * Math.Sin(this.Direction)) - (1 * Math.Cos(this.Direction));

                    double leftA = Math.Min(p1x, p1x + pw);
                    double rightA = Math.Max(p1x, p1x + pw);
                    double topA = Math.Min(p1y, p1y + ph);
                    double bottomA = Math.Max(p1y, p1y + ph);

                    if (entity.Position.X >= leftA && entity.Position.X <= rightA &&
                        entity.Position.Y >= topA && entity.Position.Y <= bottomA)
                    {
                        entity.TakeDamage(this.Damage);
                    }
                }
            }

            if ((this.state & State.HURT) == State.HURT)
            {
                this.damageTime += time;

                if (this.damageTime >= 0.4)
                {
                    this.damageTime = 0;
                    this.state ^= State.HURT;
                }
            }

            this.timeToNextAction -= time;

            if (this.timeToNextAction < 0)
            {
                this.timeToNextAction = 0;
            }

            // Motion
            if (this.velocity.Length > 0)
            {
                this.Position += this.velocity * time;
                // Friction
                Vector friction = this.velocity;

                friction.Negate();
                friction.Normalize();

                friction *= 15 * time;
                this.velocity += friction;

                if (this.velocity.Length <= friction.Length)
                {
                    this.velocity.X = 0;
                    this.velocity.Y = 0;
                }
                else if (this.velocity.Length < 0.1)
                {
                    this.velocity.X = 0;
                    this.velocity.Y = 0;
                }
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
            double x = -5 * Math.Cos(this.Direction);
            double y = -5 * Math.Sin(this.Direction);

            this.velocity.X = x;
            this.velocity.Y = y;
        }

        #endregion Methods
    }
}