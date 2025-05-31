using System;

namespace Game.Enum
{
    [Flags]
    public enum StatusEffectType
    {
        None = 0,
        Wet = 1 << 0,
        Frozen = 1 << 1,
        Poisoned = 1 << 2,
        OnFire = 1 << 3,

        All = ~0
    }
}