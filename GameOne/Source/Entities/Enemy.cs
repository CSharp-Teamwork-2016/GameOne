namespace GameOne.Source.Entities
{
    using System;
    using System.Collections.Generic;

    using Enumerations;
    using Interfaces;
    using Events;

    [Serializable]
    public abstract class Enemy : Character
    {
        #region Fields

        private double elapsedTime;
        private double nextTime;
        protected Queue<Action> pattern;
        private readonly EnemyType type;
        private readonly int xpAward;
        private double fireRate;

        #endregion Fields

        #region Constructors

        public Enemy(double x, double y, double direction, double radius, IRenderingStrategy sprite, int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.fireRate = 0;
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            this.PreparePattern();
        }

        #endregion Constructors

        [field: NonSerialized]
        public event EventHandler<KilledEventArgs> KilledEvent;

        public EnemyType Type => this.type;

        protected abstract double FireRate { get; }

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
        }

        #region Methods/Behaviour

        protected void WaitFor()
        {
        }

        protected void TurnRight()
        {
            this.Direction += Math.Round(Math.PI / 2, 2);
            this.Direction %= Math.Round(2 * Math.PI, 2);
            this.PrepareNext(0, -0.5);
        }

        protected void TurnLeft()
        {
            this.Direction -= Math.Round(Math.PI / 2, 2);
            this.Direction %= Math.Round(2 * Math.PI, 2);
            this.PrepareNext(0, -0.5);
        }

        protected void TurnAround()
        {
            this.Direction += Math.Round(Math.PI, 2);
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

            if (World.LevelMaker.RandDouble(0, 1) > this.FireRate)
            {
                this.FireProjectile();
            }
        }

        protected virtual void PreparePattern()
        {
            this.elapsedTime = 0;
            this.nextTime = World.LevelMaker.RandDouble(1, 4);

            this.pattern = new Queue<Action>();
        }

        #endregion Methods/Behaviour

        #endregion Methods
    }
}