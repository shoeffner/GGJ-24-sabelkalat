using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CrowdReactionDisplay : MonoBehaviour
{

    public string prefix = "Crowd Reaction: ";
    private TextMeshProUGUI text;
    private bool isStarted;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        isStarted = true;
        OnEnable();
        OnScoreChanged(0);
    }

    void OnEnable()
    {
        if (isStarted)
        {
            GameOrganizer.Instance.OnScoreChanged += OnScoreChanged;
        }
    }

    void OnDisable()
    {
        GameOrganizer.Instance.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int scoreDiff)
    {
        text.text = prefix + GameOrganizer.Instance.GetCurrentScore().ToString();
    }
}
