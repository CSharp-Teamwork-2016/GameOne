namespace GameOne.Source.World
{
    using System;

    public class Partition
    {
        #region Fields

        private readonly Partition parent;

        #endregion Fields

        #region Constructors

        public Partition(int x, int y, int width, int height, Partition parent)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.parent = parent;
        }

        #endregion Constructors

        #region Properties

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public bool IsHorizontal { get; private set; }

        public Partition LeftLeaf { get; private set; }

        public Partition RightLeaf { get; private set; }

        public Room Room { get; private set; }

        public Hallway Hallway { get; private set; }

        public bool HasLeaves => this.LeftLeaf != null;

        #endregion Properties

        #region Methods

        public bool TrySplit()
        {
            if (this.HasLeaves)
            {
                return this.LeftLeaf.TrySplit() || this.RightLeaf.TrySplit();
            }

            return this.Split();
        }

        public void MakeRoom()
        {
            if (this.HasLeaves)
            {
                this.LeftLeaf.MakeRoom();
                this.RightLeaf.MakeRoom();
            }
            else
            {
                this.Room = new Room(this.X, this.Y, this.Width, this.Height);
            }
        }

        public void MakeHallway()
        {
            if (!this.HasLeaves)
            { // Connect room to parent partition origin
                this.Hallway = new Hallway(this.Room, this.parent, this.parent.IsHorizontal);
            }
            else
            { // Connect leaf partitions to each other
                this.LeftLeaf.MakeHallway();
                this.RightLeaf.MakeHallway();
                this.Hallway = new Hallway(this.LeftLeaf, this.RightLeaf, this.IsHorizontal);
            }
        }

        private bool Split()
        {
            this.IsHorizontal = false; // false = horizontal, true = vertical

            if ((double)this.Width / this.Height > LevelMaker.Maxratio)
            {
                this.IsHorizontal = true;
            }
            else if ((double)this.Height / this.Width > LevelMaker.Maxratio)
            {
                this.IsHorizontal = false;
            }
            else
            {
                this.IsHorizontal = LevelMaker.Rand(1) == 1; // 0 = horizontal, 1 = vertical
            }

            if (!this.IsHorizontal)
            { // top and bottom leaves
                if (this.Height <= 2 * LevelMaker.Minsize)
                {
                    return false;
                }

                var half = LevelMaker.Minsize + LevelMaker.Rand(this.Height - (2 * LevelMaker.Minsize));
                half = (int)Math.Max(half, LevelMaker.Minratio * this.Height);
                half = (int)Math.Min(half, LevelMaker.Maxratio * this.Height);
                this.LeftLeaf = new Partition(this.X, this.Y, this.Width, half, this);
                this.RightLeaf = new Partition(this.X, this.Y + half, this.Width, this.Height - half, this);
            }
            else
            { // left and right leaves
                if (this.Width <= 2 * LevelMaker.Minsize)
                {
                    return false;
                }

                var half = LevelMaker.Minsize + LevelMaker.Rand(this.Width - (2 * LevelMaker.Minsize));
                half = (int)Math.Max(half, LevelMaker.Minratio * this.Width);
                half = (int)Math.Min(half, LevelMaker.Maxratio * this.Width);
                this.LeftLeaf = new Partition(this.X, this.Y, half, this.Height, this);
                this.RightLeaf = new Partition(this.X + half, this.Y, this.Width - half, this.Height, this);
            }

            return true;
        }
        #endregion Methods
    }
}