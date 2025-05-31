using System;
using System.Collections;
using Game.Entities.Player;
using Game.Input;
using Game.SceneElements;
using Tetraizor.SceneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Controllers.UI
{
    [RequireComponent(typeof(Animator))]
    public class TextUIController : MonoBehaviour, IInputConsumer
    {
        private PlayerEntity _player;
        private Portal _portal;

        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        private bool _isEnded = false;

        private void Start()
        {
            StartCoroutine(HideTextDelayed(5));

            _player = FindAnyObjectByType<PlayerEntity>();
            _portal = FindAnyObjectByType<Portal>();

            _player.Health.Killed.AddListener(() => StartCoroutine(ShowTextDelayed("Game Over", "You are defeated.", 1)));
            _portal.Entered.AddListener(() => StartCoroutine(ShowTextDelayed("Game Over", "You reached to the portal!\nYou won!.", 0)));

            InputController.Instance.ActivateConsumer(this);
        }

        public IEnumerator ShowTextDelayed(string title, string description, float delay)
        {
            yield return new WaitForSeconds(delay);

            ShowText(title, description);
        }

        public void ShowText(string title, string description)
        {
            if (_isEnded) return;
            else _isEnded = true;

            _title.SetText(title);
            _description.SetText(description);

            GetComponent<Animator>().SetTrigger("ShowText");
        }

        public IEnumerator HideTextDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            HideText();
        }

        public void HideText()
        {
            GetComponent<Animator>().SetTrigger("HideText");
        }

        public void ActivateInput()
        {
            var controls = InputController.Instance.Controls.Game;

            controls.Exit.performed += OnExitPressed;
            controls.Restart.performed += OnRestartPressed;
        }

        private void OnRestartPressed(InputAction.CallbackContext context)
        {
        }

        private void OnExitPressed(InputAction.CallbackContext context)
        {
            Application.Quit();
        }

        public void DeactivateInput()
        {
            var controls = InputController.Instance.Controls.Game;

            controls.Exit.performed -= OnExitPressed;
            controls.Restart.performed -= OnRestartPressed;
        }
    }
}