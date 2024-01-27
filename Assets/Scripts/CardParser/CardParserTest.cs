using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardParserTest : MonoBehaviour
{
    [NotNull]
    public TextMeshProUGUI setupCardText;
    [NotNull]
    public TextMeshProUGUI setupCardCategoriesText;
    [NotNull]
    public TextMeshProUGUI punchlineCardText;
    [NotNull]
    public TextMeshProUGUI punchlineCardCategoriesText;

    private CardParser cardParser;

    public void Start()
    {

        cardParser = new CardParser();
        cardParser.ReadFiles();
    }

    // Test from editor
    public void Test()
    {
        if (cardParser == null)
        {
            Start();
        }
        RegenerateCards();
    }


    public void RegenerateCards()
    {
        var setup = cardParser.GetRandomSetup();
        var punchline = cardParser.GetRandomPunchline();

        setupCardText.text = CardPrinter.GetSetupText(setup);
        setupCardCategoriesText.text = $"+ {setup.noun.category}";
        punchlineCardText.text = CardPrinter.GetPunchlineText(setup, punchline);
        punchlineCardCategoriesText.text = $"+ {punchline.category}";
    }
}
