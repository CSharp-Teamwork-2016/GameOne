﻿namespace GameOne.Source.Handlers
{
    using System.Windows;

    using Interfaces;
    using Enumerations;
    using World.Physics;
    using System.Collections.Generic;

    public class PhysicsHandler
    {
        public static void UpdateMovement(IMovable model, double time)
        {
            if (model.Velocity.Length > 0)
            {
                model.Position += model.Velocity * time;

                if (model.MovementType == MovementType.Normal)
                {
                    model.Velocity = ApplyFriction(model.Velocity, time);
                }
            }
        }

        private static Vector ApplyFriction(Vector velocity, double time)
        {
            Vector friction = velocity;

            friction.Negate();
            friction.Normalize();

            friction *= PhysicsEngine.FrictionCoefficient * time;
            velocity += friction;

            if (velocity.LengthSquared <= friction.LengthSquared)
            {
                velocity = new Vector(0, 0);
            }
            else if (velocity.LengthSquared < 0.01)
            {
                velocity = new Vector(0, 0);
            }

            return velocity;
        }

        public static void ResolveCollisions(IList<ICollidable> models)
        {
            while (models.Count > 0)
            {
                ICollidable current = models[models.Count - 1];
                models.Remove(current);
                foreach (var model in models)
                {
                    Vector? result = PhysicsEngine.Intersect(current, model);
                    if (result != null)
                    {
                        Vector remainder = Resolve(current, (Vector)result);
                        remainder.Negate();
                        Resolve(model, remainder);
                        current.Respond(model);
                        model.Respond(current);
                    }
                }
            }
        }

        private static Vector Resolve(ICollidable model, Vector penetration)
        {
            switch (model.CollisionResponse)
            {
                case CollisionResponse.DestroyOnImpact:
                    return penetration;
                case CollisionResponse.Ignore:
                    return new Vector(0,0);
                case CollisionResponse.Immovable:
                    return penetration;
                case CollisionResponse.PickUp:
                    return new Vector(0, 0);
                case CollisionResponse.Project:
                    penetration *= 0.5;
                    model.Position -= penetration;
                    return penetration;
            }
            return penetration;
        }
    }
}
