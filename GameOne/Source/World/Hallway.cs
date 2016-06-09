namespace GameOne.Source.World
{
    using System;

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
                this.x = Math.Min(originX, targetX) - LevelMaker.HALLSIZE / 2;
                this.y = originY - LevelMaker.HALLSIZE / 2;
                this.width = Math.Abs(originX - targetX) + LevelMaker.HALLSIZE;
                this.height = LevelMaker.HALLSIZE;
            }
            else
            { // vertical
                this.x = originX - LevelMaker.HALLSIZE / 2;
                this.y = Math.Min(originY, targetY) - LevelMaker.HALLSIZE / 2;
                this.width = LevelMaker.HALLSIZE;
                this.height = Math.Abs(originY - targetY) + LevelMaker.HALLSIZE;
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
                this.x = originX - LevelMaker.HALLSIZE / 2;
                this.y = originY - LevelMaker.HALLSIZE / 2;
                this.width = targetX - originX + LevelMaker.HALLSIZE;
                this.height = LevelMaker.HALLSIZE;
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
                this.x = originX - LevelMaker.HALLSIZE / 2;
                this.y = originY - LevelMaker.HALLSIZE / 2;
                this.width = LevelMaker.HALLSIZE;
                this.height = targetY - originY + LevelMaker.HALLSIZE;
            }
        }

        //========================================
        //region Properties

        public int X => this.x;

        public int Y => this.y;

        public int Width => this.width;

        public int Height => this.height;

        //endregion Properties ==============================
    }
}

