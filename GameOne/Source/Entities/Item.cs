﻿namespace GameOne.Source.Entities
{
    using Enumerations;
    using Interfaces;

    public class Item : Model, ICollidable
    {
        public Item(double x, double y, double direction, double radius, IRenderingStrategy sprite, ItemType type)
            : base(x, y, direction, radius, sprite)
        {
            this.Type = type;
            IsSolid = true;
        }

        public ItemType Type { get; }

        public bool IsSolid { get; private set; }

        public CollisionResponse Response
        {
            get
            {
                return CollisionResponse.PickUp;
            }
        }

        public Shape CollisionShape
        {
            get
            {
                return Shape.Circle;
            }
        }

        public void Collect()
        {
            this.state = State.DEAD;
        }
    }
}