namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    using Microsoft.Xna.Framework;

    public abstract class Character : Model
    {
        protected Character(string id, Vector2 position, string direction, double radius, Spritesheet sprite, State state, int health, int damage, AttackType attackType)
            : base(id, position, direction, radius, sprite, state)
        {
            this.Health = health;
            this.Damage = damage;
            this.AttackType = attackType;
        }


        public int Health { get; set; }

        public int Damage { get; set; }

        public AttackType AttackType { get; set; }

    }
}
