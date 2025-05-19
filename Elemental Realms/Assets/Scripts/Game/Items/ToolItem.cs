using System.Collections.Generic;
using Game.Interactions.Effects;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Items/Tool Item", order = 0)]
    public class ToolItem : Item, IInteractionEffectProvider
    {
        [Header("Tool Properties")]
        public int MaxDurability = 20;
        public float MovementSpeedPenalty = 0;
        public float UseSpeedPenalty = 0;
        public GameObject InteractorPrefab;

        [SerializeField] private List<AttackEffect> _attackEffects;

        public List<AttackEffect> GetEffects() => _attackEffects;
    }
}