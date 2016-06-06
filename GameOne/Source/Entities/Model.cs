namespace GameOne.Source.Entities
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Interfaces;
    using GameOne.Source.Renderer;

    public abstract class Model : Entity, IRenderable
    {
        protected System.Windows.Vector position;
        private double direction;
        private double radius;
        private Spritesheet sprite;
        private State state;

        protected Model(double x, double y, double direction, double radius, Spritesheet sprite)
        {
            this.position = new System.Windows.Vector(x, y);
            this.direction = direction;
            this.radius = radius;
            this.sprite = sprite;
            this.state = State.IDLE;
        }

		public System.Windows.Vector Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public double X
        {
            get
            {
                return this.position.X;
            }
			set
			{
				position.X = value;
			}
        }

        public double Y
        {
            get
            {
                return this.position.Y;
            }
			set
			{
				position.Y = value;
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

        public State State
        {
            get
            {
                return this.state;
            }
        }

        public abstract void Render();
    }
}