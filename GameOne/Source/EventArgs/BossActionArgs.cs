namespace GameOne.Source.EventArgs
{
    using System;
    using Enumerations;

    public class BossActionArgs : EventArgs
    {
        public BossActionArgs(BossAction action)
        {
            this.Action = action;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public BossAction Action { get; }
    }
}
