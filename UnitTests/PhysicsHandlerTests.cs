namespace UnitTests
{
    using GameOne.Source.Interfaces;
    using GameOne.Source.World.Physics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Windows;
    using GameOne.Source.Enumerations;

    [TestClass]
    public class PhysicsHandlerTests
    {
        [TestMethod]
        public void VerifyPenetrationValueForCircleCircle()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                Radius = 1,
                CollisionShape = Shape.Circle,
                IsSolid = true
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(0, 0.5),
                Radius = 1,
                CollisionShape = Shape.Circle,
                IsSolid = true
            };

            Assert.AreEqual(new Vector(0, 1.5), PhysicsEngine.Intersect(circle1, circle2), "Positive value expected");
            Assert.AreEqual(new Vector(0, -1.5), PhysicsEngine.Intersect(circle2, circle1), "Negative value expected");
        }
    }
}
