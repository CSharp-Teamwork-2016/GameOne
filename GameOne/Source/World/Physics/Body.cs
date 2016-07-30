namespace GameOne.Source.World.Physics
{
    using System.Windows;
    using GameOne.Source.Interfaces;

    public class Body : IPhysicsBody
    {
        public double Direction { get; private set; }

        public Vector Position { get; set; }

        public double Radius { get; private set; }
    }
}
