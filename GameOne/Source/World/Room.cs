using System;
using GameOne.Source.Level;

namespace GameOne.Source.World
{
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
                this.x = x + LevelMaker.rand(width - this.width);
            else
                this.x = x;
            if (height - this.height > 0)
                this.y = y + LevelMaker.rand(height - this.height);
            else
                this.y = y;

            originX = this.x + this.width / 2;
            originY = this.y + this.height / 2;
        }

        //========================================

        //region Properties

        public int X
        {
            get { return x; }
            set
            {

            }
        }

        public int Y
        {
            get { return y; }
            set
            {

            }
        }

        public int Width
        {
            get { return width; }
            set
            {

            }
        }

        public int Height
        {
            get { return height; }
            set
            {

            }
        }

        public int OriginX
        {
            get { return originX; }
            set
            {

            }
        }

        public int OriginY
        {
            get { return originY; }
            set
            {

            }
        }

        //endregion Properties ===================

        //========================================
    }
}

