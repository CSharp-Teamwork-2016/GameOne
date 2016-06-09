namespace GameOne.Source.Interfaces
{
    public interface IMovable
    {
        void Forward(double time);

        void Accelerate(double time);
    }
}
