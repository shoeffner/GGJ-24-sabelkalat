using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Sabelkalat
{
    public class PixelationActivator : MonoBehaviour
    {
        void Awake()
        {
            if (GetComponentInChildren<Canvas>().worldCamera == null)
            {
                GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            }
            GetComponentInChildren<Canvas>().worldCamera.cullingMask = LayerMask.GetMask("UI");
        }
    }
}  // namespace Sabelkalat
