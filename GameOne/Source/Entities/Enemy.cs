namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    using Microsoft.Xna.Framework;

    public class Enemy : Character
    {

        private const int xpAward = 1;

        public Enemy(string id, Vector2 position, string direction, double radius, Spritesheet sprite, State state, int health, int damage, AttackType attackType)
            : base(id, position, direction, radius, sprite, state, health, damage, attackType)
        {
            this.XPAward = xpAward;
        }

        public int XPAward { get; set; }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}