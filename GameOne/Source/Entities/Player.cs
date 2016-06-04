namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    using Microsoft.Xna.Framework;

    public class Player : Character
    {
        private const int health = 100;
        private const int damage = 10;
        private const int xpLevel = 1;
        private const int healthPotions = 0;
        private const int ammo = 200;

        public Player(string id, Vector2 position, string direction, double radius, Spritesheet sprite, State state, AttackType attackType)
            : base(id, position, direction, radius, sprite, state, health, damage, attackType)
        {
            this.XpLevel = xpLevel;
            this.HealthPotions = healthPotions;
            this.Ammo = ammo;
        }
            
        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int XpLevel { get; set; }

        public void changeXPLevel(int level)
        {
            // TODO
        }

        public void addHealthPotion(int health)
        {   
            // TODO
        }

        public void drinkPotion(int health)
        {
            // TODO
        }

        public void ApplyItemEffectsToHealth(int health)
        {
            // TODO
        }

        public void ApplyItemEffectsToAmmo(int ammo)
        {
            // TODO
        }

        public override void Update()
        {
            // TODO
        }
    }
}