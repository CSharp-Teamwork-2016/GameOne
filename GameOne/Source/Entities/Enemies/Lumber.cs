namespace GameOne.Source.Entities.Enemies
{
    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public class Lumber : Enemy
    {
        private const double LumberRadius = 0.5;
        private const int LumberHealth = 150;
        private const int LumberDamage = 10;
        private const AttackType LumberAttackType = AttackType.Melee;
        private const EnemyType LumberType = EnemyType.Lumber;
        private const int LumberXpAward = 150;

        public Lumber(double x,
            double y,
            double direction,
            IRenderingStrategy sprite,
            double healthModifier,
            double damageModifier,
            double xpAwardModifier)
            : base(x,
                  y,
                  direction,
                  LumberRadius,
                  sprite,
                  LumberHealth + (int)(LumberHealth * healthModifier),
                  LumberDamage + (int)(LumberDamage * damageModifier),
                  LumberAttackType,
                  LumberType,
                  LumberXpAward + (int)(LumberXpAward * xpAwardModifier))
        {
        }

        protected override double FireRate => 2; // does not have a ranged attack
        protected override double VelocityModifier => 0.5;

        protected override void PreparePattern()
        {
            base.PreparePattern();
            this.pattern.Enqueue(this.MoveForward);
            this.pattern.Enqueue(this.MoveForward);
            this.pattern.Enqueue(this.MoveForward);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.WaitFor);
        }
    }
}
