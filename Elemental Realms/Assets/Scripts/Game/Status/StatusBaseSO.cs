using Game.Enum;
using UnityEngine;

namespace Game.Status
{
    public abstract class StatusBaseSO : ScriptableObject
    {
        public string Name = "Status Name";
        public Sprite Icon;

        public StatusEffectType Type;

        public abstract void Inflict(StatusManager target, StatusInstance statusInstance);
        public abstract void Finish(StatusManager target, StatusInstance statusInstance);
        public abstract void Reapply(StatusManager target, StatusInstance statusInstance);
        public abstract void Tick(float deltaTime, StatusManager target, StatusInstance statusInstance);
    }

    public abstract class StatusData { }
}