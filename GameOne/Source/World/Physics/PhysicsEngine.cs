namespace GameOne.Source.World.Physics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Entities;
    using Interfaces;
    using Enumerations;

    public static class PhysicsEngine
    {
        public static readonly double UpDirection = Math.Round(1.5 * Math.PI, 2);
        public static readonly double DownDirection = Math.Round(0.5 * Math.PI, 2);
        public static readonly double LeftDirection = Math.Round(Math.PI, 2);
        public static readonly double RightDirection = 0.0;
        public const double NominalVelocity = 3;
        public const double ProjectileSpeed = 8;
        public const double FrictionCoefficient = 15;

        #region Collision detection

        /// <summary>
        /// Returns a vector, representing the ammount of interpenetration between two collision shapes
        /// </summary>
        /// <param name="m1">First shape</param>
        /// <param name="m2">Second shape</param>
        /// <returns>Penetration as a Vector; null, if no penetration occurs</returns>
        public static Vector? Intersect(ICollidable m1, ICollidable m2)
        {
            // Bounding box check
            Vector? aabb = IntersectSquareSquare(m1, m2);
            Vector? result = null;
            if (aabb == null)
            {
                return null;
            }
            if (m1.CollisionShape == Shape.Circle)
            {
                if (m2.CollisionShape == Shape.Circle)
                {
                    result = IntersectCircleCircle(m1, m2);
                }
                else if (m2.CollisionShape == Shape.Rectangle)
                {
                    result = IntersectCircleSquare(m1, m2);
                }
            }
            else if (m1.CollisionShape == Shape.Rectangle)
            {
                if (m2.CollisionShape == Shape.Circle)
                {
                    // Since we swap the operands, we need to negate the result
                    result = IntersectCircleSquare(m2, m1);
                    result?.Negate();
                }
                else if (m2.CollisionShape == Shape.Rectangle)
                {
                    result = aabb; // Already confirmed in preliminary check
                }
            }
            if (result != null && (!m1.IsSolid || !m2.IsSolid))
            {
                // If one of the operands is not solid, only indicate intersection occured
                result = new Vector(0,0);
            }
            return result;
        }

        public static Vector? IntersectCircleCircle(ICollidable m1, ICollidable m2)
        {
            Vector separation = Vector.Subtract(m1.Position, m2.Position);
            double dist = separation.LengthSquared;
            double penetration = dist - (m1.Radius + m2.Radius) * (m1.Radius + m2.Radius);

            if (penetration < 0)
            {
                penetration = Math.Sqrt(dist) - (m1.Radius + m2.Radius);
                separation.Normalize();
                separation *= penetration;
                return separation;
            }
            return null;
        }

        public static Vector? IntersectCircleSquare(ICollidable circle, ICollidable rectangle)
        {
            Vector separation = Vector.Subtract(circle.Position, rectangle.Position);
            double horizontalHalf = rectangle.BoundingBox.Width / 2;
            double verticalHalf = rectangle.BoundingBox.Height / 2;

            if (Math.Abs(separation.X) <= horizontalHalf + circle.Radius && Math.Abs(separation.Y) <= verticalHalf)
            {
                double penetration = Math.Abs(separation.X) - (horizontalHalf + circle.Radius);
                if (separation.X < 0) penetration *= -1;
                separation = new Vector(penetration, 0);
                return separation;
            }
            if (Math.Abs(separation.X) <= horizontalHalf && Math.Abs(separation.Y) <= verticalHalf + circle.Radius)
            {
                double penetration = Math.Abs(separation.Y) - (verticalHalf + circle.Radius);
                if (separation.Y < 0) penetration *= -1;
                separation = new Vector(0, penetration);
                return separation;
            }
            // Find closest vertex
            double px1 = Math.Abs(rectangle.BoundingBox.X - circle.Position.X);
            double py1 = Math.Abs(rectangle.BoundingBox.Y - circle.Position.Y);
            double px2 = Math.Abs(rectangle.BoundingBox.Right - circle.Position.X);
            double py2 = Math.Abs(rectangle.BoundingBox.Bottom - circle.Position.Y);
            double cornerX = Math.Min(px1, px2);
            double cornerY = Math.Min(py1, py2);
            Vector distCorner = new Vector(cornerX, cornerY);
            if (distCorner.LengthSquared < circle.Radius * circle.Radius)
            {
                double penetration = distCorner.Length - circle.Radius;
                separation.Normalize();
                separation *= penetration;
                return separation;
            }
            return null;
        }

        public static Vector? IntersectSquareSquare(ICollidable m1, ICollidable m2)
        {
            if (m1.BoundingBox.IntersectsWith(m2.BoundingBox))
            {
                // We don't really need this result yet, just indicate intersection
                return new Vector();
            }
            return null;
        }

        #endregion

        internal static Vector GetDirectedVector(double direction)
        {
            double x = Math.Cos(direction);
            double y = Math.Sin(direction);
            Vector result = new Vector(x, y);
            return result;
        }

        public static void BoundsCheck(List<ICollidable> models, List<Tile> tiles)
        {
            foreach (var model in models)
            {
                Wallscan(model, tiles);
            }
        }

        private static void Wallscan(ICollidable current, List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                VrfyBounds(current, tile);
            }
        }

        private static void VrfyBounds(ICollidable model, Tile tile)
        {
            double tileHalf = 0.5; // this may have to change, if we start checking irregular boundaries
            double distX = Math.Abs(tile.X - model.Position.X);
            double distY = Math.Abs(tile.Y - model.Position.Y);

            if (distX > tileHalf + model.Radius || distY > tileHalf + model.Radius)
            {
                return;
            }

            if (distX <= tileHalf + model.Radius && distY <= tileHalf)
            {
                if (model is Projectile)
                {
                    model.Die();
                    return;
                }

                double penetration = 0;

                if (tile.X - model.Position.X < 0)
                { // right side
                    penetration = distX - (tileHalf + model.Radius);
                }
                else
                { // left side
                    penetration = (tileHalf + model.Radius) - distX;
                }

                model.Position =
                    new Vector(model.Position.X - penetration, model.Position.Y); // resolve

                return;
            }

            if (distX <= tileHalf && distY <= tileHalf + model.Radius)
            {
                if (model is Projectile)
                {
                    model.Die();
                    return;
                }

                double penetration = 0;

                if (tile.Y - model.Position.Y < 0)
                { // bottom side
                    penetration = distY - (tileHalf + model.Radius);
                }
                else
                { // top side
                    penetration = (tileHalf + model.Radius) - distY;
                }

                model.Position =
                    new Vector(model.Position.X, model.Position.Y - penetration); // resolve
            }
        }

    }
}
