using Game.Entities.Common;
using UnityEngine;

namespace Game.Interactors
{
    public class InteractorField : MonoBehaviour
    {
        [SerializeField] private InteractorBase _interactor;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _interactor.Player) return;

            if (collision.TryGetComponent<EntityBase>(out var entity))
                _interactor.OnHitEnter(entity);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject == _interactor.Player) return;

            if (collision.TryGetComponent<EntityBase>(out var entity))
                _interactor.OnHitStay(entity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == _interactor.Player) return;

            if (collision.TryGetComponent<EntityBase>(out var entity))
                _interactor.OnHitExit(entity);
        }
    }
}