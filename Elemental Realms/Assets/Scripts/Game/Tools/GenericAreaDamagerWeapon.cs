using System.Collections.Generic;
using Game.Data;
using Game.Interactions;
using Game.Interactions.Effects;
using UnityEngine;

namespace Game.Tools
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(InteractorField))]
    public class GenericAreaDamagerWeapon : MonoBehaviour, IInteractionSource, IInteractorFieldParent
    {
        public List<InteractionEffectSO> Effects => _effects;
        [SerializeField] private List<InteractionEffectSO> _effects = new();
        [SerializeField] private float _radius;

        private CircleCollider2D _collider;
        private GameObject _user;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.radius = _radius;
            _collider.enabled = false;
            _collider.GetComponent<InteractorField>().Setup(this);
        }

        public void Setup(GameObject user)
        {
            _user = user;
        }

        public void Activate()
        {
            _collider.enabled = true;
        }

        public void Deactivate()
        {
            _collider.enabled = false;
        }

        public void OnHitStarted(Collider2D collider)
        {
            if (collider.gameObject == gameObject) return;

            var ctx = new InteractionContext
            {
                Source = _user,
                HitDirection = (collider.gameObject.transform.position - _user.transform.position).normalized,
                HitPoint = collider.gameObject.GetComponent<Collider2D>().ClosestPoint(_user.transform.position)
            };

            Effects.ForEach(effect => effect.ApplyEffect(collider.gameObject, ctx));
        }

        public void OnHitStayed(Collider2D collider) { }

        public void OnHitEnded(Collider2D collider) { }
    }
}