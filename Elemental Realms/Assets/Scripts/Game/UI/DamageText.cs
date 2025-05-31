using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.UI
{

    [RequireComponent(typeof(TextMeshPro))]
    public class DamageText : MonoBehaviour
    {
        private TextMeshPro _damageText;

        private void Awake()
        {
            _damageText = GetComponent<TextMeshPro>();
        }

        public void Setup(string text, Color color, float lifetime = 1)
        {
            _damageText.text = text;
            _damageText.color = color;

            transform.DOMoveY(transform.position.y + 1, lifetime * .75f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _damageText.DOFade(0, lifetime * .25f).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
        }
    }
}