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

        public int X => this.x;

        public int Y => this.y;

        public int Width => this.width;

        public int Height => this.height;

        public bool IsHorizontal => this.direction;

        //not sure

        public Partition LeftLeaf => this.leftLeaf;

        public Partition RightLeaf => this.rightLeaf;

        public Room Room => this.room;

        public Hallway Hallway => this.hallway;

        public bool HasLeaves => this.leftLeaf != null;

        //not sure

        //endregion Properties ===================

        //========================================

        //region Generation ======================

        public bool TrySplit()
        {
            if (this.HasLeaves) //not sure
            {
                return this.leftLeaf.TrySplit() || this.rightLeaf.TrySplit();
            }
            else
            {
                return this.Split();
            }
        }

        private bool Split()
        {
            this.direction = false; //false = horizontal, true = vertical

            if ((double)this.width / this.height > LevelMaker.MAXRATIO)
            {
                this.direction = true;
            }
            else if ((double)this.height / this.width > LevelMaker.MAXRATIO)
            {
                this.direction = false;
            }
            else
            {
                this.direction = LevelMaker.Rand(1) == 1; // 0 = horizontal, 1 = vertical
            }

            if (!this.direction)
            { // top and bottom leaves
                if (this.height <= 2 * LevelMaker.MINSIZE) return false;
                var half = LevelMaker.MINSIZE + LevelMaker.Rand(this.height - 2 * LevelMaker.MINSIZE);
                half = (int)Math.Max(half, LevelMaker.MINRATIO * this.height);
                half = (int)Math.Min(half, LevelMaker.MAXRATIO * this.height);
                this.leftLeaf = new Partition(this.x, this.y, this.width, half, this);
                this.rightLeaf = new Partition(this.x, this.y + half, this.width, this.height - half, this);
            }
            else
            { // left and right leaves
                if (this.width <= 2 * LevelMaker.MINSIZE) return false;
                var half = LevelMaker.MINSIZE + LevelMaker.Rand(this.width - 2 * LevelMaker.MINSIZE);
                half = (int)Math.Max(half, LevelMaker.MINRATIO * this.width);
                half = (int)Math.Min(half, LevelMaker.MAXRATIO * this.width);
                this.leftLeaf = new Partition(this.x, this.y, half, this.height, this);
                this.rightLeaf = new Partition(this.x + half, this.y, this.width - half, this.height, this);
            }

            return true;
        }

        public void MakeRoom()
        {
            if (this.HasLeaves) //not sure
            {
                this.leftLeaf.MakeRoom();
                this.rightLeaf.MakeRoom();
            }
            else
            {
                this.room = new Room(this.x, this.y, this.width, this.height);
            }
        }

        public void MakeHallway()
        {
            if (!this.HasLeaves)
            { // Connect room to parent partition origin
                this.hallway = new Hallway(this.room, this.parent, this.parent.IsHorizontal);
            }
            else
            { // Connect leaf partitions to each other
                this.leftLeaf.MakeHallway();
                this.rightLeaf.MakeHallway();
                this.hallway = new Hallway(this.leftLeaf, this.rightLeaf, this.direction);
            }
        }

        //endregion Generation ===================

        //========================================
    }
}