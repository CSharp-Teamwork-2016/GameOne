namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    public abstract class Item : Model
    {
        private ItemType type;

        protected Item(ItemType type, double x, double y, double direction, double radius, Spritesheet sprite)
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
    }
}
