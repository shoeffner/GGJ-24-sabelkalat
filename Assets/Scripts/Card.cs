using UnityEngine;

namespace Sabelkalat
{
    public class Card : MonoBehaviour
    {
        [Range(0.05f, 0.8f)]
        public float rotationSpeed = 0.2f;

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

        public void Swap()
        {
            LeanTween.rotate(gameObject, swapTarget.transform.rotation.eulerAngles, rotationSpeed).setEaseInCirc().setOnComplete(Show);
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
