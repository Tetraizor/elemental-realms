using UnityEngine;
using UnityEngine.Events;
using Game.StateManagement;
using Game.Components;
using UnityEditor;
using System.Linq;
using Game.Enum;
using Game.Status;
using Game.Modifiers;

namespace Game.Entities.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    public class Entity : MonoBehaviour, ISpeedModifier
    {
        #region Properties

        [Header("Entity Properties")]

        [SerializeField] protected EntityTag _tags = EntityTag.None;
        [SerializeField] public StatusEffectType ResistantEffects = StatusEffectType.None;
        public EntityTag Tags => _tags;

        [HideInInspector] public GameObject EntityRenderer { get; protected set; }
        [HideInInspector] public Rigidbody2D EntityRigidbody { get; protected set; }
        [HideInInspector] public StateManager StateManager { get; protected set; } = new StateManager();
        [HideInInspector] public StatusManager StatusManager { get; protected set; }

        [HideInInspector] public UnityEvent Spawned;
        [HideInInspector] public UnityEvent Killed;

        [HideInInspector] public HealthComponent Health;

        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            StatusManager = new StatusManager(this);
            EntityRigidbody = GetComponent<Rigidbody2D>();
            Health = GetComponent<HealthComponent>();
            EntityRenderer = transform.Find("Renderer").gameObject;

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
            StatusManager.TickStatus(Time.deltaTime);
        }

        protected virtual void Kill()
        {
            Killed?.Invoke();
            StatusManager.Statuses.ForEach(status => StatusManager.RemoveStatus(status));

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            StatusManager.Statuses.ForEach(status => StatusManager.RemoveStatus(status));
        }

        public float GetSpeedModifier()
        {
            float speedMultiplier = 1;

            StatusManager.Statuses.ForEach(status =>
            {
                if (status.Status is ISpeedModifier speedModifier)
                {
                    speedMultiplier *= speedModifier.GetSpeedModifier();
                }
            });

            return speedMultiplier;
        }

        #endregion
    }
}