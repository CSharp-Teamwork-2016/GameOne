namespace GameOne.Source.Entities
{
    using System;
    using Enumerations;
    using Renderer;
    using Containers;

    public class Player : Character
    {
        #region Fields

        private const int InitialHealthPotions = 0;
        private const int InitialAmmo = 200;

        private int experience;

        #endregion Fields

        #region Constructors

        public Player(double x, double y, double direction, int xpLevel = 1)
            : base(x, y, direction, 0.30, new Spritesheet(), 100, 30)
        {
            this.XpLevel = xpLevel;
            this.experience = 0;
            this.XpToNext = 320;
            this.HealthPotions = InitialHealthPotions;
            this.Ammo = InitialAmmo;
        }

        #endregion Constructors

        #region Properties

        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int XpLevel { get; private set; }

        public int XpToNext { get; private set; }

        #endregion Properties

        #region Methods

        public void GainXP(int level)
        {
            this.experience += level;
            if (this.experience >= this.XpToNext)
            {
                this.XpLevel++;
                this.experience -= this.XpToNext;
                this.XpToNext = (int)((this.XpToNext * 1.4) / 10) * 10;
                this.Damage = (int)(this.Damage * 1.25);
                base.MaxHealth = (int)(base.MaxHealth * 1.1);
                this.Health = base.MaxHealth;
            }
        }

        public void DrinkPotion()
        {
            if (this.timeToNextAction > 0)
            {
                return;
            }

            if (this.HealthPotions > 0 && this.Health < base.MaxHealth)
            {
                this.HealthPotions--;
                this.Heal(30);
            }
        }

        public void PickUpItem(ItemType type)
        {
            switch (type)
            {
                case ItemType.PotionHealth:
                    this.Heal(15);
                    break;
                case ItemType.QuartzFlask:
                    this.HealthPotions++;
                    break;
                case ItemType.EndKey:
                    GameContainer.level.ExitTriggered = true;
                    break;
            }
        }

        public override void Update(double time)
        {
            base.Update(time);

            Primitive.CameraX = this.Position.X;
            Primitive.CameraY = this.Position.Y;

            GameContainer.DebugInfo = $"Player stats:{Environment.NewLine}State: {this.state}{Environment.NewLine}Health: {this.Health} / {base.MaxHealth}{Environment.NewLine}Damage: {this.Damage}{Environment.NewLine}{Environment.NewLine}Level {this.XpLevel}{Environment.NewLine}XP: {this.experience} / {this.XpToNext}{Environment.NewLine}{Environment.NewLine}Enemies remaining: {GameContainer.level.EnemyCount}{Environment.NewLine}";
            // Loop.debugInfo += string.Format($"State: {state}\n");
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            this.Knockback();
        }

        public void Respawn()
        {
            this.state = State.IDLE;
            this.Health = 100;
        }

        internal void Input(UserInput input)
        {
            if ((this.state & State.HURT) == State.HURT)
            {
                return; // don't let the player move if he's hit
            }

            switch (input)
            {
                case UserInput.MoveUp:
                    this.MoveUp();
                    break;
                case UserInput.MoveDown:
                    this.MoveDown();
                    break;
                case UserInput.MoveLeft:
                    this.MoveLeft();
                    break;
                case UserInput.MoveRight:
                    this.MoveRight();
                    break;
                case UserInput.Attack:
                    this.Attack();
                    break;
                case UserInput.DrinkPotion:
                    this.DrinkPotion();
                    break;
                case UserInput.Shoot:
                    this.FireProjectile();
                    break;
            }
        }

        #endregion Methods
    }
}