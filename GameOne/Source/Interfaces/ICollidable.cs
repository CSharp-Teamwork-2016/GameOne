namespace GameOne.Source.Interfaces
{
    public interface ICollidable : IPhysicsBody, IRemovable
    {
        bool IsSolid { get; }
        Enumerations.CollisionResponse CollisionResponse { get; }
        Enumerations.Shape CollisionShape { get; }
        void Respond(ICollidable model);
    }
}
