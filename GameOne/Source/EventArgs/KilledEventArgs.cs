namespace GameOne.Source.EventArgs
{
    using System;

    public class KilledEventArgs : EventArgs
    {
        public KilledEventArgs(int xpAward)
        {
            this.XpAward = xpAward;
        }

        public int XpAward { get; } 
    }
}
