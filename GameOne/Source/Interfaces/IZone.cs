namespace GameOne.Source.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IZone : IRemovable
    {
        double X { get; }
        double Y { get; }
        double Width { get; }
        double Height { get; }
    }
}
