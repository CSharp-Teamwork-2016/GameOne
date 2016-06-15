namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Player : Character
    {
        private const int InitialHealthPotions = 0;
        private const int InitialAmmo = 200;

        private int xpLevel;
        private int experience;

        public Player(double x, double y, double direction, int xpLevel = 1)
            : base(x, y, direction, 0.30, new Spritesheet(), 100, 10)
        {
            this.xpLevel = xpLevel;
            experience = 0;
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
        }

        public void GainXP(int level)
        {
            this.experience += level;
        }

        public void DrinkPotion()
        {
            if (this.HealthPotions > 0 && health < maxHealth)
            {
                this.HealthPotions--;
                Heal(30);
            }
        }

		internal void Input(UserInput input)
		{
            if ((state & State.HURT) == State.HURT) return; // don't let the player move if he's hit
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
                case UserInput.Attack:
                    Attack();
                    break;
                case UserInput.DrinkPotion:
                    DrinkPotion();
                    break;
			}
		}

        public void PickUpItem(ItemType type)
        {
            switch (type)
            {
                case ItemType.PotionHealth:
                    Heal(15);
                    break;
                case ItemType.QuartzFlask:
                    HealthPotions++;
                    break;
                case ItemType.EndKey:
                    break;
            }
        }

        public override void Render()
        {
            // TODO
        }

        public override void Update(double time)
        {
            base.Update(time);

            Primitive.CameraX = Position.X;
            Primitive.CameraY = Position.Y;

            Loop.debugInfo = string.Format($"Player stats:\nState: {state}\nHealth: {health}\nPotions: {HealthPotions}\n\nLevel {xpLevel}\nXP: {experience}");
            //Loop.debugInfo += string.Format($"State: {state}\n");
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            Knockback();
        }
    }
}