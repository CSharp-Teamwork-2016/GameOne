namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    using Microsoft.Xna.Framework;

    public abstract class Model : Entity
    {
        protected Model(string id, Vector2 position, string direction, double radius, Spritesheet sprite, State state)
            : base(id)
        {
            this.Position = position;
            this.Direction = direction;
            this.Radius = radius;
            this.Sprite = sprite;
            this.State = state;
        }
        

        public Vector2 Position { get; set; }

        public string Direction { get; set; }

        public double Radius { get; set; }

        public Spritesheet Sprite { get; set; }

        public State State { get; set; }

        public abstract override void Update();
    }
}