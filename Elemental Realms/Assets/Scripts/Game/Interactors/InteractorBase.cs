using Game.Data;
using Game.Entities.Common;
using Game.Entities.Player;
using UnityEngine;

namespace Game.Interactors
{
    public abstract class InteractorBase : MonoBehaviour
    {
        public PlayerEntity Player { get; private set; }

        public float SpeedMultiplier { get; protected set; } = 1;

        public abstract void StartInteraction(InteractionData data);
        public abstract void CancelInteraction(InteractionData data);
        public abstract void EndInteraction(InteractionData data);

        public virtual void PullUp(PlayerEntity player)
        {
            Player = player;
        }
        public abstract void PutDown();

        public abstract void OnHitEnter(EntityBase entity);
        public abstract void OnHitStay(EntityBase entity);
        public abstract void OnHitExit(EntityBase entity);
    }
}