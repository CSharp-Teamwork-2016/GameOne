namespace UnitTests
{
    using System.Windows;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using GameOne.Source.World;
    using GameOne.Source.World.Physics;
    using GameOne.Source.Interfaces;
    using GameOne.Source.Enumerations;
    using System;
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
                Position = new Vector(-0.5, 0),
                Radius = 1
            };

            Assert.IsNotNull(PhysicsEngine.IntersectCircleCircle(circle1, circle2), "Vertical check failed");
            Assert.IsNotNull(PhysicsEngine.IntersectCircleCircle(circle1, circle3), "Horizontal check failed");
            Assert.IsNotNull(PhysicsEngine.IntersectCircleCircle(circle2, circle3), "Diagonal check failed");
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

            Assert.IsNull(PhysicsEngine.IntersectCircleCircle(circle1, circle2), "Vertical check failed");
            Assert.IsNull(PhysicsEngine.IntersectCircleCircle(circle1, circle3), "Horizontal check failed");
            Assert.IsNull(PhysicsEngine.IntersectCircleCircle(circle2, circle3), "Diagonal check failed");
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

            Assert.IsNotNull(PhysicsEngine.IntersectCircleSquare(circle1, square1), "Horizontal check failed");
            Assert.IsNotNull(PhysicsEngine.IntersectCircleSquare(circle1, square2), "Corner check failed");
            Assert.IsNotNull(PhysicsEngine.IntersectCircleSquare(circle2, square1), "Vertical check failed");
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

            Assert.IsNull(PhysicsEngine.IntersectCircleSquare(circle1, square1), "Horizontal check failed");
            Assert.IsNull(PhysicsEngine.IntersectCircleSquare(circle1, square2), "Corner check failed");
            Assert.IsNull(PhysicsEngine.IntersectCircleSquare(circle2, square1), "Vertical check failed");
            Assert.IsNull(PhysicsEngine.IntersectCircleSquare(circle2, square2), "Large distance check failed");
        }

        [TestMethod]
        public void VerifyPenetrationValueForCircleCircle()
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

            Assert.AreEqual(new Vector(0, 1.5), PhysicsEngine.IntersectCircleCircle(circle1, circle2), "Positive value expected");
            Assert.AreEqual(new Vector(0, -1.5), PhysicsEngine.IntersectCircleCircle(circle2, circle1), "Negative value expected");
        }

        [TestMethod]
        public void VerifyPenetrationValueForCircleSquare()
        {
            ICollidable circle1 = new RigidBody()
            {
                Position = new Vector(1.25, 0),
                Radius = 1
            };
            ICollidable circle2 = new RigidBody()
            {
                Position = new Vector(1, 1.15),
                Radius = 1
            };
            ICollidable square1 = new RigidBody()
            {
                Position = new Vector(0, 0),
                BoundingBox = new Rect(-0.5, -0.5, 1, 1)
            };

            Assert.AreEqual(new Vector(-0.25, 0), PhysicsEngine.IntersectCircleSquare(circle1, square1), "Negative value expected");
            Vector result2 = (Vector)PhysicsEngine.IntersectCircleSquare(circle2, square1);
            result2 = new Vector(Math.Round(result2.X, 1), Math.Round(result2.Y, 1));
            Assert.AreEqual(new Vector(-0.1, -0.1), result2, "Corner check failed");
        }
    }
}
