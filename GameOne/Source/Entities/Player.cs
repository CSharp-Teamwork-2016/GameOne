namespace GameOne.Source.Entities
{
    using Enumerations;
    using Renderer;

    public class Player : Character
    {
        #region Fields

        private const int InitialHealthPotions = 0;
        private const int InitialAmmo = 200;

        private int xpLevel;
        private int experience;
        private int xpToNext;

        #endregion Fields

        //===================================================================

        #region Constructors

        public Player(double x, double y, double direction, int xpLevel = 1)
            : base(x, y, direction, 0.30, new Spritesheet(), 100, 30)
        {
            this.xpLevel = xpLevel;
            experience = 0;
            xpToNext = 320;
            this.HealthPotions = InitialHealthPotions;
            this.Ammo = InitialAmmo;
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int MaxHealth => base.MaxHealth;

        public int XpLevel => this.xpLevel;

        public int XpToNext => xpToNext;

        #endregion Properties

        //===================================================================

        #region Methods

        public void GainXP(int level)
        {
            this.experience += level;
            if (experience >= xpToNext)
            {
                xpLevel++;
                experience -= xpToNext;
                xpToNext = (int)((xpToNext * 1.4) / 10) * 10;
                Damage = (int)(Damage * 1.25);
                base.MaxHealth = (int)(base.MaxHealth * 1.1);
                Health = base.MaxHealth;
            }
        }

        public void DrinkPotion()
        {
            if (timeToNextAction > 0)
            {
                return;
            }

            if (this.HealthPotions > 0 && Health < base.MaxHealth)
            {
                this.HealthPotions--;
                Heal(30);
            }
        }

        internal void Input(UserInput input)
        {
            if ((state & State.HURT) == State.HURT)
            {
                return; // don't let the player move if he's hit
            }

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
                case UserInput.Shoot:
                    FireProjectile();
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
                    Loop.level.ExitTriggered = true;
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

            Loop.DebugInfo = string.Format($"Player stats:\nState: {state}\nHealth: {Health} / {base.MaxHealth}\nDamage: {Damage}\n\nLevel {xpLevel}\nXP: {experience} / {xpToNext}\n\nEnemies remaining: {Loop.level.EnemyCount}\n");
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
            Health = 100;
        }

        #endregion Methods
    }
}