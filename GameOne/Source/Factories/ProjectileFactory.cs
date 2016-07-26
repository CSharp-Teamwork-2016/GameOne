namespace GameOne.Source.Factories
{
    using System;
    using System.Windows;
    using Entities;
    using Enumerations;
    using World;

    public class ProjectileFactory
    {
        private const double BulletSpeed = 8;

        public static Projectile MakeProjectile(Character source, ProjectileType type)
        {
            double direction = source.Direction % (2 * Math.PI);
            double velocityX = 0;
            double velocityY = 0;

            if (direction == Physics.UpDirection)
            {
                velocityY = -BulletSpeed;
            }
            else if (direction == Physics.DownDirection)
            {
                velocityY = BulletSpeed;
            }
            else if (direction == Physics.LeftDirection)
            {
                velocityX = -BulletSpeed;
            }
            else if (direction == Physics.RightDirection)
            {
                velocityX = BulletSpeed;
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
