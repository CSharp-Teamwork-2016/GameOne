namespace GameOne.Source.Entities
{
    using System.Windows;

    using Enumerations;
    using Interfaces;
    using Renderer;

    public abstract class Model : Entity, IRenderable
    {
        private Spritesheet sprite;
        protected State state;

        protected Model(double x, double y, double direction, double radius, Spritesheet sprite)
        {
            this.Position = new Vector(x, y);
            this.Direction = direction;
            this.Radius = radius;
            this.sprite = sprite;
            this.state = State.IDLE;
        }

        public bool Alive => state != State.DEAD;

        public Vector Position { get; set; }

        public double Direction { get; protected set; }

        public double Radius { get; set; }

        public State State => this.state;

        public abstract void Render();
    }
}