namespace GameOne.Source.Entities
{
    using Enumerations;
    using Renderer;

    public class Item : Model
    {
        public Item(double x, double y, double direction, double radius, Spritesheet sprite, ItemType type)
            : base(x, y, direction, radius, sprite)
        {
            this.Type = type;
        }

        public ItemType Type { get; }

        public override void Update(double time)
        {
            // Loop.debugInfo += string.Format($"{Alive}\n");
        }

        public void Collect()
        {
            this.state = State.DEAD;
        }
    }
}