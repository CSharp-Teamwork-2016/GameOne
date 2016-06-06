namespace GameOne.Source.Entities
{
	using System;
	using GameOne.Source.Enumerations;
	using GameOne.Source.Renderer;

	public abstract class Character : Model
	{
		private System.Windows.Vector velocity;
		private int health;
		private int damage;
		private AttackType attackType;

		protected Character(double x, double y, double direction, double radius, Spritesheet sprite, int health, int damage, AttackType attackType = AttackType.Melee)
			: base(x, y, direction, radius, sprite)
		{
			velocity = new System.Windows.Vector(0, 0);

			this.health = health;
			this.damage = damage;
			this.attackType = attackType;
		}

		#region Properties

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

			protected set
			{
				this.attackType = value;
			}
		}

		#endregion

		#region Movement

		public void MoveUp()
		{
			Direction = 1.5 * Math.PI;
			velocity.Y = -3;
		}
		public void MoveDown()
		{
			Direction = 0.5 * Math.PI;
			velocity.Y = 3;
		}
		public void MoveLeft()
		{
			Direction = Math.PI;
			velocity.X = -3;
		}
		public void MoveRight()
		{
			Direction = 0;
			velocity.X = 3;
		}

		#endregion

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
			return this.Health <= 0;

			//if (this.Health <= 0)
			//{
			//    return false;
			//}

			//return true;
		}

		public override void Update(double time)
		{
			// Motion
			if (velocity.Length > 0)
			{
				position.X += velocity.X * time;
				position.Y += velocity.Y * time;
				// Friction
				System.Windows.Vector friction = new System.Windows.Vector();
				friction.X = -velocity.X;
				friction.Y = -velocity.Y;
				friction.Normalize();
				System.Windows.Vector.Multiply(friction, time);
				velocity += friction;
				if (velocity.Length < 0.1)
				{
					velocity.X = 0;
					velocity.Y = 0;
				}
			}
		}
	}
}
