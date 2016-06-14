namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Enemy : Character
    {
        private double elapsedTime;
        private double nextTime;
        private EnemyType type;
        private int xpAward;

        public Enemy(double x, double y, double direction, double radius, Spritesheet sprite, 
                int health, int damage, AttackType attackType, EnemyType type, int xpAward)
            : base(x, y, direction, radius, sprite, health, damage, attackType)
        {
            this.type = type;
            this.xpAward = xpAward;

            // Behaviour
            PrepareNext();
        }

        private void PrepareNext()
        {
            elapsedTime = 0;
            nextTime = (new System.Random((int)Id)).NextDouble() * 3;
        }

        public override void Update(double time)
        {
            elapsedTime += time;
            if (elapsedTime >= nextTime)
            {
                System.Random rnd = new System.Random();
                PrepareNext();
                Direction = System.Math.PI / 2 * rnd.Next(4);
                MoveForward();
            }
            base.Update(time);
        }

        public override void Render()
        {
            // TODO
        }
    }
}