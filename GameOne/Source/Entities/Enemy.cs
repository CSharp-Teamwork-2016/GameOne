namespace GameOne.Source.Entities
{
    using System.Collections.Generic;
    using System;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Enemy : Character
    {
        private double elapsedTime;
        private double nextTime;
        private Queue<Action> pattern;
        private EnemyType type;
        private int xpAward;

        public Enemy(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            PreparePattern();
        }


        #region Behaviour

        private void WaitFor()
        {

        }

        private void TurnRight()
        {
            Direction += Math.PI / 2;
            PrepareNext(0, -0.5);
        }

        private void PrepareNext(double delay = 0, double extend = 0)
        {
            elapsedTime = 0;
            nextTime = World.LevelMaker.RandDouble(0 + delay, 1 + delay + extend);
            pattern.Enqueue(pattern.Dequeue());
        }

        private void ProcessPattern(double time)
        {
            elapsedTime += time;
            if (elapsedTime >= nextTime)
            {
                PrepareNext(0.5);
            }
            pattern.Peek()();
        }

        private void PreparePattern()
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(1, 4);

            pattern = new Queue<Action>();
            pattern.Enqueue(MoveForward);
            pattern.Enqueue(WaitFor);
            pattern.Enqueue(TurnRight);
            pattern.Enqueue(WaitFor);
            pattern.Enqueue(MoveForward);
            pattern.Enqueue(WaitFor);
            pattern.Enqueue(TurnRight);
            pattern.Enqueue(TurnRight);
            pattern.Enqueue(WaitFor);
        }

        #endregion

        public override void Update(double time)
        {
            // Behaviour
            ProcessPattern(time);
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