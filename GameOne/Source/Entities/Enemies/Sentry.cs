
namespace GameOne.Source.Entities.Enemies
{
    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public class Sentry : Enemy
    {
        private const double SentryRadius = 0.30;
        private const int SentryHealth = 30;
        private const int SentryDamage = 6;
        private const AttackType SentryAttackType = AttackType.Ranged;
        private const EnemyType SentryType = EnemyType.Sentry;
        private const int SentryXpAward = 30;

        public Sentry(double x,
            double y,
            double direction,
            IRenderingStrategy sprite,
            double healthModifier,
            double damageModifier,
            double xpAwardModifier)
            : base(x,
                  y,
                  direction,
                  SentryRadius,
                  sprite,
                  SentryHealth + (int)(SentryHealth * healthModifier),
                  SentryDamage + (int)(SentryDamage * damageModifier),
                  SentryAttackType,
                  SentryType,
                  SentryXpAward + (int)(SentryXpAward * xpAwardModifier))
        {
        }

        protected override double FireRate => 0.95;

        protected override void PreparePattern()
        {
            base.PreparePattern();
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.WaitFor);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.TurnRight);
            this.pattern.Enqueue(this.WaitFor);
        }
    }
}
