using System.Collections;
using Game.Components;
using Game.Entities.Common;
using Game.Entities.Player;
using Game.Items;
using UnityEngine;

namespace Game.Entities.Skeleton
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MoveableComponent))]
    public class SkeletonEntity : Entity
    {
        [SerializeField] private GameObject Particle;
        public GameObject Projectile;
        public Item ProjectileItem;

        [HideInInspector] public MoveableComponent Moveable;
        public Vector2 SpawnPosition;

        protected override void Awake()
        {
            base.Awake();

            SpawnPosition = transform.position;
            Moveable = GetComponent<MoveableComponent>();

            Health.Changed.AddListener(OnHealthChanged);
        }

        protected override void Start()
        {
            base.Start();

            StateManager.SetState(new SkeletonIdleState(this));
        }

        public void SearchForTargets()
        {
            Collider2D[] searchCandidates = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Entity"));

            foreach (var searchCandidate in searchCandidates)
            {
                if (searchCandidate.gameObject.GetComponent<Entity>() is PlayerEntity)
                {
                    StateManager.SetState(new SkeletonFollowState(this, searchCandidate.gameObject.GetComponent<Entity>()));
                }
            }
        }

        protected override void Kill()
        {
            Killed.Invoke();

            StateManager.SetState(new SkeletonKillState(this));

            StartCoroutine(SpawnSmoke());

            Destroy(gameObject, 1.5f);
        }

        private IEnumerator SpawnSmoke()
        {
            yield return new WaitForSeconds(1.3f);
            var particle = Instantiate(Particle, transform.position, Quaternion.identity);
            Destroy(particle.gameObject, 3);
        }

        private void OnHealthChanged(float oldHealth, float newHealth)
        {
            if (newHealth != 0)
                StateManager.SetState(new SkeletonTakeDamageState(this));
        }

        public void SpawnProjectile(Vector2 atPosition)
        {
            var direction = ((Vector3)atPosition - gameObject.transform.position).normalized;

            var projectile = Instantiate(Projectile, transform.position + direction * 2, Quaternion.identity);
            if (projectile.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(direction * 30 * rb.mass, ForceMode2D.Impulse);
            }

            if (projectile.TryGetComponent(out ProjectilePickableComponent pickable))
            {
                pickable.InitializeWithItem(new Data.ItemInstance() { Item = ProjectileItem });
                pickable.DestroyOnHitChance = 1;
                pickable.StickOnHitChance = 0;
                pickable.ActivationVelocityThreshold = 5;
            }

            Destroy(projectile, 2);
        }
    }
}