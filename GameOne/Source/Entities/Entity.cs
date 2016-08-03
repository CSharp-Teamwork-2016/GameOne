namespace GameOne.Source.Entities
{
    using Enumerations;
    using Interfaces;
    using System;

    // Base class for all non-geometry (non-Tile, see Level) objects
    [Serializable]
    public abstract class Entity : IRemovable
    {
        private static ulong nextId;
        protected State state;

        protected Entity()
        {
            this.Id = nextId++;
        }

        public ulong Id { get; }
        
        public bool Alive => !this.state.HasFlag(State.DEAD);

        public virtual void Die()
        {
            this.state = State.DEAD;
        }
    }
}
