using UnityEngine;
using UnityEngine.Events;
using Game.StateManagement;
using Game.Components;

namespace Game.Entities.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    public class Entity : MonoBehaviour
    {
        #region Properties

        [Header("Base References")]
        [SerializeField] protected GameObject _renderer;

        public Rigidbody2D EntityRigidbody { get; protected set; }
        public StateManager StateManager { get; protected set; }

        public UnityEvent Spawned;
        public UnityEvent Killed;

        public HealthComponent Health;

        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            StateManager = new StateManager();

            EntityRigidbody = GetComponent<Rigidbody2D>();

            Health.Killed.AddListener(Kill);
        }

        protected virtual void Start()
        {
            Spawned?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            StateManager.FixedTickState(Time.fixedDeltaTime);
        }

        protected virtual void Update()
        {
            StateManager.TickState(Time.deltaTime);
        }

        protected virtual void Kill()
        {
            Killed?.Invoke();

            Destroy(gameObject);
        }

        #endregion
    }
}