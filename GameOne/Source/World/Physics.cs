namespace GameOne.Source.World
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Entities;

    public class Physics
    {
        //All static
        #region Methods

        public static void CollisionResolution(List<Model> models)
        {
            foreach (Model model in models)
            {
                Hitscan(model, models.Where(current => current != model).ToList());
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
            if (!e1.Alive || !e2.Alive) return;
            Vector separation = Vector.Subtract(e1.Position, e2.Position);
            double dist = separation.Length;
            double penetration = dist - (e1.Radius + e2.Radius);

            if (penetration < 0)
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
                        return;
                    }
                    else return;
                }

                separation.Normalize();
                separation = Vector.Multiply(separation, penetration / 2);
                e1.Position -= separation;
                e2.Position += separation;
                //Loop.DebugInfo += "Collision\n";
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

        public static void BoundsCheck(List<Model> models, List<Tile> tiles)
        {
            foreach (Model model in models)
            {
                Wallscan(model, tiles);
            }
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
