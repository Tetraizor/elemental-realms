using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Interactions;
using Game.Interactions.Effects;
using Game.Modifiers;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Tools
{
    [RequireComponent(typeof(Animator))]
    public class GenericMeleeWeapon : MonoBehaviour, IInteractionSource, IInteractorFieldParent, ISpeedModifier
    {
        public List<InteractionEffectSO> Effects => _effects;
        [SerializeField] private List<InteractionEffectSO> _effects;

        private GameObject _user;
        private List<InteractorField> _interactorFields;

        private bool _isActive = false;

        [SerializeField] private string[] _animationTriggerNames = new string[] { "Attack1" };
        [SerializeField] private float _cooldown = .5f;
        private float _cooldownTimer = 0;

        public UnityEvent<GameObject, InteractionContext> Hit;

        private void Awake()
        {
            _interactorFields = GetComponentsInChildren<InteractorField>().ToList();
            _interactorFields.ForEach(field => field.Setup(this));
        }

        private void Update()
        {
            if (_isActive)
            {
                if (_cooldownTimer <= 0)
                {
                    _cooldownTimer = _cooldown;

                    string triggerName = _animationTriggerNames[Random.Range(0, _animationTriggerNames.Length)];
                    GetComponent<Animator>().SetTrigger(triggerName);
                }
            }

            _cooldownTimer -= Time.deltaTime;
            _cooldownTimer = Mathf.Max(0, _cooldownTimer);
        }

        public void Setup(GameObject user)
        {
            _user = user;
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void OnHitStarted(Collider2D collider)
        {
            if (collider.gameObject != _user)
            {
                var ctx = new InteractionContext
                {
                    Source = _user,
                    HitDirection = (collider.gameObject.transform.position - _user.transform.position).normalized,
                    HitPoint = collider.gameObject.GetComponent<Collider2D>().ClosestPoint(_user.transform.position)
                };

                _effects.ForEach(effect => effect.ApplyEffect(
                    collider.gameObject,
                    ctx
                ));

                Hit?.Invoke(collider.gameObject, ctx);
            }
        }

        public void OnHitStayed(Collider2D collider) { }

        public void OnHitEnded(Collider2D collider) { }

        public float GetSpeedModifier() => _isActive ? .5f : 1;

        private void OnDestroy()
        {
            Hit.RemoveAllListeners();
        }
    }
}