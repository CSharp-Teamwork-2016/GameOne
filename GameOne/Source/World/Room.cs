namespace GameOne.Source.World
{
    using System;

    public class Room
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private int originX;
        private int originY;

        public Room(int x, int y, int width, int height)
        {
            this.width = LevelMaker.MINSIZE + LevelMaker.rand(LevelMaker.MAXSIZE - LevelMaker.MINSIZE);
            this.width = Math.Min(this.width, width);
            this.height = LevelMaker.MINSIZE + LevelMaker.rand(LevelMaker.MAXSIZE - LevelMaker.MINSIZE);
            this.height = Math.Min(this.height, height);

            if (width - this.width > 0)
            {
                this.x = x + LevelMaker.rand(width - this.width);
            }
            else
            {
                this.x = x;
            }

            if (height - this.height > 0)
            {
                this.y = y + LevelMaker.rand(height - this.height);
            }
            else
            {
                this.y = y;
            }

            this.originX = this.x + this.width / 2;
            this.originY = this.y + this.height / 2;
        }

        //========================================

        //region Properties

        public int X => this.x;

        public int Y => this.y;

        public int Width => this.width;

        public int Height => this.height;

        public int OriginX => this.originX;

        public int OriginY => this.originY;

        //endregion Properties ===================

        //========================================
    }
}