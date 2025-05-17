using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Interactions;
using Game.Interactions.Effects;
using Game.Items;
using Game.Modifiers;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Tools
{
    [RequireComponent(typeof(Animator))]
    public class GenericMeleeWeapon : MonoBehaviour, IInteractionSource, IInteractorFieldParent, ISpeedModifier
    {
        private GameObject _user;
        private ItemInstance _itemInstance;
        private List<InteractorField> _interactorFields;

        private bool _isActive = false;

        private float _useSpeedPenalty = .5f;
        private float _moveSpeedPenalty = .2f;

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

        public void Setup(GameObject user, ItemInstance itemInstance)
        {
            _user = user;
            _itemInstance = itemInstance;

            _useSpeedPenalty = (_itemInstance.Item as ToolItem).UseSpeedPenalty;
            _moveSpeedPenalty = (_itemInstance.Item as ToolItem).MovementSpeedPenalty;
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

                (_itemInstance.Item as ToolItem).AttackEffects.ForEach(effect => effect.ApplyEffect(
                    collider.gameObject,
                    ctx
                ));

                _itemInstance.Durability -= 1;

                Hit?.Invoke(collider.gameObject, ctx);
            }
        }

        public void OnHitStayed(Collider2D collider) { }

        public void OnHitEnded(Collider2D collider) { }

        public float GetSpeedModifier() => _isActive ? (1 - _useSpeedPenalty) : (1 - _moveSpeedPenalty);

        private void OnDestroy()
        {
            Hit.RemoveAllListeners();
        }
    }
}