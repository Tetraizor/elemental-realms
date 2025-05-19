using Game.Enum;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
    public class Item : ScriptableObject
    {
        [Header("Item Properties")]
        [SerializeField] private int _id = 0;
        [SerializeField] private string _itemName = "Item Name";
        [SerializeField] private string _description = "Item Description";
        [SerializeField] private Sprite _sprite = null;

        [SerializeField] private ItemType _type = ItemType.Material;
        [SerializeField] private int _mass = 5;
        [SerializeField] private int _maxStackSize = 99;
        [SerializeField] private GameObject _prefab;

        [SerializeField] private bool _droppable = true;

        [SerializeField] public StatusEffectType ResistantEffects = StatusEffectType.All;

        public int Id => _id;
        public string Name => _itemName;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public ItemType Type => _type;
        public int Mass => _mass;
        public int MaxStackSize => _maxStackSize;
        public GameObject Prefab => _prefab;
        public bool Droppable => _droppable;
    }
}