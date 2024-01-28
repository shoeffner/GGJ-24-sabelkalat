using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrinter
{
    private static Dictionary<CardParser.Gender, string> subjectivePronouns = new() {
        {CardParser.Gender.Male, "he"},
        {CardParser.Gender.Female, "she"},
        {CardParser.Gender.Neuter, "it"},
    };

    private static Dictionary<CardParser.Gender, string> objectivePronouns = new() {
        {CardParser.Gender.Male, "him"},
        {CardParser.Gender.Female, "her"},
        {CardParser.Gender.Neuter, "it"},
    };

    private static Dictionary<CardParser.Gender, string> possessivePronouns = new() {
        {CardParser.Gender.Male, "his"},
        {CardParser.Gender.Female, "her"},
        {CardParser.Gender.Neuter, "its"},
    };

    public static string GetSetupText(CardParser.SetupCard setupCard)
    {
        var text = ReplaceNounPlaceholders(setupCard.text, setupCard.noun);

        if (!string.IsNullOrEmpty(text))
        {
            // Make the first letter uppercase
            text = char.ToUpper(text[0]) + text.Substring(1);
        }

        return text;
    }

    public static string GetPunchlineText(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard)
    {
        return ReplaceNounPlaceholders(punchlineCard.text, setupCard.noun);
    }

    private static string ReplaceNounPlaceholders(string text, CardParser.Noun noun)
    {
        // a / an handling
        while (text.IndexOf("<aan>") >= 0)
        {
            var aanIndex = text.IndexOf("<aan>");
            var before = text.Substring(0, aanIndex);
            var after = text.Substring(aanIndex + 5);

            var nextNoun = after.IndexOf("<noun>");
            var nextAdjective = after.IndexOf("<adjective>");

            if (nextNoun == 1)
            {
                if (noun.indefiniteArticle == null)
                {
                    Debug.LogWarning($"Noun {noun.name} does not have aan defined!");
                    noun.indefiniteArticle = "a";
                }
                text = before + noun.indefiniteArticle + after;
            }
            else if (nextAdjective == 1)
            {
                if (noun.indefiniteArticleAdjective == null)
                {
                    Debug.LogWarning($"The adjective {noun.adjective} of noun {noun.name} does not have aan defined!");
                    noun.indefiniteArticleAdjective = "a";
                }
                text = before + noun.indefiniteArticleAdjective + after;
            }
            else
            {
                Debug.LogWarning("After <aan> there is something else than a <noun> or <adjective>.");
                Debug.Log($"{text}");
                Debug.Log($"Next Noun: {nextNoun}");
                Debug.Log($"Next Adjective: {nextAdjective}");
                text = before + "<error>" + after;
            }
        }

        text = text.Replace("<noun>", noun.name);
        text = text.Replace("<pronounSubjective>", subjectivePronouns[noun.gender]);
        text = text.Replace("<pronounObjective>", objectivePronouns[noun.gender]);
        text = text.Replace("<pronounPossessive>", possessivePronouns[noun.gender]);
        text = text.Replace("<adjective>", noun.adjective);

        return text;
    }
}
