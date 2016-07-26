namespace GameOne.Source.World
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Entities;

    public class Physics
    {
        public static readonly double UpDirection = Math.Round(1.5 * Math.PI, 2);
        public static readonly double DownDirection = Math.Round(0.5 * Math.PI, 2);
        public static readonly double LeftDirection = Math.Round(Math.PI, 2);
        public static readonly double RightDirection = 0.0;

        #region Methods

        public static void CollisionResolution(List<Model> models)
        {
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
                Resolve(current, model);
            }
        }

        private static void Resolve(Model e1, Model e2)
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
