namespace GameOne.Source.Enumerations
{
    [System.Flags]
    public enum State
    {
        IDLE = 0x00,
        ATTACK = 0x02,
        HURT = 0x04,
        DEAD = 0x08
    }
}