namespace GameOne.Source.Handlers
{
    using Interfaces;
    using Enumerations;
    using System.Windows;

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

            friction *= 15 * time;
            velocity += friction;

            if (velocity.Length <= friction.Length)
            {
                velocity = new Vector(0, 0);
            }
            else if (velocity.Length < 0.1)
            {
                velocity = new Vector(0, 0);
            }

            return velocity;
        }
    }
}
