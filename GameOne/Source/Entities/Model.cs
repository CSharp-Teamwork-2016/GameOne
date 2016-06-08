namespace GameOne.Source.Entities
{
    using System.Windows;

    using GameOne.Source.Enumerations;
    using GameOne.Source.Interfaces;
    using GameOne.Source.Renderer;

    public abstract class Model : Entity, IRenderable
    {
        private Vector position;
        private double direction;
        private double radius;
        private Spritesheet sprite;
        private State state;

        protected Model(double x, double y, double direction, double radius, Spritesheet sprite)
        {
            this.position = new Vector(x, y);
            this.direction = direction;
            this.radius = radius;
            this.sprite = sprite;
            this.state = State.IDLE;
        }

		public Vector Position
		{
			get
			{
				return this.position;
			}

			set
			{
				this.position += value;
			}
		}

        public double Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
            }
        }

        public double Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                this.radius = value;
            }
        }

        public State State => this.state;

        public abstract void Render();
    }
}