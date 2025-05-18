namespace Game.Enum
{
    [System.Flags]
    public enum EntityTag
    {
        None = 0,

        Player = 1 << 0,
        Pickable = 1 << 1,

        // Enemies
        Enemy = 1 << 16,
        Slime = 1 << 17,

        All = ~0
    }
}