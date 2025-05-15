using System;
using Game.Entities.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.Controllers.UI
{
    public class PlayerHUDController : MonoBehaviour
    {
        [Header("Entity References")]
        [SerializeField] private PlayerEntity _player;

        [Header("UI References")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private TextMeshProUGUI _playerHealthLabel;
        [SerializeField] private Image _playerHealthColor;

        private void Awake()
        {
            _player.Health.Changed.AddListener(OnPlayerHealthChanged);
            _player.Spawned.AddListener(OnPlayerSpawned);
            _player.Health.Killed.AddListener(OnPlayerKilled);

            OnPlayerHealthChanged(_player.Health.Health);
        }

        private void OnPlayerKilled()
        {
            _root.gameObject.SetActive(false);
        }

        private void OnPlayerSpawned()
        {
            _root.gameObject.SetActive(true);
            _playerHealthLabel.SetText($"Health: {_player.Health.Health}");
        }

        private void OnPlayerHealthChanged(float newHealth)
        {
            // Animate the fill amount
            float targetFill = newHealth / _player.Health.BaseHealth;
            _playerHealthColor.DOFillAmount(targetFill, 0.5f).SetEase(Ease.OutQuad);

            // Animate the number text smoothly from current value to new value
            int currentHealth = int.Parse(_playerHealthLabel.text);
            DOTween.To(() => currentHealth, x =>
            {
                currentHealth = x;
                _playerHealthLabel.SetText($"{currentHealth}");
            }, (int)newHealth, 0.5f).SetEase(Ease.OutQuad);
        }
    }
}