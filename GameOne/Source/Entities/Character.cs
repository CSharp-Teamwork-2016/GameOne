namespace GameOne.Source.Entities
{
    using System;
    using System.Windows;
    using System.Linq;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public abstract class Character : Model
    {
        protected int health;
        protected int maxHealth;
        protected int damage;
        private Vector velocity;
        private AttackType attackType;
        private double attackTime;
        private double damageTime;

        protected Character(double x, double y, double direction, double radius,Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.velocity = new Vector(0, 0);

            this.health = health;
            this.maxHealth = health;
            this.damage = damage;
            this.attackType = attackType;
        }

        #region Properties

        public int Health
        {
            get
            {
                return this.health;
            }
        }

        public int Damage
        {
            get
            {
                return this.damage;
            }
        }

        public AttackType AttackType
        {
            get
            {
                return this.attackType;
            }
        }

        #endregion

        #region Movement

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

        #endregion

        public virtual void TakeDamage(int damage)
        {
            if ((state & State.HURT) == State.HURT) return;
            state |= State.HURT;
            damageTime = 0;
            this.health -= damage;
            if (health <= 0) Die();
        }

        public void Attack()
        {
            if ((state & State.ATTACK) != State.ATTACK)
            {
                state |= State.ATTACK;
                attackTime = 0;
            }
        }

        public bool IsDead()
        {
            return this.Health <= 0;
        }

        public override void Update(double time)
        {
            if (state == State.DEAD) return;
            if ((state & State.ATTACK) == State.ATTACK)
            {
                attackTime += time;
                if (attackTime >= 0.3)
                {
                    state ^= State.ATTACK;
                    attackTime = 0;
                    return;
                }
                foreach (Character entity in Loop.level.Entities.OfType<Character>().Where(e => e != this && (Position - e.Position).Length < 1.5))
                {
                    /*
                    if (Direction == 0)
                    {

                    }
                    else if (Direction == Math.PI)
                    */
                    entity.TakeDamage(damage);
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
            health += amount;
            if (health > maxHealth) health = maxHealth;
        }

        public void Knockback()
        {
            double x = -5 * Math.Cos(this.Direction);
            double y = -5 * Math.Sin(this.Direction);
            this.velocity.X = x;
            this.velocity.Y = y;
        }

        protected void Die()
        {
            state = State.DEAD;
        }
    }
}