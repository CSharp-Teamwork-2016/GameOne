namespace GameOne.Source.EventArgs
{
    using System;
    using Interfaces;

    public class MeleeAttackEventArgs : EventArgs
    {
        public MeleeAttackEventArgs(ICharacter source)
        {
            this.Source = source;
        }

        public ICharacter Source { get; }
    }
}
