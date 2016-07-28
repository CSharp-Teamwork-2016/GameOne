namespace GameOne.Source.Entities
{
    using System.Windows;

    using Enumerations;
    using Interfaces;
    using Renderer;

    public abstract class Model : Entity, IRenderable
    {
        protected State state;
        private Spritesheet sprite;

        protected Model(double x, double y, double direction, double radius, Spritesheet sprite)
        {
            this.Position = new Vector(x, y);
            this.Direction = direction;
            this.Radius = radius;
            this.sprite = sprite;
            this.state = State.IDLE;
        }

        public bool Alive => this.state != State.DEAD;

        public Vector Position { get; set; }

        public double Direction { get; protected set; }

        public double Radius { get; set; }

        public State State => this.state;
        
        public virtual void Die()
        {
            this.state = State.DEAD;
        }
    }
}