namespace GameOne.Source.Interfaces
{
    public interface IPhysicsBody
    {
        double Direction { get; }
        System.Windows.Vector Position { get; set; }
        double Radius { get; }
    }
}
