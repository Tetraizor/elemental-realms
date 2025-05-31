using UnityEngine;

namespace Game.Data
{
    public class InteractionContext
    {
        public GameObject Source = null;
        public Vector2 HitPoint = Vector2.zero;
        public Vector2 HitDirection = Vector2.zero;
        public float Speed = 0;
    }
}