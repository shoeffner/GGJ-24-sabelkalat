using TMPro;
using UnityEngine;

namespace Sabelkalat
{
    public enum CardType
    {
        Setup, Punchline
    }

    public class Card : MonoBehaviour
    {
        [Range(0.05f, 0.8f)]
        public float rotationSpeed = 0.2f;

        public CardType cardType = CardType.Setup;

        [Header("Pose targets")]
        [Tooltip("The pose when this card is in view and focused.")]
        public Transform focusTarget = null;

        [Tooltip("The pose when this card is in view and not focused.")]
        public Transform holdTarget = null;

        [Tooltip("The pose when this card is not in view.")]
        public Transform hiddenTarget = null;

        [Tooltip("The pose when this card is swapped.")]
        public Transform swapTarget = null;

        private bool hasFocus = false;

        [Header("Diagnostics")]
        public bool enableLog = false;
        public TextMeshProUGUI textField = null;

        private bool initialTextReceived = false;

        void Start()
        {
            textField = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void OnCardsChanged(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard)
        {
            string newText = textField.text;
            switch (cardType)
            {
                case CardType.Setup:
                    newText = CardPrinter.GetSetupText(setupCard);
                    break;
                case CardType.Punchline:
                    newText = CardPrinter.GetPunchlineText(setupCard, punchlineCard);
                    break;
            }
            if (newText != textField.text)
            {
                if (initialTextReceived)
                {
                    Swap(newText);
                }
                else
                {
                    textField.text = newText;
                    initialTextReceived = true;
                }
            }
        }

        public void Focus()
        {
            Log($"Focusing {this}");
            hasFocus = true;
            LeanTween.rotate(gameObject, focusTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInOutQuad();
        }

        public void Unfocus()
        {
            Log($"Unfocusing {this}");
            hasFocus = false;
            LeanTween.rotate(gameObject, holdTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInOutQuad();
        }

        public void Hide()
        {
            Log($"Hiding {this}");
            LeanTween.rotate(gameObject, hiddenTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInOutQuad();
        }

        public void Show()
        {
            Log($"Showing {this}");
            if (hasFocus)
            {
                Focus();
            }
            else
            {
                LeanTween.rotate(gameObject, holdTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInOutQuad();
            }
        }

        private void Swap(string newText)
        {
            Log($"{this} got new text {newText}");
            LeanTween.rotate(gameObject, swapTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInCirc().setOnComplete(UpdateTextAndShowCard, newText);
        }

        private void UpdateTextAndShowCard(object newText)
        {
            textField.text = (string)newText;
            Show();
        }

        private void Log(string msg)
        {
            if (enableLog)
            {
                Debug.Log(msg);
            }
        }
    }
}
