namespace GameOne.Source.Strategies
{
    using Interfaces;

    public abstract class RenderingStrategy : IRenderingStrategy
    {
        public abstract void Render(IRenderable model);
    }
}
