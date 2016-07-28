namespace GameOne.Source.Handlers
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;
    using Enumerations;
    using Events;
    using Factories;
    using World;

    public class EntityHandler
    {
        private readonly List<Entity> register;

        public EntityHandler()
        {
            this.register = new List<Entity>();
        }

        public void ProcessEntities(List<Entity> entities, List<Tile> tiles, double time)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(time);
            }

            Physics.CollisionResolution(entities
                    .OfType<Model>()
                    .Where(e => e.Alive)
                    .ToList());

            var modelEntitiesTo = entities.OfType<Model>().ToList();
            var tileWalls = tiles.Where(tile => tile.TileType == TileType.Wall).ToList();

            Physics.BoundsCheck(modelEntitiesTo, tileWalls);

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
