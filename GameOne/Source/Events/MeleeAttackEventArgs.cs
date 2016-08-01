namespace GameOne.Source.Events
{
    using Interfaces;
    using System;

    public class MeleeAttackEventArgs : EventArgs
    {
        public MeleeAttackEventArgs(ICharacter source)
        {
            this.Source = source;
        }

        public ICharacter Source { get; }
    }
}
