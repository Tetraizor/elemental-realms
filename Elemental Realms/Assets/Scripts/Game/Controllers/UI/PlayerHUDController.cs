using System;
using Game.Entities.Player;
using TMPro;
using UnityEngine;

namespace Game.Controllers.UI
{
    public class PlayerHUDController : MonoBehaviour
    {
        [Header("Entity References")]
        [SerializeField] private PlayerEntity _player;

        [Header("UI References")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private TextMeshProUGUI _playerHealthLabel;

        private void Awake()
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
            _playerHealthLabel.SetText($"Health: {_player.Health.Health}");
        }

        private void OnPlayerHealthChanged(float newHealth)
        {
            _playerHealthLabel.SetText($"Health: {(int)newHealth}");
        }
    }
}