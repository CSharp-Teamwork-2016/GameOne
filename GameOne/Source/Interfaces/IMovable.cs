namespace GameOne.Source.Interfaces
{
    using System.Windows;

    public interface IMovable
    {
        Vector Position { get; }

        double Direction { get; }
    }
}
