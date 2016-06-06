namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Player : Character
    {
        private int xpLevel = 1;
        private const int InitialHealthPotions = 0;
        private const int InitialAmmo = 200;

        public Player(double x, double y, double direction)
            : base(x, y, direction, 0.25, new Spritesheet(), 100, 10, AttackType.Melee)
        {
            this.XpLevel = xpLevel;
            this.HealthPotions = InitialHealthPotions;
            this.Ammo = InitialAmmo;
        }

        public int Ammo { get; set; }

        public int HealthPotions { get; set; }

        public int XpLevel { get; set; }

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

        public void ApplyItemEffectsToHealth(int health)
        {
            this.Health += health;
        }

        public void ApplyItemEffectsToAmmo(int ammo)
        {
            this.Ammo += ammo;
        }

        public override void Update()
        {
            // TODO
        }

        public override void Render()
        {
            // TODO
        }
    }
}