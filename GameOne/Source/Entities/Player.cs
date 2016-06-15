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
        private int xpToNext;

        public Player(double x, double y, double direction, int xpLevel = 1)
            : base(x, y, direction, 0.30, new Spritesheet(), 100, 10)
        {
            this.xpLevel = xpLevel;
            experience = 0;
            xpToNext = 320;
            this.HealthPotions = InitialHealthPotions;
            this.Ammo = InitialAmmo;
        }

        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }

        public int XpLevel
        {
            get
            {
                return this.xpLevel;
            }
        }

        public int XpToNext
        {
            get
            {
                return xpToNext;
            }
        }

        public void GainXP(int level)
        {
            this.experience += level;
            if (experience >= xpToNext)
            {
                xpLevel++;
                experience -= xpToNext;
                xpToNext = (int)((xpToNext * 1.4) / 10) * 10;
                damage = (int)(damage * 1.25);
                maxHealth = (int)(maxHealth * 1.1);
                health = maxHealth;
            }
        }

        public void DrinkPotion()
        {
            if (timeToNextAction > 0) return;
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
                    Loop.level.exitTriggered = true;
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

            Loop.debugInfo = string.Format($"Player stats:\nState: {state}\nHealth: {health} / {maxHealth}\nDamage: {damage}\n\nLevel {xpLevel}\nXP: {experience} / {xpToNext}\n\nEnemies remaining: {Loop.level.enemyCount}\n");
            //Loop.debugInfo += string.Format($"State: {state}\n");
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            Knockback();
        }

        public void Respawn()
        {
            state = State.IDLE;
            health = 100;
        }
    }
}