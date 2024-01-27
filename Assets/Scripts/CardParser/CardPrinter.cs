using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrinter
{
    private static Dictionary<CardParser.Gender, string> pronouns = new() {
        {CardParser.Gender.Male, "he"},
        {CardParser.Gender.Female, "she"},
        {CardParser.Gender.Neuter, "it"},
    };

    public static string GetSetupText(CardParser.SetupCard setupCard)
    {
        return ReplaceNounPlaceholders(setupCard.text, setupCard.noun);
    }

    public static string GetPunchlineText(CardParser.SetupCard setupCard, CardParser.PunchlineCard punchlineCard)
    {
        return ReplaceNounPlaceholders(punchlineCard.text, setupCard.noun);
    }

    private static string ReplaceNounPlaceholders(string text, CardParser.Noun noun)
    {
        text = text.Replace("<noun>", noun.name);
        text = text.Replace("<aan>", noun.indefiniteArticle);
        text = text.Replace("<pronoun>", pronouns[noun.gender]);
        text = text.Replace("<adjective>", noun.adjective);
        return text;
    }
}
