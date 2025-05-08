using Game.Controllers.UI;
using Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Inventory
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _itemCount;

        [SerializeField] private Image _selectFrame;

        public Item Item;
        public int Count;

        public void SetItem(SlotData data)
        {
            SetItem(data.Item, data.Count);
        }

        public void SetItem(Item item, int count)
        {
            if (Item == null || Item.Id != item.Id)
            {
                Item = item;
                _image.sprite = Item?.Sprite;
            }

            if (_image.sprite == null) _image.color = new Color(1, 1, 1, 0);
            else _image.color = new Color(1, 1, 1, 1);

            Count = count;
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
            InventoryUIController.Instance.SelectSlot(this);
        }
    }
}