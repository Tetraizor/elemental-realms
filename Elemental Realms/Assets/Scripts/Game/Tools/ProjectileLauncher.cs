using Game.Components;
using Game.Data;
using Game.Entities.Common;
using Game.Enum;
using Game.Interactions;
using Game.Inventories;
using Game.Items;
using Game.Modifiers;
using UnityEngine;

namespace Game.Tools
{
    [RequireComponent(typeof(Animator))]
    public class ProjectileLauncher : MonoBehaviour, IInteractionSource, ISpeedModifier, IToolInitializable
    {
        [Header("Weapon Properties")]
        [SerializeField] private float _drawTime;
        [SerializeField] private float _currentDrawTime = 0;
        [SerializeField] private float _swayAngles = 8;

        // References
        private ItemInstance _itemInstance;

        // Weapon State
        private bool _isActive = false;

        private Entity _user;

        private float _useSpeedPenalty = .5f;
        private float _moveSpeedPenalty = .2f;

        public void Setup(GameObject user, ItemInstance itemInstance)
        {
            _itemInstance = itemInstance;

            _useSpeedPenalty = (_itemInstance.Item as ToolItem).UseSpeedPenalty;
            _moveSpeedPenalty = (_itemInstance.Item as ToolItem).MovementSpeedPenalty;

            _user = user.GetComponent<Entity>();
        }

        private void Update()
        {
            if (_isActive)
            {
                _currentDrawTime += Time.deltaTime;
            }
        }

        private ItemInstance GetFirstSuitableAmmo() => InventoryController.Instance.HasItemWithType(InventoryType.MaterialInventory, ItemType.Arrow);

        private void ShootProjectile(ItemInstance itemInstance)
        {
            GetComponent<Animator>().SetTrigger("Shoot");

            if (InventoryController.Instance.RemoveFirstItemByInstance(InventoryType.MaterialInventory, itemInstance))
            {
                var direction = Vector2.right;
                if (_user.TryGetComponent(out MoveableComponent moveable))
                {
                    direction = moveable.LookDirection;
                }

                var position = _user.transform.position + (Vector3)(direction * 2f);

                var spawnedObject = ItemSpawnerController.Instance.SpawnPickable(itemInstance, position);
                float hitAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + Random.Range(_swayAngles * -1, _swayAngles);
                var swayedLookVector = new Vector2(Mathf.Cos(hitAngle * Mathf.Deg2Rad), Mathf.Sin(hitAngle * Mathf.Deg2Rad));

                spawnedObject.transform.localEulerAngles = new Vector3(0, 0, hitAngle);

                if (spawnedObject.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(swayedLookVector * 60 * rb.mass, ForceMode2D.Impulse);
                }

                if (spawnedObject.TryGetComponent(out PhysicsInteractorPickableComponent pickable))
                {
                    pickable.InitializeWithItem(itemInstance);
                }
            }
        }

        public void Activate(int actionCode = 0)
        {
            if (actionCode == 0)
            {
                if (_isActive) return;

                var ammo = GetFirstSuitableAmmo();
                if (ammo != null)
                {
                    _isActive = true;

                    GetComponent<Animator>().SetTrigger("TakeOut");
                }
            }
            else
            {
                if (_isActive)
                {
                    GetComponent<Animator>().SetTrigger("PutBack");
                    _isActive = false;
                }
            }
        }

        public void Deactivate(int actionCode = 0)
        {
            if (!_isActive) return;

            if (actionCode == 0)
            {
                _isActive = false;

                if (_currentDrawTime >= _drawTime)
                {
                    ShootProjectile(GetFirstSuitableAmmo());
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("PutBack");
                }

                _currentDrawTime = 0;
            }
        }

        public float GetSpeedModifier() => _isActive ? (1 - _useSpeedPenalty) : (1 - _moveSpeedPenalty);
    }
}