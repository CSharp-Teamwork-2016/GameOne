namespace GameOne.Source.Entities
{
    using System;
    using System.Windows;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public abstract class Character : Model
    {
        private int health;
        private int damage;
        private Vector velocity;
        private AttackType attackType;

        protected Character(double x, double y, double direction, double radius,Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.velocity = new Vector(0, 0);

            this.health = health;
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

            set
            {
                this.health = value;
            }
        }

        public int Damage
        {
            get
            {
                return this.damage;
            }

            set
            {
                this.damage = value;
            }
        }

        public AttackType AttackType
        {
            get
            {
                return this.attackType;
            }

            protected set
            {
                this.attackType = value;
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

        public void TakeDamage(int damage)
        {
            this.health -= damage;
        }

        public void ProduceAttack(Character enemy)
        {
            enemy.health -= this.damage;
        }

        public bool IsDead()
        {
            return this.Health <= 0;
        }

        public override void Update(double time)
        {
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
    }
}