namespace GameOne.Source.Factories
{
    using Enumerations;
    using Interfaces;
    using Strategies;

    public abstract class RenderingStrategyFactory
    {
        public static IRenderingStrategy MakeStrategy(RenderingMethod method)
        {
            switch (method)
            {
                case RenderingMethod.Character:
                    return new CharacterRenderer();
                case RenderingMethod.Item:
                    return new ItemRenderer();
                case RenderingMethod.Projectile:
                    return new ProjectileRenderer();
                case RenderingMethod.Tile:
                    return new TileRenderer();
                default:
                    return null;
            }
        }
    }
}
