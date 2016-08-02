namespace GameOne.Source.World.Physics
{
    using System.Windows;
    using Interfaces;

    public class Body : IPhysicsBody
    {
        public double Direction { get; protected set; }

        public Vector Position { get; set; }

        public double Radius { get; private set; }
    }
}
