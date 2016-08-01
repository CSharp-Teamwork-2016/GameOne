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
    using Entities.Zones;
    using Microsoft.Xna.Framework;
    using System;
    using System.Windows;
    public class EntityHandler
    {
        private readonly List<Entity> register;
        private readonly List<DamageZone> damageZones;

        public EntityHandler()
        {
            this.register = new List<Entity>();
            damageZones = new List<DamageZone>();
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
            // Update damage zones
            foreach (var zone in damageZones)
            {
                zone.Update(time);
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

            this.NotifyInDamageZone(entities);

            this.register.Clear();

            // Remove dead entities
            this.RemoveDead(entities);
            this.RemoveDeadZones(damageZones);
        }

        private void NotifyInDamageZone(List<Entity> entities)
        {
            foreach (var model in entities.OfType<ICharacter>())
            {
                foreach (var zone in this.damageZones)
                {
                    if (zone.Source == model) continue;
                    Vector position = model.Position;
                    if (position.X >= zone.X &&
                        position.X <= zone.X + zone.Width &&
                        position.Y >= zone.Y &&
                        position.Y <= zone.Y + zone.Height)
                    {
                        model.TakeDamage(zone.Source.Damage);
                    }
                }
            }
        }

        public void Subscribe(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is Character && !(entity is Player))
                {
                    ((Character)entity).FireProjectileEvent += this.RegisterProjectile;
                    ((Character)entity).AttackEvent += this.RegisterAttack;
                }
            }
        }

        public void SubscribeToPlayer(Player player)
        {
            player.FireProjectileEvent += this.RegisterProjectile;
            player.AttackEvent += this.RegisterAttack;
        }

        private void RemoveDead(List<Entity> entities)
        {
            List<Entity> result = entities.Where(e => !e.Alive).ToList();
            foreach (var item in result)
            {
                entities.Remove(item);
            }
        }

        private void RemoveDeadZones(List<DamageZone> zones)
        {
            List<DamageZone> result = zones.Where(z => !z.Alive).ToList();
            foreach (var item in result)
            {
                zones.Remove(item);
            }
        }

        private void RegisterProjectile(object sender, ProjectileEventArgs e)
        {
            Projectile projectile = ProjectileFactory.MakeProjectile((Character)sender, e.Type);
            this.register.Add(projectile);
        }

        private void RegisterAttack(object setnder, MeleeAttackEventArgs e)
        {
            ICharacter source = e.Source;

            double p1x = source.Position.X + (Math.Cos(source.Direction + Math.PI / 2) * 0.5);
            double p1y = source.Position.Y + (Math.Sin(source.Direction + Math.PI / 2) * 0.5);

            double pw = (1.2 * Math.Cos(source.Direction)) + (1 * Math.Sin(source.Direction));
            double ph = (1.2 * Math.Sin(source.Direction)) - (1 * Math.Cos(source.Direction));

            double leftA = Math.Min(p1x, p1x + pw);
            double rightA = Math.Max(p1x, p1x + pw);
            double topA = Math.Min(p1y, p1y + ph);
            double bottomA = Math.Max(p1y, p1y + ph);

            DamageZone zone = new DamageZone(leftA, topA, rightA - leftA, bottomA - topA, e.Source, 0.2);
            damageZones.Add(zone);
        }
    }
}
