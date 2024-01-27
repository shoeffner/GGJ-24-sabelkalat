using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

// Display the score in a text field for testing
public class ScoreDisplay : MonoBehaviour
{
    [NotNull]
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = $"Score: {GameOrganizer.Instance.CurrentScore}";
    }
}
