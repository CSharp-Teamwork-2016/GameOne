namespace GameOne.Source.Interfaces
{
    using Enumerations;

    public interface IRenderable
    {
        bool Alive { get; }
        System.Windows.Vector Position { get; }
        double Direction { get; }
        double Radius { get; }
        State State { get; }
    }
}
