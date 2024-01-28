using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public int positiveMultiplier = 1;
    public int negativeMultiplier = -1;

    public void OnCardSubmitted(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard)
    {
        Audience audience = GameOrganizer.Instance.audience;

        int score = 0;

        foreach (var category in audience.GetCurrentAudienceCategories())
        {
            if (setupCard.noun.category == category)
            {
                score += positiveMultiplier;
            }
            if (setupCard.counterCategory == category)
            {
                score -= positiveMultiplier;
            }

            if (punchlineCard.goodCategory == category)
            {
                score += positiveMultiplier;
            }
            if (punchlineCard.counterCategory == category)
            {
                score -= positiveMultiplier;
            }
        }

        //Debug.Log($"Score change: {score}");
        GameOrganizer.Instance.AddScore(score);
    }
}
