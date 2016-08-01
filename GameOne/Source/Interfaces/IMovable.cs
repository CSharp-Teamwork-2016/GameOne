namespace GameOne.Source.Interfaces
{
    using System.Windows;

    public interface IMovable : ICollidable
    {
        Vector Velocity { get; }
    }
}
