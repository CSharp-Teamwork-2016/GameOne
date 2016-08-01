namespace GameOne.Source.Entities.Zones
{
    using Microsoft.Xna.Framework;
    using Interfaces;
    using Enumerations;

    public class DamageZone : Zone, IUpdatable
    {
        private double lifeTime;

        public DamageZone(double x, double y, double width, double height, ICharacter source, double lifeTime)
            : base(x, y, width, height)
        {
            this.Source = source;
            this.lifeTime = lifeTime;
        }

        public ICharacter Source { get; }

        public void Update(double time)
        {
            this.lifeTime -= time;

            if (this.lifeTime <= 0)
            {
                this.state = State.DEAD;
            }
        }
    }
}
