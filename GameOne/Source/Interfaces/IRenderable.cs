namespace GameOne.Source.Interfaces
{
    using System.Windows;
    using Enumerations;

    public interface IRenderable
    {
        Vector Position { get; }
        IRenderingStrategy RenderingStrategy();
    }
}
