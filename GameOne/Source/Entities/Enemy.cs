namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Enemy : Character
    {
        private EnemyType type;
        private int xpAward;

        public Enemy(double x, double y, double direction, double radius, Spritesheet sprite, 
                int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;
            // this.AttackType = attackType; // ??
        }

        public override void Update(double time)
        {
            // TODO
        }

        public override void Render()
        {
            // TODO
        }
    }
}