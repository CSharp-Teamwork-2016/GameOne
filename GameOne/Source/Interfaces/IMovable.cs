namespace GameOne.Source.Interfaces
{
    using Enumerations;
    using System.Windows;

    public interface IMovable : ICollidable
    {
        Vector Velocity { get; set; }
        MovementType MovementType { get; }
    }
}
