using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Data;
using Game.Entities.Common;
using Game.Entities.Player;
using Game.StateManagement;
using UnityEditor;
using UnityEngine;

namespace Game.Entities.Slime
{
    [RequireComponent(typeof(Animator))]
    public class SlimeEntity : DynamicEntityBase
    {
        protected override void Awake()
        {
            base.Awake();

            StateManager.SetState(new SlimeIdleState(this));
        }

        public void SearchForTargets()
        {
            Collider2D[] searchCandidates = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Entity"));

            foreach (var searchCandidate in searchCandidates)
            {
                if (searchCandidate.gameObject.GetComponent<EntityBase>() is PlayerEntity)
                {
                    StateManager.SetState(new SlimeFollowState(this, searchCandidate.gameObject.GetComponent<EntityBase>()));
                }
            }
        }

        public override void TakeDamage(DamageData data)
        {
            if (Health <= 0) return;

            EntityRigidbody.AddForce(data.HitDirection * data.Strength * 20, ForceMode2D.Impulse);

            base.TakeDamage(data);

            if (Health > 0)
                StateManager.SetState(new SlimeTakeDamageState(this));
        }

        public override void Kill()
        {
            StateManager.SetState(new SlimeKillState(this));

            DOVirtual.DelayedCall(2, () =>
            {
                base.Kill();
            });
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Handles.Label(transform.position + Vector3.up * 2f, StateManager.CurrentState.GetType().ToString().Split('.').Last());
        }
#endif
    }
}