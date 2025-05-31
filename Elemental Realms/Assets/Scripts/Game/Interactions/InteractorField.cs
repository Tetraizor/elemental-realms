using UnityEngine;

namespace Game.Interactions
{
    public class InteractorField : MonoBehaviour
    {
        private IInteractorFieldParent _parent;

        public void Setup(IInteractorFieldParent parent) => _parent = parent;

        private void OnTriggerEnter2D(Collider2D collider) => _parent.OnHitStarted(collider);
        private void OnTriggerStay2D(Collider2D collider) => _parent.OnHitStayed(collider);
        private void OnTriggerExit2D(Collider2D collider) => _parent.OnHitEnded(collider);
    }
}