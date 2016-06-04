namespace GameOne.Source.Entities
{
	using System;
	using GameOne.Source.Enumerations;
	using GameOne.Source.Renderer;

	public class Player : Character
    {
        private int xpLevel = 1;
        private const int healthPotions = 0;
        private const int ammo = 200;

        public Player(double x, double y, double direction)
            : base(x, y, direction, 0.25, new Spritesheet(), 100, 10, AttackType.Melee)
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

		public override void Render()
		{

		}
	}
}