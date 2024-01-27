using UnityEngine;

namespace Sabelkalat
{
    public class Card : MonoBehaviour
    {
        [Tooltip("The pose when this card is in view and focused.")]
        public Transform focusTarget = null;

        [Tooltip("The pose when this card is in view and not focused.")]
        public Transform holdTarget = null;

        [Tooltip("The pose when this card is not in view.")]
        public Transform hiddenTarget = null;

        private bool hasFocus = false;

        [Header("Diagnostics")]
        public bool enableLog = false;

        public void Focus()
        {
            if(hasFocus) return;
            Log($"Focusing {this}");
            hasFocus = true;
            LeanTween.rotateLocal(gameObject, focusTarget.position, 1f).setEase(LeanTweenType.easeInOutQuad);
        }

        public void Unfocus()
        {
            if(!hasFocus) return;
            Log($"Unfocusing {this}");
            hasFocus = false;
            LeanTween.rotateLocal(gameObject, holdTarget.position, 1f).setEase(LeanTweenType.easeInOutQuad);
        }

        public void Hide()
        {
            if(!hasFocus) return;
            Log($"Hiding {this}");
            LeanTween.rotateLocal(gameObject, hiddenTarget.position, 1f).setEase(LeanTweenType.easeInOutQuad);
            hasFocus = false;
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
                LeanTween.rotateLocal(gameObject, holdTarget.position, 1f).setEase(LeanTweenType.easeInOutQuad);
            }
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
