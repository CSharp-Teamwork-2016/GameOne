namespace GameOne.Source.World.Physics
{
    using Enumerations;
    using Interfaces;

    public class RigidBody : Body, ICollidable
    {
        public bool IsSolid { get; set; }

        public CollisionResponse CollisionResponse { get; set; }

        public bool Alive => true;

        public Shape CollisionShape { get; set; }

        public void Respond(ICollidable model)
        {

        }

        public void Die()
        {

        }
    }
}
