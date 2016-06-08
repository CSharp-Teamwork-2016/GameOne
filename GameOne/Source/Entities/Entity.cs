namespace GameOne.Source.Entities
{
    // Base class for all non-geometry (non-Tile, see Level) objects
    public abstract class Entity
    {
        private ulong id;

        protected Entity()
        {
            this.id = 0;
        }

        public ulong Id => this.id++;

        public abstract void Update(double time);
    }
}
