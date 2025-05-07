using System.Linq;
using Game.Components;
using Game.Entities.Common;
using Game.Entities.Player;
using Game.StateManagement;
using UnityEditor;
using UnityEngine;

namespace Game.Entities.Slime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MoveableComponent))]
    public class SlimeEntity : Entity
    {
        [HideInInspector] public MoveableComponent Moveable;

        protected override void Awake()
        {
            base.Awake();

            Health = GetComponent<HealthComponent>();
            Moveable = GetComponent<MoveableComponent>();
        }

        protected override void Start()
        {
            base.Start();

            StateManager.SetState(new SlimeIdleState(this));
        }

        public void SearchForTargets()
        {
            Collider2D[] searchCandidates = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Entity"));

            foreach (var searchCandidate in searchCandidates)
            {
                if (searchCandidate.gameObject.GetComponent<Entity>() is PlayerEntity)
                {
                    StateManager.SetState(new SlimeFollowState(this, searchCandidate.gameObject.GetComponent<Entity>()));
                }
            }
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