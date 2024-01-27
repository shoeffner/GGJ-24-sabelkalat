using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardParserTest : MonoBehaviour
{

    [Header("Input Data")]
    //[Tooltip("Which categories the audience is currently requesting.")]
    //public List<string> audienceCategories;
    [Tooltip("Number of setup cards in hand.")]
    public int setupCardsCount = 5;
    [Tooltip("Number of punchline cards in hand.")]
    public int punchlineCardCount = 5;

    [Header("UI")]

    [NotNull]
    public TextMeshProUGUI setupCardText;
    [NotNull]
    public TextMeshProUGUI setupCardCategoriesText;
    [NotNull]
    public TextMeshProUGUI punchlineCardText;
    [NotNull]
    public TextMeshProUGUI punchlineCardCategoriesText;

    private CardParser cardParser;

    private List<CardParser.SetupCard> setupCards;
    private List<CardParser.PunchlineCard> punchlineCards;

    private int currentSetup = 0;
    private int currentPunchline = 0;

    public void Start()
    {
        Setup();
        RegenerateCards();
    }

    private void Setup()
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
        setupCards = new List<CardParser.SetupCard>();
        for (int i = 0; i < setupCardsCount; i++)
        {
            var setup = cardParser.GetRandomSetup();
            setupCards.Add(setup);
        }

        punchlineCards = new List<CardParser.PunchlineCard>();
        for (int i = 0; i < punchlineCardCount; i++)
        {
            var punchline = cardParser.GetRandomPunchline();
            punchlineCards.Add(punchline);
        }

        currentSetup = 0;
        currentPunchline = 0;

        DisplayCurrentCards();
    }

    private void DisplayCurrentCards()
    {
        var setup = setupCards[currentSetup];
        var punchline = punchlineCards[currentPunchline];
        setupCardText.text = CardPrinter.GetSetupText(setup);
        setupCardCategoriesText.text = $"+ {setup.noun.category}\n- {setup.counterCategory}";
        punchlineCardText.text = CardPrinter.GetPunchlineText(setup, punchline);
        punchlineCardCategoriesText.text = $"+ {punchline.category}\n- {punchline.counterCategory}";
    }

    public void NextSetup()
    {
        currentSetup++;
        if (currentSetup >= setupCards.Count)
        {
            currentSetup = 0;
        }
        DisplayCurrentCards();
    }

    public void PreviousSetup()
    {
        currentSetup--;
        if (currentSetup <= 0)
        {
            currentSetup = setupCards.Count - 1;
        }
        DisplayCurrentCards();
    }

    public void NextPunchline()
    {
        currentPunchline++;
        if (currentPunchline >= punchlineCards.Count)
        {
            currentPunchline = 0;
        }
        DisplayCurrentCards();
    }

    public void PreviousPunchline()
    {
        currentPunchline--;
        if (currentPunchline <= 0)
        {
            currentPunchline = punchlineCards.Count - 1;
        }
        DisplayCurrentCards();
    }
}
