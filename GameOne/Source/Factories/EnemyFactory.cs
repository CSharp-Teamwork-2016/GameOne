namespace GameOne.Source.Factories
{
    using System;
    using Entities;
    using Enumerations;
    using World;
    using Entities.Enemies;

    public class EnemyFactory
    {
        public static Enemy MakeEnemy(double x, double y, EnemyType type, int difficulty)
        {
            Enemy enemy;

            double direction = Math.Round(Math.PI / 2 * LevelMaker.Rand(4), 2);
            double damageModifier = difficulty / 3.0;
            double hpModifier = difficulty / 12.0;
            switch (type)
            {
                case EnemyType.Zombie:
                    enemy = new Zombie(x, y, direction, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hpModifier, damageModifier, hpModifier);
                    break;
                case EnemyType.Sentry:
                    enemy = new Sentry(x, y, direction, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hpModifier, damageModifier, hpModifier);
                    break;
                case EnemyType.Lumber:
                    enemy = new Lumber(x, y, direction, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hpModifier, damageModifier, hpModifier);
                    break;
                case EnemyType.Charger:
                    enemy = new Charger(x, y, direction, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hpModifier, damageModifier, hpModifier);
                    break;
                case EnemyType.Harvester:
                    enemy = new Harvester(x, y, direction, RenderingStrategyFactory.MakeStrategy(RenderingMethod.Character), hpModifier, damageModifier, hpModifier);
                    break;
                default:
                    throw new ArgumentException("Unrecognized enemy type.");
            }

            return enemy;
        }
    }
}
