namespace GameOne.Source.Entities
{
	using GameOne.Source.Enumerations;
	using GameOne.Source.Renderer;

	public abstract class Character : Model
	{
		protected int health;
		protected int damage;
		protected AttackType attackType;

		protected Character(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
			: base(x, y, direction, radius, sprite)
		{
			this.health = health;
			this.damage = damage;
			this.attackType = attackType;
		}


		public int Health { get { return health; } }

		public int Damage { get { return damage; } }

		public AttackType AttackType { get { return attackType; } }

	    public void TakeDamage(int damage)
	    {
	        this.health -= damage;
	    }
        
        public void ProduceAttack(Enemy enemy)
        {
            enemy.health -= this.damage;
        }
    }
}
