using System.Collections.Generic;
using Game.Interactions.Effects;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Items/Projectile Item", order = 0)]
    public class ProjectileItem : Item, IInteractionEffectProvider
    {
        [SerializeField] private List<AttackEffect> _effects;

        public List<AttackEffect> GetAttackEffects() => _effects;
    }
}