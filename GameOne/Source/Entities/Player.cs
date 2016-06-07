﻿namespace GameOne.Source.Entities
{
	using GameOne.Source.Enumerations;
	using GameOne.Source.Renderer;

	public class Player : Character
    {
        private const int InitialHealthPotions = 0;
        private const int InitialAmmo = 200;

        private int xpLevel;

        public Player(double x, double y, double direction, int xpLevel = 1)
            : base(x, y, direction, 0.30, new Spritesheet(), 100, 10, AttackType.Melee)
        {
            this.XpLevel = xpLevel;
            this.HealthPotions = InitialHealthPotions;
            this.Ammo = InitialAmmo;
        }

        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int XpLevel
        {
            get
            {
                return this.xpLevel;
            }
            set
            {
                this.xpLevel = value;
            }
        }

        public void ChangeXPLevel(int level)
        {
            this.XpLevel += level;
        }

        public void AddHealthPotion(int health)
        {
            this.HealthPotions++;
        }

        public void DrinkPotion(int health)
        {
            if (this.HealthPotions > 0)
            {
                this.HealthPotions--;
                this.Health += 20;
            }
        }

		internal void Input(UserInput input)
		{
			switch (input)
			{
				case UserInput.MoveUp:
					MoveUp();
					break;
				case UserInput.MoveDown:
					MoveDown();
					break;
				case UserInput.MoveLeft:
					MoveLeft();
					break;
				case UserInput.MoveRight:
					MoveRight();
					break;
			}
		}

		public void ApplyItemEffectsToHealth(int health)
        {
            this.Health += health;
        }

        public void ApplyItemEffectsToAmmo(int ammo)
        {
            this.Ammo += ammo;
        }

        public override void Render()
        {
            // TODO
        }
    }
}