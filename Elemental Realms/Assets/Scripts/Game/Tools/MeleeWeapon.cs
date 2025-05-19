using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Entities.Common;
using Game.Enum;
using Game.Interactions;
using Game.Items;
using Game.Modifiers;
using Game.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Tools
{
    [RequireComponent(typeof(Animator))]
    public class MeleeWeapon : MonoBehaviour, IInteractionSource, IInteractorFieldParent, ISpeedModifier, IItemThrowable, IToolInitializable
    {
        [Header("Weapon Properties")]
        [SerializeField] private EntityTag _targetTags = EntityTag.Enemy;
        [SerializeField] private float _cooldown = .5f;
        [SerializeField] private bool _throwable = true;
        [SerializeField] private string[] _animationTriggerNames = new string[] { "Attack1" };

        // State Properties
        private bool _isPrimaryActive = false;
        private bool _isSecondaryActive = false;

        private float _useSpeedPenalty = .5f;
        private float _moveSpeedPenalty = .2f;

        private float _cooldownTimer = 0;

        // References
        private GameObject _user;
        private ItemInstance _itemInstance;
        private List<InteractorField> _interactorFields;

        // Events
        [HideInInspector] public UnityEvent<GameObject, InteractionContext> Hit;
        [HideInInspector] public UnityEvent<ItemInstance> WeaponThrown;
        public UnityEvent<ItemInstance> GetThrownEvent() => WeaponThrown;

        private void Awake()
        {
            _interactorFields = GetComponentsInChildren<InteractorField>().ToList();
            _interactorFields.ForEach(field => field.Setup(this));
        }

        private void Update()
        {
            if (_isPrimaryActive && !_isSecondaryActive)
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

        public void Activate(int actionCode = 0)
        {
            if (actionCode == 0)
            {
                if (!_isSecondaryActive)
                    _isPrimaryActive = true;
                else Throw(_itemInstance);
            }
            else if (_throwable)
            {
                _isSecondaryActive = true;
                _isPrimaryActive = false;

                StartLiftingWeapon();
            }
        }

        public void Deactivate(int actionCode = 0)
        {
            if (actionCode == 0)
            {
                _isPrimaryActive = false;
            }
            else if (_throwable)
            {
                _isSecondaryActive = false;

                StopLiftingWeapon();
            }
        }

        public void Throw(ItemInstance thrownItem)
        {
            WeaponThrown.Invoke(thrownItem);
        }

        public void StartLiftingWeapon()
        {
            GetComponent<Animator>().SetTrigger("StartLifting");
        }

        public void StopLiftingWeapon()
        {
            GetComponent<Animator>().SetTrigger("StopLifting");
        }

        public void OnHitStarted(Collider2D collider)
        {
            if (collider.gameObject == _user) return;
            if (!collider.gameObject.TryGetComponent(out Entity entity)) return;
            if (!_targetTags.HasCommon(entity.Tags)) return;

            var ctx = new InteractionContext
            {
                Source = _user,
                HitDirection = (collider.gameObject.transform.position - _user.transform.position).normalized,
                HitPoint = collider.gameObject.GetComponent<Collider2D>().ClosestPoint(_user.transform.position)
            };

            if (_itemInstance.Item is IInteractionEffectProvider effectProvider)
            {
                effectProvider.GetAttackEffects().ForEach(effect => effect.ApplyEffect(
                    collider.gameObject,
                    ctx
                ));
            }

            if (_itemInstance.Item is IStatusEffectProvider statusProvider)
            {
                if (entity != null)
                {
                    statusProvider.GetStatusEffects().ForEach(status => entity.StatusManager.AddStatus(new Status.StatusInstance { Status = status }));
                }
            }

            _itemInstance.Durability -= 1;

            Hit?.Invoke(collider.gameObject, ctx);
        }

        public void OnHitStayed(Collider2D collider) { }

        public void OnHitEnded(Collider2D collider) { }

        public float GetSpeedModifier() => _isPrimaryActive || _isSecondaryActive ? (1 - _useSpeedPenalty) : (1 - _moveSpeedPenalty);

        private void OnDestroy()
        {
            Hit.RemoveAllListeners();
        }
    }
}