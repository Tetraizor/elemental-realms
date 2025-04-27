using UnityEngine;
using Game.StateManagement;
using Game.Data;
using System;

namespace Game.Entities.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityBase : MonoBehaviour
    {
        [Header("Base Properties")]
        // Health
        [SerializeField] private int _baseHealth = 20;
        public int BaseHealth => _baseHealth;
        public int Health { protected set; get; }

        [Header("Base References")]
        [SerializeField] protected GameObject _renderer;

        public Rigidbody2D EntityRigidbody { get; protected set; }
        public StateManager StateManager { private set; get; }

        protected virtual void Awake()
        {
            StateManager = new StateManager();

            EntityRigidbody = GetComponent<Rigidbody2D>();

            Health = _baseHealth;
        }

        protected virtual void Start() { }

        protected virtual void FixedUpdate()
        {
            StateManager.FixedTickState(Time.fixedDeltaTime);
        }

        protected virtual void Update()
        {
            StateManager.TickState(Time.deltaTime);
        }

        public virtual void TakeDamage(DamageData damageData)
        {
            SetHealth(Health - damageData.Strength);
        }

        public void SetHealth(int health)
        {
            Health = Math.Clamp(health, 0, _baseHealth);

            if (health == 0)
            {
                Kill();
            }
        }

        public virtual void Kill()
        {
            Destroy(gameObject);
        }
    }
}