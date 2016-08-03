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

        #region Methods

        public static bool Intersect(ICollidable m1, ICollidable m2)
        {
            // Bounding box check
            if (!IntersectSquareSquare(m1, m2))
            {
                return false;
            }
            if (m1.CollisionShape == Shape.Circle)
            {
                if (m2.CollisionShape == Shape.Circle)
                {
                    return IntersectCircleCircle(m1, m2);
                }
                else if (m2.CollisionShape == Shape.Square)
                {
                    return IntersectCircleSquare(m1, m2);
                }
            }
            else if (m1.CollisionShape == Shape.Square)
            {
                if (m2.CollisionShape == Shape.Circle)
                {
                    return IntersectCircleSquare(m2, m1);
                }
                else if (m2.CollisionShape == Shape.Square)
                {
                    return true; // Already confirmed in preliminary check
                }
            }
            return false;
        }

        public static bool IntersectCircleCircle(ICollidable m1, ICollidable m2)
        {
            Vector separation = Vector.Subtract(m1.Position, m2.Position);
            double dist = separation.Length;
            double penetration = dist - (m1.Radius + m2.Radius);

            if (penetration < 0)
            {
                return true;
            }
            return false;
        }

        public static bool IntersectCircleSquare(ICollidable circle, ICollidable square)
        {
            double horizontalHalf = square.BoundingBox.Width / 2;
            double verticalHalf = square.BoundingBox.Height / 2;
            double distX = Math.Abs(square.Position.X - circle.Position.X);
            double distY = Math.Abs(square.Position.Y - circle.Position.Y);

            if (distX <= horizontalHalf + circle.Radius && distY <= verticalHalf)
            {
                return true;
            }
            if (distX <= horizontalHalf && distY <= verticalHalf + circle.Radius)
            {
                return true;
            }
            // Find closest vertex
            double px1 = Math.Abs(square.BoundingBox.X - circle.Position.X);
            double py1 = Math.Abs(square.BoundingBox.Y - circle.Position.Y);
            double px2 = Math.Abs(square.BoundingBox.Right - circle.Position.X);
            double py2 = Math.Abs(square.BoundingBox.Bottom - circle.Position.Y);
            double cornerX = Math.Min(px1, px2);
            double cornerY = Math.Min(py1, py2);
            Vector distCorner = new Vector(cornerX, cornerY);
            if (distCorner.Length < circle.Radius)
            {
                return true;
            }
            return false;
        }

        public static bool IntersectSquareSquare(ICollidable m1, ICollidable m2)
        {
            if (m1.BoundingBox.IntersectsWith(m2.BoundingBox))
            {
                return true;
            }
            return false;
        }

        public static void DetectCollisions(List<Model> models)
        {
            // Save first item in a temp variable
            // Remove from list
            // Check temp item against remaining collection
            // Repeat until collection is empty
            foreach (Model model in models)
            {
                Hitscan(model, models.Where(current => current != model).ToList());
            }
        }

        public static void BoundsCheck(List<Model> models, List<Tile> tiles)
        {
            foreach (Model model in models)
            {
                Wallscan(model, tiles);
            }
        }

        private static void Hitscan(Model current, List<Model> models)
        {
            foreach (Model model in models)
            {
                ResolveCollision(current, model);
            }
        }

        private static void ResolveCollision(Model e1, Model e2)
        {
            if (!e1.Alive || !e2.Alive)
            {
                return;
            }

            Vector separation = Vector.Subtract(e1.Position, e2.Position);
            double dist = separation.Length;
            double penetration = dist - (e1.Radius + e2.Radius);

            if (penetration < 0)
            {
                if (HandleItems(e1, e2))
                {
                    return;
                }

                if (HandleProjectiles(e1, e2))
                {
                    return;
                }

                separation.Normalize();
                separation = Vector.Multiply(separation, penetration / 2);
                e1.Position -= separation;
                e2.Position += separation;
                // Loop.DebugInfo += "Collision\n";
                if (e1 is Player && e2 is Enemy)
                {
                    ((Character)e1).TakeDamage(((Character)e2).Damage);
                }
                else if (e2 is Player && e1 is Enemy)
                {
                    ((Character)e2).TakeDamage(((Character)e1).Damage);
                }
            }
        }

        internal static Vector GetDirectedVector(double direction)
        {
            double x = Math.Cos(direction);
            double y = Math.Sin(direction);
            Vector result = new Vector(x, y);
            return result;
        }

        private static bool HandleItems(Model e1, Model e2)
        {
            if (e1 is Item)
            {
                Model t = e2;
                e2 = e1;
                e1 = t;
            }

            if (e2 is Item)
            {
                if (e1 is Player)
                {
                    ((Item)e2).Collect();
                    ((Player)e1).PickUpItem(((Item)e2).Type);
                }

                return true;
            }

            return false;
        }

        private static bool HandleProjectiles(Model e1, Model e2)
        {
            if (e1 is Projectile)
            {
                Model t = e2;
                e2 = e1;
                e1 = t;
            }

            if (e2 is Projectile)
            {
                Projectile projectile = (Projectile)e2;
                if (e1 is Character)
                {
                    Character target = (Character)e1;
                    if (target is Player)
                    {
                        if (projectile.Source is Enemy)
                        {
                            projectile.Die();
                            target.TakeDamage(projectile.Source.Damage);
                        }

                        return true;
                    }
                    else if (target is Enemy)
                    {
                        if (projectile.Source is Player)
                        {
                            projectile.Die();
                            target.TakeDamage((int)(projectile.Source.Damage * 0.4));
                        }

                        return true;
                    }
                }

                return true;
            }

            return false;
        }

        private static void Wallscan(Model current, List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                VrfyBounds(current, tile);
            }
        }

        private static void VrfyBounds(Model model, Tile tile)
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

        #endregion Methods
    }
}
