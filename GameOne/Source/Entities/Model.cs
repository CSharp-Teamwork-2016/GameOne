namespace GameOne.Source.Entities
{
    using System.Windows;

    using Enumerations;
    using Interfaces;
    using System;

    [Serializable]
    public abstract class Model : Entity, IRenderable, IPhysicsBody
    {
        private IRenderingStrategy sprite;

        protected Model(double x, double y, double direction, double radius, IRenderingStrategy sprite)
        {
            this.Position = new Vector(x, y);
            this.Direction = direction;
            this.Radius = radius;
            this.sprite = sprite;
            this.state = State.IDLE;
        }

        public Vector Position { get; set; }

        public double Direction { get; protected set; }

        public double Radius { get; set; }

        public Rect BoundingBox
        {
            get
            {
                return new Rect(this.Position.X - this.Radius, this.Position.Y - this.Radius, this.Radius * 2, this.Radius * 2);
            }
        }

        public State State => this.state;

        public IRenderingStrategy RenderingStrategy()
        {
            return this.sprite;
        }
    }
}