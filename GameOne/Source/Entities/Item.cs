namespace GameOne.Source.Entities
{
    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public class Item : Model, ICollidable
    {
        public Item(double x, double y, double direction, double radius, IRenderingStrategy sprite, ItemType type)
            : base(x, y, direction, radius, sprite)
        {
            this.Type = type;
            IsSolid = false;
        }

        public ItemType Type { get; }

        public bool IsSolid { get; private set; }

        public CollisionResponse CollisionResponse
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

        public void Respond(ICollidable model)
        {
            if (model is Player)
            {
                this.Collect();
            }
        }
    }
}