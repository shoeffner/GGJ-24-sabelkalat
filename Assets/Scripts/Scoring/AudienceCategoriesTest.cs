using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudienceCategoriesTest : MonoBehaviour
{
    [NotNull]
    public TextMeshProUGUI text;

    void Update()
    {
        var categories = GameOrganizer.Instance.audience.GetCurrentAudienceCategories();

        var str = "";
        foreach (var category in categories)
        {
            str += category.name + "\n";
        }

        text.text = str;

    }
}
