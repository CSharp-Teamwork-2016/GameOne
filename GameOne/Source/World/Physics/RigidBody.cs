namespace GameOne.Source.World.Physics
{
    using Enumerations;
    using Interfaces;

    public class RigidBody : Body, ICollidable
    {
        private bool isSolid;
        private readonly CollisionResponse collisionResponse;
        private readonly Shape collisionShape;

        public bool IsSolid
        {
            get
            {
                return this.isSolid;
            }
            set
            {
                this.isSolid = value;
            }
        }

        public CollisionResponse Response
        {
            get
            {
                return this.collisionResponse;
            }
        }

        public Shape CollisionShape
        {
            get
            {
                return this.collisionShape;
            }
        }
    }
}
