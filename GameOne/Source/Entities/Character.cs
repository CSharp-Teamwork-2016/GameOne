namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public abstract class Character : Model
    {
        private int health;
        private int damage;
        private AttackType attackType;

        protected Character(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
            : base(x, y, direction, radius, sprite)
        {
            this.health = health;
            this.damage = damage;
            this.attackType = attackType;
        }


        public int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                this.health = value;
            }
        }

        public int Damage
        {
            get
            {
                return this.damage;
            }

            set
            {
                this.damage = value;
            }
        }

        public AttackType AttackType
        {
            get
            {
                return this.attackType;
            }
        }

        public void TakeDamage(int damage)
        {
            this.health -= damage;
        }

        public void ProduceAttack(Enemy enemy)
        {
            enemy.health -= this.damage;
        }

        public bool IsDead()
        {
            if (this.Health <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
