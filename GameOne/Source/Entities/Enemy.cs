﻿namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Enemy : Character
    {
        private double elapsedTime;
        private double nextTime;
        private EnemyType type;
        private int xpAward;

        public Enemy(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            this.PrepareNext();
        }

        private void PrepareNext()
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(1,3);
        }

        public override void Update(double time)
        {
            this.elapsedTime += time;
            if (this.elapsedTime >= this.nextTime)
            {
                PrepareNext();
                Direction = System.Math.PI / 2 * World.LevelMaker.Rand(4);
                MoveForward();
            }
            base.Update(time);
        }

        public override void Render()
        {
            // TODO
        }

        protected override void Die()
        {
            base.Die();
            Loop.level.Player.GainXP(xpAward);
            Loop.level.EnemySlain();
        }
    }
}