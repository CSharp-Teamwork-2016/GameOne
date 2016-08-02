namespace GameOne.Source.Events
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
