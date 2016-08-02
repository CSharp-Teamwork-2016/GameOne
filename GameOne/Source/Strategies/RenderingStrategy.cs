namespace GameOne.Source.Strategies
{
    using Interfaces;
    using System;
    [Serializable]
    public abstract class RenderingStrategy : IRenderingStrategy
    {
        public abstract void Render(IRenderable model);
    }
}
