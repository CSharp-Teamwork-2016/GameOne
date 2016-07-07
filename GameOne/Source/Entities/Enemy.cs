namespace GameOne.Source.Entities
{
    using System;
    using System.Collections.Generic;

    using Enumerations;
    using Renderer;

    public class Enemy : Character
    {
        #region Fields

        private double elapsedTime;
        private double nextTime;
        private Queue<Action> pattern;
        private EnemyType type;
        private int xpAward;

        #endregion Fields

        //===================================================================

        #region Constructors

        public Enemy(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            PreparePattern();
        }

        #endregion Constructors

        //===================================================================

        public EnemyType Type => this.type;

        #region Methods

        #region Methods/Behaviour

        private void WaitFor()
        {

        }

        private void TurnRight()
        {
            Direction += Math.Round(Math.PI / 2, 2);
            Direction %= Math.Round(2 * Math.PI, 2);
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

            // Random firing pattern hack
            double probability = 0.99;
            if (type == EnemyType.Sentry) probability = 0.95;
            if (World.LevelMaker.RandDouble(0, 1) > probability)
                FireProjectile();
        }

        private void PreparePattern()
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(1, 4);

            pattern = new Queue<Action>();
            if (type == EnemyType.Zombie)
            {
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
            else if (type == EnemyType.Sentry)
            {
                pattern.Enqueue(WaitFor);
                pattern.Enqueue(TurnRight);
                pattern.Enqueue(WaitFor);
                pattern.Enqueue(WaitFor);
                pattern.Enqueue(TurnRight);
                pattern.Enqueue(TurnRight);
                pattern.Enqueue(WaitFor);
            }
        }

        #endregion Methods/Behaviour

        public override void Update(double time)
        {
            if (state == State.DEAD)
            {
                return;
            }
            // Behaviour
            ProcessPattern(time);
            base.Update(time);
        }

        public override void Die()
        {
            base.Die();
            Loop.level.Player.GainXP(xpAward);
            Loop.level.EnemySlain();
        }

        #endregion Methods
    }
}