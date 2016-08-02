namespace GameOne.Source.World
{
    using System;

    [Serializable]
    public class Room
    {
        #region Constructors

        public Room(int x, int y, int width, int height)
        {
            this.Width = LevelMaker.Minsize + LevelMaker.Rand(LevelMaker.Maxsize - LevelMaker.Minsize);
            this.Width = Math.Min(this.Width, width);
            this.Height = LevelMaker.Minsize + LevelMaker.Rand(LevelMaker.Maxsize - LevelMaker.Minsize);
            this.Height = Math.Min(this.Height, height);

            if (width - this.Width > 0)
            {
                this.X = x + LevelMaker.Rand(width - this.Width);
            }
            else
            {
                this.X = x;
            }

            if (height - this.Height > 0)
            {
                this.Y = y + LevelMaker.Rand(height - this.Height);
            }
            else
            {
                this.Y = y;
            }

            this.OriginX = this.X + (this.Width / 2);
            this.OriginY = this.Y + (this.Height / 2);
        }

        #endregion Constructors

        #region Properties

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public int OriginX { get; }

        public int OriginY { get; }

        #endregion Properties
    }
}