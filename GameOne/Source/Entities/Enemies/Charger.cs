namespace GameOne.Source.Entities.Enemies
{
    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public class Charger : Enemy
    {
        private const double ChargerRadius = 0.25;
        private const int ChargerHealth = 30;
        private const int ChargerDamage = 10;
        private const AttackType ChargerAttackType = AttackType.Melee;
        private const EnemyType ChargerType = EnemyType.Charger;
        private const int ChargerXpAward = 60;
        private const double nominalVelocityModifier = 0.3;
        private const double chargeVelocityModifier = 3;

        private double velocityModifier;

        public Charger(double x,
            double y,
            double direction,
            IRenderingStrategy sprite,
            double healthModifier,
            double damageModifier,
            double xpAwardModifier)
            : base(x,
                  y,
                  direction,
                  ChargerRadius,
                  sprite,
                  ChargerHealth + (int)(ChargerHealth * healthModifier),
                  ChargerDamage + (int)(ChargerDamage * damageModifier),
                  ChargerAttackType,
                  ChargerType,
                  ChargerXpAward + (int)(ChargerXpAward * xpAwardModifier))
        {
            this.velocityModifier = nominalVelocityModifier;
        }

        protected override double FireRate => 2; // does not have a ranged attack
        protected override double VelocityModifier => this.velocityModifier;

        private void BeginCharge()
        {
            double charge = World.LevelMaker.RandDouble();
            if (charge >= 0.5)
            {
                double havoc = World.LevelMaker.RandDouble();
                if (havoc >= 0.9)
                {
                    this.TurnAround();
                }
                else if (havoc >= 0.7)
                {
                    this.TurnRight();
                }
                else if (havoc >= 0.5)
                {
                    this.TurnRight();
                }
                this.velocityModifier = chargeVelocityModifier;
                base.MoveForward();
            }
        }

        private void EndCharge()
        {
            this.velocityModifier = nominalVelocityModifier;
        }

        protected override void PreparePattern()
        {
            base.PreparePattern();
            this.pattern.Enqueue(this.MoveForward);
            this.pattern.Enqueue(this.BeginCharge);
            this.pattern.Enqueue(this.EndCharge);
            this.pattern.Enqueue(this.WaitFor);

            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.WaitFor);

            this.pattern.Enqueue(this.MoveForward);
            this.pattern.Enqueue(this.BeginCharge);
            this.pattern.Enqueue(this.EndCharge);
            this.pattern.Enqueue(this.WaitFor);

            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.WaitFor);
        }
    }
}
