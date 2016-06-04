namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Interfaces;
    using GameOne.Source.Renderer;

    public abstract class Model : Entity, IRenderable
    {
        protected System.Windows.Vector position;
        protected double direction;
        protected double radius;
        protected Spritesheet sprite;
        protected State state;

        protected Model(double x, double y, double direction, double radius, Spritesheet sprite)
        {
            position = new System.Windows.Vector(x, y);
            this.direction = direction;
            this.radius = radius;
            this.sprite = sprite;
            state = State.IDLE;
        }


        public double X { get { return position.X; } }

        public double Y { get { return position.Y; } }

        public double Direction { get { return direction; } set { direction = value; } }

        public double Radius { get { return radius; } set { radius = value; } }

        public State State { get { return state; } }

        public abstract void Render();
    }
}