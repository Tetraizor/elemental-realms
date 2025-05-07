using UnityEngine;
using UnityEngine.Events;
using Game.StateManagement;
using Game.Components;
using UnityEditor;
using System.Linq;

namespace Game.Entities.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    public class Entity : MonoBehaviour
    {
        #region Properties

        [Header("Entity Properties")]
        [HideInInspector] public GameObject EntityRenderer { get; protected set; }
        [HideInInspector] public Rigidbody2D EntityRigidbody { get; protected set; }
        [HideInInspector] public StateManager StateManager { get; protected set; }

        [HideInInspector] public UnityEvent Spawned;
        [HideInInspector] public UnityEvent Killed;

        [HideInInspector] public HealthComponent Health;

        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            StateManager = new StateManager();
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
        }

        protected virtual void Kill()
        {
            Killed?.Invoke();
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Handles.Label(transform.position + Vector3.up * 2f, StateManager.CurrentState.GetType().ToString().Split('.').Last());
        }
#endif
    }
}