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
        [SerializeField] private TextMeshProUGUI _itemCount;

        public Item Item;
        public int Count;

        private InventoryUIController _inventoryUiController;

        public void Setup(InventoryUIController inventoryUiController)
        {
            _inventoryUiController = inventoryUiController;
        }

        public void SetItem(SlotData data)
        {
            SetItem(data.Item, data.Count);
        }

        public void SetItem(Item item, int count)
        {
            Item = item;
            Count = count;
            _image.sprite = item?.Sprite;

            if (_image.sprite == null) _image.color = new Color(1, 1, 1, 0);
            else _image.color = new Color(1, 1, 1, 1);

            _itemCount.SetText(Count > 0 ? Count.ToString() : "");
        }

        public void Select()
        {
            _selectFrame.enabled = true;
        }

        public void Deselect()
        {
            _selectFrame.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryUiController.SelectSlot(this);
        }
    }
}