using Game.Enum;
using UnityEngine;

namespace Game.Tools
{
    [CreateAssetMenu(fileName = "ToolStats", menuName = "Tools/Tool Stats", order = 0)]
    public class ToolStatsSO : ScriptableObject
    {
        public float Cooldown;
        public float Damage;
        public DamageType DamageType;
    }
}