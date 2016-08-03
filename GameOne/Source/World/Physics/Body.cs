namespace GameOne.Source.World.Physics
{
    using System.Windows;
    using Interfaces;

    public class Body : IPhysicsBody
    {
        public double Direction { get; set; }

        public Vector Position { get; set; }

        public double Radius { get; set; }

        public Rect BoundingBox { get; set; }
    }
}
