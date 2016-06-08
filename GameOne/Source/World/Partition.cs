namespace GameOne.Source.World
{
    using System;

    public class Partition
    {
        private Partition parent;
        private Partition leftLeaf;
        private Partition rightLeaf;

        private int x;
        private int y;
        private int width;
        private int height;
        private bool direction; // false = horizontal split; true = vertical split

        private Room room;
        private Hallway hallway;

        public Partition(int x, int y, int width, int height, Partition parent)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.parent = parent;
        }

        //========================================

        //region Properties ======================

        public int X
        {
            get
            {
                return x;
            }
            set
            {
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
            }
        }

        public bool IsHorizontal
        {
            get
            {
                return direction;
            }
            set
            {
            }
        } //not sure

        public Partition LeftLeaf
        {
            get
            {
                return leftLeaf;
            }
            set
            {
            }
        }

        public Partition RightLeaf
        {
            get
            {
                return rightLeaf;
            }
            set
            {
            }
        }

        public Room Room
        {
            get
            {
                return room;
            }
            set
            {
            }
        }

        public Hallway Hallway
        {
            get
            {
                return hallway;
            }
            set
            {
            }
        }

        public bool HasLeaves
        {
            get
            {
                return leftLeaf != null;
            }
            set
            {
            }
        }  //not sure

        //endregion Properties ===================

        //========================================

        //region Generation ======================

        public bool TrySplit()
        {
            if (HasLeaves) //not sure
            {
                return leftLeaf.TrySplit() || rightLeaf.TrySplit();
            }
            else
            {
                return Split();
            }
        }

        private bool Split()
        {
            direction = false; //false = horizontal, true = vertical

            if ((double)width / height > LevelMaker.MAXRATIO)
            {
                direction = true;
            }
            else if ((double)height / width > LevelMaker.MAXRATIO)
            {
                direction = false;
            }
            else
            {
                direction = LevelMaker.rand(1) == 1; // 0 = horizontal, 1 = vertical
            }

            if (!direction)
            { // top and bottom leaves
                if (height <= 2 * LevelMaker.MINSIZE) return false;
                var half = LevelMaker.MINSIZE + LevelMaker.rand(height - 2 * LevelMaker.MINSIZE);
                half = (int)Math.Max(half, LevelMaker.MINRATIO * height);
                half = (int)Math.Min(half, LevelMaker.MAXRATIO * height);
                leftLeaf = new Partition(x, y, width, half, this);
                rightLeaf = new Partition(x, y + half, width, height - half, this);
            }
            else
            { // left and right leaves
                if (width <= 2 * LevelMaker.MINSIZE) return false;
                var half = LevelMaker.MINSIZE + LevelMaker.rand(width - 2 * LevelMaker.MINSIZE);
                half = (int)Math.Max(half, LevelMaker.MINRATIO * width);
                half = (int)Math.Min(half, LevelMaker.MAXRATIO * width);
                leftLeaf = new Partition(x, y, half, height, this);
                rightLeaf = new Partition(x + half, y, width - half, height, this);
            }

            return true;
        }

        public void MakeRoom()
        {
            if (HasLeaves) //not sure
            {
                leftLeaf.MakeRoom();
                rightLeaf.MakeRoom();
            }
            else
            {
                room = new Room(x, y, width, height);
            }
        }

        public void MakeHallway()
        {
            if (!HasLeaves)
            { // Connect room to parent partition origin
                hallway = new Hallway(room, parent, parent.IsHorizontal);
            }
            else
            { // Connect leaf partitions to each other
                leftLeaf.MakeHallway();
                rightLeaf.MakeHallway();
                hallway = new Hallway(leftLeaf, rightLeaf, direction);
            }
        }

        //endregion Generation ===================

        //========================================
    }
}