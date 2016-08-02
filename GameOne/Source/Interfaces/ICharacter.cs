using System.Windows;

namespace GameOne.Source.Interfaces
{
    public interface ICharacter
    {
        Vector Position { get; }
        int Damage { get; }
        double Direction { get; }
        void TakeDamage(int damage);
    }
}
