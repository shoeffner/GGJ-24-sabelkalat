using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

        [Tooltip("Positive category holder")]
        public Image positiveCategory = null;

        [Tooltip("Negative category holder")]
        public Image negativeCategory = null;

        private bool hasFocus = false;

        [Header("Diagnostics")]
        public bool enableLog = false;

        private CardParser.SetupCard currentSetupCard = null;
        private CardParser.PunchlineCard currentPunchlineCard = null;

        private TextMeshProUGUI textField = null;

        private bool initialTextReceived = false;

        void Awake()
        {
            textField = GetComponentInChildren<TextMeshProUGUI>();
            if (positiveCategory == null)
            {
                Debug.LogError("No category holder for positive category");
            }
            if (negativeCategory == null)
            {
                Debug.LogError("No category holder for negative category");
            }
        }

        public void OnCardsChanged(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard)
        {
            if (!initialTextReceived)
            {
                if (cardType == CardType.Setup)
                {
                    OnSetupCardChanged(setupCard, false);
                }
                if (cardType == CardType.Punchline)
                {
                    OnPunchlineCardChanged(setupCard, punchlineCard, false);
                }
                initialTextReceived = true;
                currentSetupCard = setupCard;
                currentPunchlineCard = punchlineCard;
                return;
            }
            if (cardType == CardType.Setup && setupCard != currentSetupCard)
            {
                OnSetupCardChanged(setupCard);
                currentSetupCard = setupCard;
            }
            if (cardType == CardType.Punchline)
            {
                OnPunchlineCardChanged(setupCard, punchlineCard, punchlineCard != currentPunchlineCard);
                currentPunchlineCard = punchlineCard;
            }
        }

        public void OnSetupCardChanged(CardParser.SetupCard setupCard, bool swap = true)
        {
            string text = CardPrinter.GetSetupText(setupCard);
            if (swap)
            {
                Swap(text, setupCard.noun.category.icon, setupCard.counterCategory.icon);
            }
            else
            {
                textField.text = text;
                positiveCategory.sprite = setupCard.noun.category.icon;
                negativeCategory.sprite = setupCard.counterCategory.icon;
            }
        }

        public void OnPunchlineCardChanged(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard, bool swap)
        {
            string text = CardPrinter.GetPunchlineText(setupCard, punchlineCard);
            if (swap)
            {
                Swap(text, punchlineCard.goodCategory.icon, punchlineCard.counterCategory.icon);
            }
            else
            {
                textField.text = text;
                positiveCategory.sprite = punchlineCard.goodCategory.icon;
                negativeCategory.sprite = punchlineCard.counterCategory.icon;
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

        private void Swap(string newText, Sprite goodCategory, Sprite badCategory)
        {
            Log($"{this} got new text {newText}");
            var args = (newText, goodCategory, badCategory);
            LeanTween.rotate(gameObject, swapTarget.transform.rotation.eulerAngles, rotationSpeed)
            .setEaseInCirc()
            .setOnComplete(UpdateTextAndShowCard, args);
        }

        private void UpdateTextAndShowCard(object args)
        {
            var (newText, goodCategory, badCategory) = ((string, Sprite, Sprite))args;
            textField.text = newText;
            positiveCategory.sprite = goodCategory;
            negativeCategory.sprite = badCategory;
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
