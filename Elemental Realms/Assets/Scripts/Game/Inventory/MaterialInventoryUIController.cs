using Game.Controllers.UI;
using TMPro;
using UnityEngine;

namespace Game.Inventory
{
    public class MaterialInventoryUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private InventoryUIController _inventoryUiController;

        private void Start()
        {
            _inventoryUiController = GetComponent<InventoryUIController>();
            _inventoryUiController.SlotSelected.AddListener(OnSlotSelected);
        }

        private void OnSlotSelected(ItemSlot slot)
        {

        }
    }
}