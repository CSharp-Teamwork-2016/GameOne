namespace UnitTests
{
    using System.Windows;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using GameOne.Source.World;
    using GameOne.Source.World.Physics;
    using GameOne.Source.Interfaces;
    using GameOne.Source.Enumerations;

    [TestClass]
    public class PhysicsTest
    {
        [TestMethod]
        public void IntersectingCircleCircle()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                Radius = 1
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(0, 0.5),
                Radius = 1
            };
            ICollidable circle3 = new RigidBody()
            {
                Position = new Vector(-0.5, 0.5),
                Radius = 1
            };

            Assert.IsTrue(PhysicsEngine.IntersectCircleCircle(circle1, circle2));
            Assert.IsTrue(PhysicsEngine.IntersectCircleCircle(circle1, circle3));
            Assert.IsTrue(PhysicsEngine.IntersectCircleCircle(circle2, circle3));
        }

        [TestMethod]
        public void NonIntersectingCircleCircle()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                Radius = 1
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(0, 2),
                Radius = 1
            };
            ICollidable circle3 = new RigidBody()
            {
                Position = new Vector(-2, 0),
                Radius = 1
            };

            Assert.IsFalse(PhysicsEngine.IntersectCircleCircle(circle1, circle2));
            Assert.IsFalse(PhysicsEngine.IntersectCircleCircle(circle1, circle3));
            Assert.IsFalse(PhysicsEngine.IntersectCircleCircle(circle2, circle3));
        }

        [TestMethod]
        public void IntersectingCircleSquare()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(1, 0.25),
                Radius = 1
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(0, 0.75),
                Radius = 0.5
            };
            ICollidable square1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                BoundingBox = new Rect(-1, -0.5, 2, 1)
            };
            ICollidable square2 = new RigidBody()
            {
                Position = new Vector(2.5, 1.5),
                BoundingBox = new Rect(1.5, 1, 3.5, 2)
            };

            Assert.IsTrue(PhysicsEngine.IntersectCircleSquare(circle1, square1));
            Assert.IsTrue(PhysicsEngine.IntersectCircleSquare(circle1, square2));
            Assert.IsTrue(PhysicsEngine.IntersectCircleSquare(circle2, square1));
        }

        [TestMethod]
        public void NonIntersectingCircleSquare()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(1.6, 0.25),
                Radius = 1
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(0, 1.25),
                Radius = 0.5
            };
            ICollidable square1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                BoundingBox = new Rect(-0.5, -0.5, 1, 1)
            };
            ICollidable square2 = new RigidBody()
            {
                Position = new Vector(3.35, 1.5),
                BoundingBox = new Rect(2.35, 1.1, 2, 1)
            };

            Assert.IsFalse(PhysicsEngine.IntersectCircleSquare(circle1, square1));
            Assert.IsFalse(PhysicsEngine.IntersectCircleSquare(circle1, square2));
            Assert.IsFalse(PhysicsEngine.IntersectCircleSquare(circle2, square1));
            Assert.IsFalse(PhysicsEngine.IntersectCircleSquare(circle2, square2));
        }
    }
}
