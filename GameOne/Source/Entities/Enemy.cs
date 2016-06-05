namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Enemy : Character
    {
        private EnemyType type;
        private int xpAward;

        public Enemy(EnemyType type, double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, int xpAward, AttackType attackType)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;
            this.AttackType = attackType;
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}