namespace GameOne.Source.Interfaces
{
    public interface IRemovable
    {
        bool Alive { get; }
        void Die();
    }
}
