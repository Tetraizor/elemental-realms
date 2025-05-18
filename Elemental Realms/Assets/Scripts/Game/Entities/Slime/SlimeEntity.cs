using System.Linq;
using Game.Components;
using Game.Entities.Common;
using Game.Entities.Player;
using Game.StateManagement;
using Game.Tools;
using UnityEditor;
using UnityEngine;

namespace Game.Entities.Slime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MoveableComponent))]
    public class SlimeEntity : Entity
    {
        [HideInInspector] public MoveableComponent Moveable;
        public AreaDamager AreaDamager { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            Moveable = GetComponent<MoveableComponent>();

            AreaDamager = GetComponentInChildren<AreaDamager>();
            AreaDamager.Setup(gameObject);

            Health.Changed.AddListener(OnHealthChanged);
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

        protected override void Kill()
        {
            base.Kill();

            StateManager.SetState(new SlimeKillState(this));

            Destroy(gameObject, 1.5f);
        }

        private void OnHealthChanged(float newHealth)
        {
            if (newHealth != 0)
                StateManager.SetState(new SlimeTakeDamageState(this));
        }
    }
}