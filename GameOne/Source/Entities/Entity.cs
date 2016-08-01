namespace GameOne.Source.Entities
{
    // Base class for all non-geometry (non-Tile, see Level) objects
    public abstract class Entity
    {
        private static ulong nextId;

        protected Entity()
        {
            this.Id = nextId++;
        }

        public ulong Id { get; }
    }
}
