namespace GameOne.Source.Entities
{
    using Interfaces;

    public abstract class Zone : Entity, IZone
    {
        protected Zone(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}
