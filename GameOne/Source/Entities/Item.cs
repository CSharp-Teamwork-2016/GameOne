namespace GameOne.Source.Entities
{
    using System;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public class Item : Model
    {
        private ItemType type;

        public Item(double x, double y, double direction, double radius, 
                Spritesheet sprite, ItemType type)
            : base(x, y, direction, radius, sprite)
        {
            this.type = type;
        }

        public ItemType Type
        {
            get
            {
                return this.type;
            }
        }

        public override void Update(double time)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override void Render()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
