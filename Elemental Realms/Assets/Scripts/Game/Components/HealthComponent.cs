using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Properties")]
        [SerializeField] private float _baseHealth = 20;
        public float BaseHealth => _baseHealth;
        public float Health { protected set; get; }

        [Header("Events")]
        public UnityEvent Killed;
        public UnityEvent<float> Changed;

        public bool IsInvincible = false;

        private void Awake()
        {
            Health = _baseHealth;
        }

        public virtual void TakeDamage(float amount)
        {
            if (IsInvincible) return;
            SetHealth(Health - amount);
        }

        public void SetHealth(float health)
        {
            Health = Mathf.Clamp(health, 0, _baseHealth);

            Changed?.Invoke(Health);

            if (Health == 0)
            {
                Kill();
            }
        }

        public virtual void Kill()
        {
            Killed?.Invoke();
        }
    }
}