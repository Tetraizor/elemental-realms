using UnityEngine;
using UnityEditor;

namespace Game.Editor
{
    [InitializeOnLoad]
    public static class SpriteRendererDefaultSettings
    {
        static SpriteRendererDefaultSettings()
        {
            ObjectFactory.componentWasAdded += OnComponentAdded;
        }

        private static void OnComponentAdded(Component component)
        {
            if (component is SpriteRenderer spriteRenderer)
            {
                spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
            }
        }
    }
}