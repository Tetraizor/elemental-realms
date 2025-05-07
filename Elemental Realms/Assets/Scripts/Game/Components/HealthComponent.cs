using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Properties")]
        [SerializeField] private int _baseHealth = 20;
        public int BaseHealth => _baseHealth;
        public int Health { protected set; get; }

        [Header("Events")]
        public UnityEvent Killed;
        public UnityEvent<int> Changed;

        private void Awake()
        {
            Health = _baseHealth;
        }

        public virtual void TakeDamage(DamageData damageData)
        {
            SetHealth(Health - damageData.Strength);
        }

        public void SetHealth(int health)
        {
            Health = Mathf.Clamp(health, 0, _baseHealth);

            Changed?.Invoke(Health);

            if (health == 0)
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