namespace GameOne.Source.Interfaces
{
    public interface ICollidable : IPhysicsBody
    {
        bool IsSolid { get; }
        Enumerations.CollisionResponse Response { get; }
        Enumerations.Shape CollisionShape { get; }
    }
}
