namespace GameOne.Source.Interfaces
{
    using System.Windows;
    using Enumerations;

    public interface IRenderable
    {
        bool Alive { get; }

        Vector Position { get; }

        double Direction { get; }

        double Radius { get; }

        State State { get; }
    }
}
