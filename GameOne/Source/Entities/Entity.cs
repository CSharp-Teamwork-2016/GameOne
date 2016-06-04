namespace GameOne.Source.Entities
{
    public abstract class Entity
    {
        // Base class for all non-geometry (non-Tile, see Level) objects
        // All objects have location fields
        // Interfaces to be implemented by child-classes IMovable, IRenderable

        protected Entity(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }

        public abstract void Update();
    }
}
