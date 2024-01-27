using UnityEngine;

namespace Sabelkalat
{
    public class Card : MonoBehaviour
    {

        [Tooltip("The pose when this card is in view and not focused.")]
        public Transform holdTarget = null;

        [Tooltip("The pose when this card is in view and focused.")]
        public Transform focusTarget = null;

        [Tooltip("The pose when this card is not in view.")]
        public Transform hiddenTarget = null;

        private bool hasFocus = false;

        [Header("Diagnostics")]
        public bool enableLog = false;

        public void Focus()
        {
            Log($"Focusing {this}");
            // TODO: move to focusTarget
            hasFocus = true;
        }

        public void Unfocus()
        {
            Log($"Unfocusing {this}");
            // TODO: move to holdTarget
            hasFocus = false;
        }

        public void Hide()
        {
            Log($"Hiding {this}");
            // TODO: move to hiddenTarget
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
                // TODO: move to holdTarget
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
}  // namespace Sabelkalat
