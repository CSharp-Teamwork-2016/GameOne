using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOne.Source.World
{
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

        //=====================================================
        //region Properties

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

        //not sure
        public bool IsHorizontal
        {
            get
            {
                return direction;
            }
            set
            {
            }
        }

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

        //not sure
        public bool HasLeaves
        {
            get
            {
                return leftLeaf != null;
            }
            set
            {
            }
        }

        //end region ============================

        //========================================================

        //region Generation

        public bool trySplit()
        {
            //not sure
            if (HasLeaves)
            {
                return leftLeaf.trySplit() || rightLeaf.trySplit();
            }
            else
            {
                return split();
            }
        }

        //TODO
        //private bool split()
        //{
        //    direction = false; // false = horizontal, true = vertical
        //    if (width / height > LevelMaker.Max)
        //    {

        //    }


        //}



    }
}
