namespace GameOne.Source.Interfaces
{
    using System.Windows;

    public interface IMovable
    {
        Vector Velocity { get; }
        void TurnTo(double direction);
        void Accelerate(Vector acceleration);
        void Stop();
    }
}
