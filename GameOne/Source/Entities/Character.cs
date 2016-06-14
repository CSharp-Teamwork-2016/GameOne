﻿namespace GameOne.Source.Entities
{
    using System;
    using System.Windows;
    using System.Linq;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public abstract class Character : Model
    {
        protected int health;
        protected int damage;
        private Vector velocity;
        private AttackType attackType;
        private double attackTime;
        private double damageTime;

        protected Character(
            double x, double y, double direction, double radius,
            Spritesheet sprite, int health, int damage,
            AttackType attackType = AttackType.Melee) : base(x, y, direction, radius, sprite)
        {
            velocity = new Vector(0, 0);

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
            Direction = 1.5 * Math.PI;
            velocity.Y = -3;
        }
        public void MoveDown()
        {
            Direction = 0.5 * Math.PI;
            velocity.Y = 3;
        }
        public void MoveLeft()
        {
            Direction = Math.PI;
            velocity.X = -3;
        }
        public void MoveRight()
        {
            Direction = 0;
            velocity.X = 3;
        }
        public void MoveForward()
        {
            double x = 3 * Math.Cos(Direction);
            double y = 3 * Math.Sin(Direction);
            velocity.X = x;
            velocity.Y = y;
        }

        #endregion

        public void TakeDamage(int damage)
        {
            if (state == State.HURT) return;
            state = State.HURT;
            damageTime = 0;
            this.health -= damage;
            if (health <= 0) Die();
        }

        public void Attack()
        {
            if (state != State.ATTACK)
            {
                state = State.ATTACK;
                attackTime = 0;
            }
        }

        public bool IsDead()
        {
            return this.Health <= 0;

            //if (this.Health <= 0)
            //{
            //    return false;
            //}

            //return true;
        }

        public override void Update(double time)
        {
            if (state == State.DEAD) return;
            if (state == State.ATTACK)
            {
                attackTime += time;
                if (attackTime >= 0.3)
                {
                    state = State.IDLE;
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
            if (state == State.HURT)
            {
                damageTime += time;
                if (damageTime >= 0.4)
                {
                    damageTime = 0;
                    state = State.IDLE;
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
                if (velocity.Length <= friction.Length)
                {
                    velocity.X = 0;
                    velocity.Y = 0;
                }
                else if (velocity.Length < 0.1)
                {
                    velocity.X = 0;
                    velocity.Y = 0;
                }
            }
        }

        protected void Die()
        {
            state = State.DEAD;
        }
    }
}