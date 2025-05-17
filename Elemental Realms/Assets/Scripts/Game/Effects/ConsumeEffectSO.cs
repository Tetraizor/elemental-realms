using Game.Enum;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "Consume Effect", menuName = "Effects/Consume Effect", order = 0)]
    public class ConsumeEffectSO : ScriptableObject
    {
        public ConsumeEffectType Type;
        public string Name;
        public Sprite Icon;
    }
}