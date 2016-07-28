namespace GameOne.Source.World
{
    using System;

    public class Hallway
    {
        #region Constructors

        public Hallway(Room room, Partition parent, bool horizontal)
        {
            var originX = room.OriginX;
            var originY = room.OriginY;
            var targetX = parent.X + (parent.Width / 2);
            var targetY = parent.Y + (parent.Height / 2);

            if (!horizontal)
            {
                this.X = Math.Min(originX, targetX) - (LevelMaker.Hallsize / 2);
                this.Y = originY - (LevelMaker.Hallsize / 2);
                this.Width = Math.Abs(originX - targetX) + LevelMaker.Hallsize;
                this.Height = LevelMaker.Hallsize;
            }
            else
            { // vertical
                this.X = originX - (LevelMaker.Hallsize / 2);
                this.Y = Math.Min(originY, targetY) - (LevelMaker.Hallsize / 2);
                this.Width = LevelMaker.Hallsize;
                this.Height = Math.Abs(originY - targetY) + LevelMaker.Hallsize;
            }
        }

        public Hallway(Partition leftLeaf, Partition rightLeaf, bool horizontal)
        {
            int originX;
            int originY;

            if (horizontal)
            {
                if (leftLeaf.Room != null)
                {
                    originX = leftLeaf.Room.X + (leftLeaf.Room.Width / 2);
                }
                else
                {
                    originX = leftLeaf.X + (leftLeaf.Width / 2);
                }

                int targetX;
                if (rightLeaf.Room != null)
                {
                    targetX = rightLeaf.Room.X + (rightLeaf.Room.Width / 2);
                }
                else
                {
                    targetX = rightLeaf.X + (rightLeaf.Width / 2);
                }

                originY = leftLeaf.Y + (leftLeaf.Height / 2);
                this.X = originX - (LevelMaker.Hallsize / 2);
                this.Y = originY - (LevelMaker.Hallsize / 2);
                this.Width = targetX - originX + LevelMaker.Hallsize;
                this.Height = LevelMaker.Hallsize;
            }
            else
            { // vertical
                if (leftLeaf.Room != null)
                {
                    originY = leftLeaf.Room.Y + (leftLeaf.Room.Height / 2);
                }
                else
                {
                    originY = leftLeaf.Y + (leftLeaf.Height / 2);
                }

                int targetY;
                if (rightLeaf.Room != null)
                {
                    targetY = rightLeaf.Room.Y + (rightLeaf.Room.Height / 2);
                }
                else
                {
                    targetY = rightLeaf.Y + (rightLeaf.Height / 2);
                }

                originX = leftLeaf.X + (leftLeaf.Width / 2);
                this.X = originX - (LevelMaker.Hallsize / 2);
                this.Y = originY - (LevelMaker.Hallsize / 2);
                this.Width = LevelMaker.Hallsize;
                this.Height = targetY - originY + LevelMaker.Hallsize;
            }
        }

        #endregion Constructors

        #region Properties

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        #endregion Properties
    }
}