﻿namespace GameOne.Source.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Entities;
    using Entities.Zones;
    using Enumerations;
    using EventArgs;
    using Factories;
    using Interfaces;
    using World;
    using Entities.Enemies;

    public class EntityHandler
    {
        private readonly List<Entity> register;
        private readonly List<DamageZone> damageZones;

        private Level level;

        public EntityHandler(Level level)
        {
            this.level = level;
            this.register = new List<Entity>();
            damageZones = new List<DamageZone>();
        }

        public void ProcessEntities(double time)
        {
            // Hot pipeline
            for (int i = 0; i < level.Entities.Count; i++)
            {
                Entity entity = level.Entities[i];
                if (!entity.Alive)
                {
                    continue;
                }
                // Update internal state
                if (entity is IUpdatable)
                {
                    ((IUpdatable)entity).Update(time);
                }
                // Update physical state
                if (entity is IMovable)
                {
                    PhysicsHandler.UpdateMovement(((IMovable)entity), time);
                }
                // Collision detection
                if (entity is ICollidable)
                {
                    // Against entities
                    for (int j = i + 1; j < level.Entities.Count; j++)
                    {
                        if (!level.Entities[j].Alive || !(level.Entities[j] is ICollidable))
                        {
                            continue;
                        }
                        PhysicsHandler.ResolveCollisions((ICollidable)entity, (ICollidable)level.Entities[j]);
                    }
                    // Against geometry
                    for (int k = 0; k < level.Walls.Count; k++)
                    {
                        if (Math.Abs(((ICollidable)entity).Position.X - level.Walls[k].X) > 2 ||
                            Math.Abs(((ICollidable)entity).Position.Y - level.Walls[k].Y) > 2) continue;
                        PhysicsHandler.ResolveCollisions((ICollidable)entity, level.Walls[k]);
                    }
                }
            }

            // Update damage zones
            foreach (var zone in damageZones)
            {
                zone.Update(time);
            }

            // Add registered entities to list
            foreach (var item in this.register)
            {
                level.Entities.Add(item);
            }

            this.NotifyInDamageZone(level.Entities);

            this.register.Clear();

            // Remove dead entities
            this.RemoveDead(level.Entities);
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
                if (entity is Enemy)
                {
                    Enemy character = (Enemy)entity;
                    character.FireProjectileEvent += this.RegisterProjectile;
                    character.AttackEvent += this.RegisterAttack;
                    character.KilledEvent += this.RegisterKill;
                }
            }
        }

        public void SubscribeToPlayer(Player player)
        {
            player.FireProjectileEvent += this.RegisterProjectile;
            player.AttackEvent += this.RegisterAttack;
        }

        public void SubscribeToBoss(object boss)
        {
            ((Harvester)boss).BossActionHandler += OnBossAction;
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

        private void RegisterKill(object source, KilledEventArgs e)
        {
            this.level.EnemySlain();
            this.level.Player.GainXP(e.XpAward);
        }
        
        private void OnBossAction(object caller, BossActionArgs args)
        {
            switch (args.Action)
            {
                case BossAction.RaiseChargers:
                    Enemy charger1 = EnemyFactory.MakeEnemy(args.X - 4, args.Y - 4, EnemyType.Charger, 4);
                    Enemy charger2 = EnemyFactory.MakeEnemy(args.X + 4, args.Y - 4, EnemyType.Charger, 4);
                    Enemy charger3 = EnemyFactory.MakeEnemy(args.X - 4, args.Y + 4, EnemyType.Charger, 4);
                    Enemy charger4 = EnemyFactory.MakeEnemy(args.X + 4, args.Y + 4, EnemyType.Charger, 4);
                    register.Add(charger1);
                    register.Add(charger2);
                    register.Add(charger3);
                    register.Add(charger4);
                    break;
            }
        }
    }
}
