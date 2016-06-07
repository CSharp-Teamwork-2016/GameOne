using System;
using GameOne.Source.Level;

namespace GameOne.Source.World
{
    public class Hallway
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public Hallway(Room room, Partition parent, bool horizontal)
        {
            var originX = room.OriginX;
            var originY = room.OriginY;
            var targetX = parent.X + parent.Width / 2;
            var targetY = parent.Y + parent.Height / 2;

            if (!horizontal)
            {
                x = Math.Min(originX, targetX) - LevelMaker.HALLSIZE / 2;
                y = originY - LevelMaker.HALLSIZE / 2;
                width = Math.Abs(originX - targetX) + LevelMaker.HALLSIZE;
                height = LevelMaker.HALLSIZE;
            }
            else
            { // vertical
                x = originX - LevelMaker.HALLSIZE / 2;
                y = Math.Min(originY, targetY) - LevelMaker.HALLSIZE / 2;
                width = LevelMaker.HALLSIZE;
                height = Math.Abs(originY - targetY) + LevelMaker.HALLSIZE;
            }
        }

        public Hallway(Partition leftLeaf, Partition rightLeaf, bool horizontal)
        {
            int originX;
            int originY;
            int targetX;
            int targetY;

            if (horizontal)
            {
                if (leftLeaf.Room != null)
                {
                    originX = leftLeaf.Room.X + leftLeaf.Room.Width / 2;
                }
                else
                {
                    originX = leftLeaf.X + leftLeaf.Width / 2;
                }

                if (rightLeaf.Room != null)
                {
                    targetX = rightLeaf.Room.X + rightLeaf.Room.Width / 2;
                }
                else
                {
                    targetX = rightLeaf.X + rightLeaf.Width / 2;
                }

                originY = leftLeaf.Y + leftLeaf.Height / 2;
                x = originX - LevelMaker.HALLSIZE / 2;
                y = originY - LevelMaker.HALLSIZE / 2;
                width = targetX - originX + LevelMaker.HALLSIZE;
                height = LevelMaker.HALLSIZE;
            }
            else
            { // vertical
                if (leftLeaf.Room != null)
                {
                    originY = leftLeaf.Room.Y + leftLeaf.Room.Height / 2;
                }
                else
                {
                    originY = leftLeaf.Y + leftLeaf.Height / 2;
                }
                if (rightLeaf.Room != null)
                {
                    targetY = rightLeaf.Room.Y + rightLeaf.Room.Height / 2;
                }
                else
                {
                    targetY = rightLeaf.Y + rightLeaf.Height / 2;
                }
                originX = leftLeaf.X + leftLeaf.Width / 2;
                x = originX - LevelMaker.HALLSIZE / 2;
                y = originY - LevelMaker.HALLSIZE / 2;
                width = LevelMaker.HALLSIZE;
                height = targetY - originY + LevelMaker.HALLSIZE;
            }
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

        //endregion Properties ==============================
    }
}

