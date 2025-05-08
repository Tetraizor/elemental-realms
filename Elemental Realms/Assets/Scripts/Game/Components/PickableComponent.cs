using Game.Interactions;
using Game.Items;
using DG.Tweening;
using UnityEngine;

namespace Game.Components
{
    public class PickableComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item _itemData;
        private SpriteRenderer _renderer;

        public Vector2 InteractablePosition => gameObject.transform.position;

        private void Start()
        {
            if (_itemData != null)
            {
                Setup(_itemData);
            }
        }

        public void Setup(Item itemData)
        {
            _itemData = itemData;
            _renderer = GetComponentInChildren<SpriteRenderer>();

            GetComponentInChildren<SpriteRenderer>().sprite = _itemData.Sprite;

            ToggleSelection(false);
        }

        public void Pickup()
        {
            _renderer.transform.parent = null;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_renderer.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack));
            sequence.Append(_renderer.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));

            Destroy(gameObject);
        }

        public void ToggleSelection(bool state)
        {
            if (state)
            {
                var material = _renderer.material;
                material.SetFloat("_ShowOutline", 1);
            }
            else
            {
                var material = _renderer.material;
                material.SetFloat("_ShowOutline", 0);
            }
        }

        public void Interact()
        {
            Pickup();
        }

        public void Activate()
        {
            ToggleSelection(true);
        }

        public void Deactivate()
        {
            ToggleSelection(false);
        }
    }
}
