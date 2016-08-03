namespace GameOne.Source.World.Physics
{
    using Enumerations;
    using Interfaces;

    public class RigidBody : Body, ICollidable
    {
        public bool IsSolid { get; set; }

        public CollisionResponse Response { get; set; }

        public Shape CollisionShape { get; set; }
    }
}
