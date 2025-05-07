using UnityEngine;

namespace Game.Interactions
{
    public interface IInteractorFieldParent
    {
        public void OnHitStarted(Collider2D collider);
        public void OnHitStayed(Collider2D collider);
        public void OnHitEnded(Collider2D collider);
    }
}