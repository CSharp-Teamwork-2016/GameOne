namespace GameOne.Source.World
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows;

    using Entities;

	public class Physics
	{
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
			Vector separation = Vector.Subtract(e1.Position, e2.Position);
			double dist = separation.Length;
			double penetration = dist - (e1.Radius + e2.Radius);
			if (penetration < 0)
			{
				separation.Normalize();
				separation = Vector.Multiply(separation, penetration / 2);
				e1.Position -= separation;
				e2.Position += separation;
				Loop.DebugInfo += "Collision\n";
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
			double distX = Math.Abs(tile.X - model.X);
			double distY = Math.Abs(tile.Y - model.Y);
			if (distX > tileHalf + model.Radius || distY > tileHalf + model.Radius) return;
			if (distX <= tileHalf + model.Radius && distY <= tileHalf)
			{
				double penetration = 0;
				if (tile.X - model.X < 0)
				{ // right side
					penetration = distX - (tileHalf + model.Radius);
				}
				else
				{ // left side
					penetration = (tileHalf + model.Radius) - distX;
				}
				model.X -= penetration; // resolve
				return;
			}
			if (distX <= tileHalf && distY <= tileHalf + model.Radius)
			{
				double penetration = 0;
				if (tile.Y - model.Y < 0)
				{ // bottom side
					penetration = distY - (tileHalf + model.Radius);
				}
				else
				{ // top side
					penetration = (tileHalf + model.Radius) - distY;
				}
				model.Y -= penetration; // resolve
			}
		}
	}
}
