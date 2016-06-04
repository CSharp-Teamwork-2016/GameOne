namespace GameOne.Source.Entities
{
	/// Base class for all non-geometry (non-Tile, see Level) objects
	public abstract class Entity
    {
		private static ulong nextId = 0;
		private ulong id;

        protected Entity()
        {
			id = nextId++;
        }

		public ulong ID { get { return id; } }

        public abstract void Update();
    }
}
