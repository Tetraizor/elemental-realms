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

        private void Start()
        {
            _player.Health.Changed.AddListener(OnPlayerHealthChanged);
            _player.Spawned.AddListener(OnPlayerSpawned);
            _player.Health.Killed.AddListener(OnPlayerKilled);
        }

        private void OnPlayerKilled()
        {
            _root.gameObject.SetActive(false);
        }

        private void OnPlayerSpawned()
        {
            _root.gameObject.SetActive(true);
        }

        private void OnPlayerHealthChanged(float oldHealth, float newHealth)
        {
            float targetFill = newHealth / _player.Health.BaseHealth;
            _playerHealthColor.DOFillAmount(targetFill, 0.5f).SetEase(Ease.OutQuad);

            DOTween.To(() => oldHealth, x =>
            {
                oldHealth = x;
                _playerHealthLabel.SetText($"{oldHealth:0}");
            }, (int)newHealth, 0.5f).SetEase(Ease.OutQuad);
        }
    }
}