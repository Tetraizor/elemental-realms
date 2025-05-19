using Game.Effects;
using Game.Interactions.Effects;
using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class ConsumeEffectInstance
    {
        public ConsumeEffectSO ConsumeEffect;
        public int Magnitude;

        public void GetConsumed(GameObject target)
        {
            switch (ConsumeEffect.Type)
            {
                case Enum.ConsumeEffectType.Healing:
                    new HealingConsumeEffect().GetConsumed(target, this);
                    break;
                case Enum.ConsumeEffectType.Harming:
                    break;
            }
        }
    }
}