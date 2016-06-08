namespace GameOne.Source.Entities
{
    // Base class for all non-geometry (non-Tile, see Level) objects
    public abstract class Entity
    {
        private static ulong nextId = 0;
        private ulong id;

        protected Entity()
        {
            this.id = nextId++;
        }

        public ulong Id => this.id;

        public abstract void Update(double time);
    }
}
