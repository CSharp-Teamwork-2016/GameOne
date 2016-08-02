namespace GameOne.Source.Factories
{
    using System;
    using System.Windows;
    using Entities;
    using Enumerations;
    using World.Physics;

    public class ProjectileFactory
    {
        public static Projectile MakeProjectile(Character source, ProjectileType type)
        {
            double direction = source.Direction % (2 * Math.PI);
            double velocityX = 0;
            double velocityY = 0;

            if (direction == PhysicsEngine.UpDirection)
            {
                velocityY = -PhysicsEngine.ProjectileSpeed;
            }
            else if (direction == PhysicsEngine.DownDirection)
            {
                velocityY = PhysicsEngine.ProjectileSpeed;
            }
            else if (direction == PhysicsEngine.LeftDirection)
            {
                velocityX = -PhysicsEngine.ProjectileSpeed;
            }
            else if (direction == PhysicsEngine.RightDirection)
            {
                velocityX = PhysicsEngine.ProjectileSpeed;
            }

            double radius;
            switch (type)
            {
                case ProjectileType.Bullet:
                    radius = 0.15;
                    break;
                default:
                    radius = 0.15;
                    break;
            }

            Vector velocity = new Vector(velocityX, velocityY);

            return new Projectile(source.Position.X, source.Position.Y, direction, radius, null, source, velocity);
        }
    }
}
