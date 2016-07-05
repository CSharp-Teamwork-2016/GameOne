namespace GameOne.Source.Entities
{
    using System;
    using System.Windows;
    using System.Linq;

    using Enumerations;
    using Renderer;

    public abstract class Character : Model
    {
        #region Fields

        private Vector velocity;
        private double attackTime;
        private double damageTime;
        protected double timeToNextAction;

        #endregion Fields

        //===================================================================

        #region Constructors

        protected Character(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.velocity = new Vector(0, 0);

            this.Health = health;
            this.MaxHealth = health;
            this.Damage = damage;
            this.AttackType = attackType;
            timeToNextAction = 0;
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        public int Health { get; protected set; }

        public int MaxHealth { get; protected set; }

        public int Damage { get; protected set; }

        public AttackType AttackType { get; }

        #endregion Properties

        //===================================================================

        #region Methods

        #region Methods/Movement

        public void MoveUp()
        {
            this.Direction = 1.5 * Math.PI;
            this.velocity.Y = -3;
        }

        public void MoveDown()
        {
            this.Direction = 0.5 * Math.PI;
            this.velocity.Y = 3;
        }

        public void MoveLeft()
        {
            this.Direction = Math.PI;
            this.velocity.X = -3;
        }

        public void MoveRight()
        {
            this.Direction = 0;
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
            if (state == State.DEAD)
            {
                return;
            }

            if ((state & State.HURT) == State.HURT)
            {
                return;
            }

            state |= State.HURT;
            damageTime = 0;
            this.Health -= damage;

            if (Health <= 0)
            {
                Die();
            }
        }

        public void Attack()
        {
            if (timeToNextAction > 0)
            {
                return;
            }

            if ((state & State.ATTACK) != State.ATTACK)
            {
                state |= State.ATTACK;
                attackTime = 0;
                timeToNextAction = 0.6;
            }
        }

        public override void Update(double time)
        {
            if (state == State.DEAD)
            {
                return;
            }

            if ((state & State.ATTACK) == State.ATTACK)
            {
                attackTime += time;

                if (attackTime >= 0.2)
                {
                    state ^= State.ATTACK;
                    attackTime = 0;
                    return;
                }

                foreach (Character entity in Loop.level.Entities.OfType<Character>().Where(e => e != this && (Position - e.Position).Length < 2))
                {
                    double p1x = Position.X + Math.Cos(Direction + Math.PI / 2) * 0.5;
                    double p1y = Position.Y + Math.Sin(Direction + Math.PI / 2) * 0.5;

                    double pw = 1.2 * Math.Cos(Direction) + 1 * Math.Sin(Direction);
                    double ph = 1.2 * Math.Sin(Direction) - 1 * Math.Cos(Direction);

                    double leftA = Math.Min(p1x, p1x + pw);
                    double rightA = Math.Max(p1x, p1x + pw);
                    double topA = Math.Min(p1y, p1y + ph);
                    double bottomA = Math.Max(p1y, p1y + ph);

                    if (entity.Position.X >= leftA && entity.Position.X <= rightA &&
                        entity.Position.Y >= topA && entity.Position.Y <= bottomA)
                    {
                        entity.TakeDamage(Damage);
                    }
                }
            }

            if ((state & State.HURT) == State.HURT)
            {
                damageTime += time;

                if (damageTime >= 0.4)
                {
                    damageTime = 0;
                    state ^= State.HURT;
                }
            }

            timeToNextAction -= time;

            if (timeToNextAction < 0)
            {
                timeToNextAction = 0;
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
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public void Knockback()
        {
            double x = -5 * Math.Cos(this.Direction);
            double y = -5 * Math.Sin(this.Direction);

            this.velocity.X = x;
            this.velocity.Y = y;
        }

        protected virtual void Die()
        {
            state = State.DEAD;
        }

        #endregion Methods
    }
}