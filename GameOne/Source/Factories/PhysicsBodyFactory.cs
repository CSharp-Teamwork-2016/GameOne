using GameOne.Source.Interfaces;
using GameOne.Source.World.Physics;

namespace GameOne.Source.Factories
{
    public static class PhysicsBodyFactory
    {
        public static IPhysicsBody MakeBody()
        {
            return new MovableBody();
        }
    }
}
