namespace GameOne.Source.Handlers
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;
    using Enumerations;
    using Events;
    using Factories;
    using World;
    using World.Physics;
    using Interfaces;
    public class EntityHandler
    {
        private readonly List<Entity> register;

        public EntityHandler()
        {
            this.register = new List<Entity>();
        }

        public void ProcessEntities(List<Entity> entities, List<Tile> tiles, double time)
        {
            // Update internal state
            foreach (Entity entity in entities.OfType<IUpdatable>())
            {
                ((IUpdatable)entity).Update(time);
            }
            // Update physical state
            foreach (Entity entity in entities.OfType<IMovable>())
            {
                PhysicsHandler.UpdateMovement(((IMovable)entity), time);
            }

            PhysicsEngine.DetectCollisions(entities
                    .OfType<Model>()
                    .Where(e => e.Alive)
                    .ToList());

            var modelEntitiesTo = entities.OfType<Model>().ToList();
            var tileWalls = tiles.Where(tile => tile.TileType == TileType.Wall).ToList();

            PhysicsEngine.BoundsCheck(modelEntitiesTo, tileWalls);

            // Add new entities to list
            foreach (var item in this.register)
            {
                entities.Add(item);
            }

            this.register.Clear();

            // Remove dead entities
            this.RemoveDead(entities);
        }

        public void Subscribe(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is Character && !(entity is Player))
                {
                    ((Character)entity).FireProjectileEvent += this.RegisterProjectile;
                }
            }
        }

        public void SubscribeToPlayer(Player player)
        {
            player.FireProjectileEvent += this.RegisterProjectile;
        }

        private void RemoveDead(List<Entity> entities)
        {
            List<Entity> result = entities.Where(e => e is Model && !((Model)e).Alive).ToList();
            foreach (var item in result)
            {
                entities.Remove(item);
            }
        }

        private void RegisterProjectile(object sender, ProjectileEventArgs e)
        {
            Projectile projectile = ProjectileFactory.MakeProjectile((Character)sender, e.Type);
            this.register.Add(projectile);
        }
    }
}
