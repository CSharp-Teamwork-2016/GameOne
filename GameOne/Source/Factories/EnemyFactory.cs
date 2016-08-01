namespace GameOne.Source.Factories
{
    using System;
    using Entities;
    using Enumerations;
    using World;

    public class EnemyFactory
    {
        public static Enemy MakeEnemy(double x, double y, EnemyType type, int difficulty)
        {
            double direction = Math.Round(Math.PI / 2 * LevelMaker.Rand(4), 2);
            int hp = 0;
            int damage = 0;
            switch (type)
            {
                case EnemyType.Zombie:
                    hp = 50;
                    damage = 3;
                    break;
                case EnemyType.Sentry:
                    hp = 30;
                    damage = 6;
                    break;
            }

            damage += (int)(damage * difficulty / 3.0);
            hp += (int)(hp * difficulty / 12.0);
            Enemy enemy = new Enemy(x, y, direction, 0.3, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hp, damage, AttackType.Melee, type, hp);
            return enemy;
        }
    }
}
