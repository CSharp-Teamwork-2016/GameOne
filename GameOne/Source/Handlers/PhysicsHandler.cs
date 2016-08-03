namespace GameOne.Source.Handlers
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

        public static void ResolveCollisions(ICollidable model1, ICollidable model2)
        {
            Vector? result = PhysicsEngine.Intersect(model1, model2);
            if (result != null)
            {
                Resolve(model1, model2, result.Value);
                Resolve(model2, model1, -result.Value);
                model1.Respond(model2);
                model2.Respond(model1);
            }
        }

        public static void ResolveCollisions(IList<ICollidable> models)
        {
            while (models.Count > 0)
            {
                ICollidable current = models[models.Count - 1];
                models.RemoveAt(models.Count - 1);
                foreach (var model in models)
                {
                    Vector? result = PhysicsEngine.Intersect(current, model);
                    if (result != null)
                    {
                        Resolve(current, model, (Vector)result);
                        Resolve(model, current, -(Vector)result);
                        current.Respond(model);
                        model.Respond(current);
                    }
                }
            }
        }

        private static void Resolve(ICollidable current, ICollidable model, Vector penetration)
        {
            switch (current.CollisionResponse)
            {
                case CollisionResponse.Ignore:
                    return;
                case CollisionResponse.Immovable:
                    return;
                case CollisionResponse.DestroyOnImpact:
                    return;
                case CollisionResponse.PickUp:
                    return;
                case CollisionResponse.Project:
                    if (model.CollisionResponse == CollisionResponse.Project)
                    {
                        penetration *= 0.5;
                    }
                    current.Position -= penetration;
                    return;
            }
        }
    }
}
