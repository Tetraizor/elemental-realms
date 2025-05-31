using Game.Components;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.Utils
{
    public class EditorItemUtils
    {
        [MenuItem("Utils/Item/Update Scene Items")]
        private static void UpdateSceneItems()
        {
            foreach (var pickable in GameObject.FindObjectsByType<PickableComponent>(FindObjectsSortMode.None))
            {
                pickable.InitializeWithItem(null);
                pickable.transform.name = $"Pickable {pickable.ItemInstance.Item.Name}";
            }
        }
    }
}