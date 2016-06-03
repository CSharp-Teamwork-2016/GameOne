namespace GameOne.Source.Entities.Characters
{
    using System.Collections.Generic;

    using GameOne.Source.Entities.Interfaces;
    using GameOne.Source.Entities.Items;

    using Microsoft.Xna.Framework;

    public abstract class Character : Entity, IPosition
    {
        protected Character(string id, Vector2 position, int health, int attack, int defense, int stamina, int range)
            : base(id)
        {
            this.Position = position;
            this.Health = health;
            this.Attack = attack;
            this.Defense = defense;
            this.Stamina = stamina;
            this.Range = range;
            this.IsAlive = true;
            this.Inventory = new List<Item>();
        }

        public Vector2 Position { get; set; }

        public int Health { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Stamina { get; set; }

        public int Range { get; set; }

        public bool IsAlive { get; set; }

        public List<Item> Inventory { get; private set; }

        public void AddToInventory(Item item)
        {
            this.Inventory.Add(item);
            this.ApplyItemEffects(item);
        }

        public void RemoveFromInventory(Item item)
        {
            this.Inventory.Remove(item);
            this.RemoveItemEffects(item);
        }

        protected virtual void ApplyItemEffects(Item item)
        {

        }

        protected virtual void RemoveItemEffects(Item item)
        {

        }

    }
}
