using DG.Tweening;
using Game.Controllers.UI;
using Game.Data;
using Game.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Inventories
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler
    {
        [Header("UI References")]
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectFrame;
        [SerializeField] private Image _equipBackground;
        [SerializeField] private TextMeshProUGUI _itemCount;

        [HideInInspector] public ItemInstance ItemInstance;
        [HideInInspector] public int Count;

        private InventoryUIController _inventoryUiController;

        public void Setup(InventoryUIController inventoryUiController)
        {
            _inventoryUiController = inventoryUiController;
        }

        public void SetItem(SlotData data)
        {
            SetItem(data.ItemInstance, data.Count);
        }

        public void SetItem(ItemInstance itemInstance, int count)
        {
            ItemInstance = itemInstance;
            Count = count;
            _image.sprite = itemInstance?.Item?.Sprite;

            if (_image.sprite == null) _image.color = new Color(1, 1, 1, 0);
            else _image.color = new Color(1, 1, 1, 1);

            _itemCount.SetText(Count > 1 ? Count.ToString() : "");
        }

        public void Select()
        {
            _selectFrame.enabled = true;
        }

        public void Deselect()
        {
            _selectFrame.enabled = false;
        }

        public void EquipItem()
        {
            _equipBackground.DOFade(1, .1f);
            Debug.Log("Equipped Slot " + transform.GetSiblingIndex());
        }

        public void UnequipItem()
        {
            _equipBackground.DOFade(0, .1f);
            Debug.Log("Unequipped Slot " + transform.GetSiblingIndex());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryUiController.SelectSlot(this);
        }
    }
}