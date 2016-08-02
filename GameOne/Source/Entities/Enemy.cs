namespace GameOne.Source.Entities
{
    using System;
    using System.Collections.Generic;

    using Enumerations;
    using Containers;
    using Interfaces;
    using Events;

    [Serializable]
    public class Enemy : Character
    {
        #region Fields

        private double elapsedTime;
        private double nextTime;
        private Queue<Action> pattern;
        private readonly EnemyType type;
        private readonly int xpAward;

        #endregion Fields

        #region Constructors

        public Enemy(double x, double y, double direction, double radius, IRenderingStrategy sprite, int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            this.PreparePattern();
        }

        #endregion Constructors

        [field: NonSerialized]
        public event EventHandler<KilledEventArgs> KilledEvent;

        public EnemyType Type => this.type;

        #region Methods

        public override void Update(double time)
        {
            if (this.state == State.DEAD)
            {
                return;
            }
            // Behaviour
            this.ProcessPattern(time);
            base.Update(time);
        }

        public override void Die()
        {
            base.Die();
            KilledEventArgs args = new KilledEventArgs(this.xpAward);
            KilledEvent(this, args);
            //GameContainer.level.Player.GainXP();
            //GameContainer.level.EnemySlain();
        }

        #region Methods/Behaviour

        private void WaitFor()
        {
        }

        private void TurnRight()
        {
            this.Direction += Math.Round(Math.PI / 2, 2);
            this.Direction %= Math.Round(2 * Math.PI, 2);
            this.PrepareNext(0, -0.5);
        }

        private void PrepareNext(double delay = 0, double extend = 0)
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(0 + delay, 1 + delay + extend);
            this.pattern.Enqueue(this.pattern.Dequeue());
        }

        private void ProcessPattern(double time)
        {
            this.elapsedTime += time;
            if (this.elapsedTime >= this.nextTime)
            {
                this.PrepareNext(0.5);
            }

            this.pattern.Peek()();

            // Random firing pattern hack
            double probability = 0.99;

            if (this.type == EnemyType.Sentry)
            {
                probability = 0.95;
            }

            if (World.LevelMaker.RandDouble(0, 1) > probability)
            {
                this.FireProjectile();
            }
        }

        private void PreparePattern()
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(1, 4);

            this.pattern = new Queue<Action>();
            if (this.type == EnemyType.Zombie)
            {
                this.pattern.Enqueue(this.MoveForward);
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.MoveForward);
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.WaitFor);
            }
            else if (this.type == EnemyType.Sentry)
            {
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.WaitFor);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.TurnRight);
                this.pattern.Enqueue(this.WaitFor);
            }
        }

        #endregion Methods/Behaviour

        #endregion Methods
    }
}