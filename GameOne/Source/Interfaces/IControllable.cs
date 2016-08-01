namespace GameOne.Source.Interfaces
{
    using System.Windows;

    public interface IControllable
    {
        void MoveUp();
        void MoveDown();
        void MoveLeft();
        void MoveRight();
        void MoveForward();
        void TurnTo(double direction);
        void Accelerate(Vector acceleration);
        void Stop();
    }
}
