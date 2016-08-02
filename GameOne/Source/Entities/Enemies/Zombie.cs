namespace GameOne.Source.Entities.Enemies
{
    using Enumerations;
    using Interfaces;

    public class Zombie : Enemy
    {
        private const double ZombieRadius = 0.30;
        private const int ZombieHealth = 50;
        private const int ZombieDamage = 3;
        private const AttackType ZombieAttackType = AttackType.Melee;
        private const EnemyType ZombieType = EnemyType.Zombie;
        private const int ZombieXpAward = 50;

        public Zombie(double x,
            double y,
            double direction,
            IRenderingStrategy sprite,
            double healthModifier,
            double damageModifier,
            double xpAwardModifier)
            : base(x,
                  y,
                  direction,
                  ZombieRadius,
                  sprite,
                  ZombieHealth + (int)(ZombieHealth * healthModifier),
                  ZombieDamage + (int)(ZombieDamage * damageModifier),
                  ZombieAttackType,
                  ZombieType,
                  ZombieXpAward + (int)(ZombieXpAward * xpAwardModifier))
        {
        }

        protected override double FireRate => 0.99;

        protected override void PreparePattern()
        {
            base.PreparePattern();
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
    }
}
