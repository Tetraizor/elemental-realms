using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Game.Enum;
using Game.UI;
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

        [SerializeField] private bool _showText = true;

        [Header("Resistance Properties")]
        [SerializeField] private float _knockbackResistance = .5f;
        public float KnockbackResistance => _knockbackResistance;

        [SerializedDictionary("Damage Type", "Resistance %")]
        public SerializedDictionary<DamageType, float> TypeVsResistance = new();

        [Header("Events")]
        [HideInInspector] public UnityEvent Killed;
        [HideInInspector] public UnityEvent<float, float> Changed;

        [HideInInspector] public bool IsInvincible = false;

        private GameObject _damageText;

        private void Awake()
        {
            Health = _baseHealth;
            _damageText = Resources.Load<GameObject>("UI/DamageText");
        }

        private void Start()
        {
            SetHealth(BaseHealth);
        }

        public virtual void TakeDamage(float amount, DamageType type)
        {
            if (IsInvincible) return;

            float trueDamage = TypeVsResistance.ContainsKey(type) ? amount * (1 - TypeVsResistance[type]) : amount;

            if (_showText)
            {
                var damageText = Instantiate(_damageText,
                    transform.position +
                        new Vector3(0, 1) +
                        new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0).normalized,
                    Quaternion.identity
                ).GetComponent<DamageText>();

                float percentage = 1 - (trueDamage / BaseHealth);
                var color = new Color(1, percentage, percentage);
                damageText.Setup(trueDamage.ToString("0.0"), color);
            }

            SetHealth(Health - trueDamage);
        }

        public virtual void Heal(float amount)
        {
            if (IsInvincible) return;

            if (_showText)
            {
                var damageText = Instantiate(
                    _damageText,
                    transform.position +
                        new Vector3(0, 1) +
                        new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0).normalized,
                    Quaternion.identity
                ).GetComponent<DamageText>();

                float percentage = 1 - (amount / BaseHealth);
                var color = new Color(percentage, 1, percentage);
                damageText.Setup(amount.ToString("0.0"), color);
            }

            SetHealth(Health + amount);
        }

        public void SetHealth(float health)
        {
            float previousHealth = Health;
            Health = Mathf.Clamp(health, 0, _baseHealth);

            Changed?.Invoke(previousHealth, Health);

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