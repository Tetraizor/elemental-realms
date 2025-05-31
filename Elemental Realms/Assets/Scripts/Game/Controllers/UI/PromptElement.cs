using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PromptElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _promptText;

        public void Setup(string label, string promptText)
        {
            _icon.transform.parent.gameObject.SetActive(false);
            _promptText.gameObject.SetActive(true);
            _promptText.SetText(promptText);

            _label.SetText(label);
        }

        public void Setup(string label, Sprite promptSprite)
        {
            _icon.sprite = promptSprite;
            _icon.transform.parent.gameObject.SetActive(true);
            _promptText.gameObject.SetActive(false);

            _label.SetText(label);
        }
    }
}