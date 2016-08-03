namespace GameOne.Source.Entities.Enemies
{
    using System;
    using Enumerations;
    using Interfaces;
    using EventArgs;
    using World.Physics;

    [Serializable]
    public class Harvester : Enemy
    {
        private const double HarvesterRadius = 0.50;
        private const int HarvesterHealth = 5000;
        private const int HarvesterDamage = 35;
        private const AttackType HarvesterAttackType = AttackType.Melee;
        private const EnemyType HarvesterType = EnemyType.Harvester;
        private const int HarvesterXpAward = 3000;

        private readonly double startingX;
        private readonly double startingY;

        private int wave;

        public Harvester(double x,
            double y,
            double direction,
            IRenderingStrategy sprite,
            double healthModifier,
            double damageModifier,
            double xpAwardModifier)
            : base(x,
                  y,
                  direction,
                  HarvesterRadius,
                  sprite,
                  HarvesterHealth + (int)(HarvesterHealth * healthModifier),
                  HarvesterDamage + (int)(HarvesterDamage * damageModifier),
                  HarvesterAttackType,
                  HarvesterType,
                  HarvesterXpAward + (int)(HarvesterXpAward * xpAwardModifier))
        {
            this.startingX = x;
            this.startingY = y;

            this.wave = 4;
        }

        [field: NonSerialized]
        public event EventHandler<BossActionArgs> BossActionHandler;

        protected override double FireRate => 0.99;

        protected override void PreparePattern()
        {
            base.PreparePattern();
            this.pattern.Enqueue(this.GoToLeft);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.GoToUp);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.GoToRight);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.GoToDown);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.SpawnChargers);
        }

        private void GoToLeft()
        {
            if (Math.Round(this.Position.Y) > this.startingY)
            {
                MoveUp();
            }
            else if (Math.Round(this.Position.Y) < this.startingY)
            {
                MoveDown();
            }
            else if (Math.Round(this.Position.X) > this.startingX - 4)
            {
                MoveLeft();
            }
            else
            {
                this.Direction = PhysicsEngine.RightDirection;
            }
        }

        private void GoToRight()
        {
            if (Math.Round(this.Position.Y) > this.startingY)
            {
                MoveUp();
            }
            else if (Math.Round(this.Position.Y) < this.startingY)
            {
                MoveDown();
            }
            else if (Math.Round(this.Position.X) < this.startingX + 4)
            {
                MoveRight();
            }
            else
            {
                this.Direction = PhysicsEngine.LeftDirection;
            }
        }

        private void GoToUp()
        {
            if (Math.Round(this.Position.X) > this.startingX)
            {
                MoveLeft();
            }
            else if (Math.Round(this.Position.X) < this.startingX)
            {
                MoveRight();
            }
            else if (Math.Round(this.Position.Y) > this.startingY - 4)
            {
                MoveUp();
            }
            else
            {
                this.Direction = PhysicsEngine.DownDirection;
            }
        }

        private void GoToDown()
        {
            if (Math.Round(this.Position.X) > this.startingX)
            {
                MoveLeft();
            }
            else if (Math.Round(this.Position.X) < this.startingX)
            {
                MoveRight();
            }
            else if (Math.Round(this.Position.Y) < this.startingY + 4)
            {
                MoveDown();
            }
            else
            {
                this.Direction = PhysicsEngine.UpDirection;
            }
        }

        private void SpawnChargers()
        {
            BossActionArgs args = new BossActionArgs(BossAction.RaiseChargers)
            {
                X = this.startingX,
                Y = this.startingY
            };

            this.BossActionHandler(this, args);
            this.PrepareNext(0, -0.5);

            this.wave += 2;
        }

        protected override void PrepareNext(double delay = 0, double extend = 0)
        {
            this.elapsedTime = 0;
            this.nextTime = 5;
            this.pattern.Enqueue(this.pattern.Dequeue());
        }
    }
}
