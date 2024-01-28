using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardDealer : MonoBehaviour
{

    [Header("Input Data")]
    [Tooltip("Number of setup cards in hand.")]
    public int setupCardsCount = 5;
    [Tooltip("Number of punchline cards in hand.")]
    public int punchlineCardCount = 5;

    [NotNull]
    public CategoryReader categoryReader;
    [Header("Output")]

    [Tooltip("Is called when the displayed cards text is changed.")]
    public UnityEvent<CardParser.SetupCard, CardParser.PunchlineCard> onCardsChanged;
    [Tooltip("Is called when the cards are submitted.")]
    public UnityEvent<CardParser.SetupCard, CardParser.PunchlineCard> onCardSubmitted;

    private CardParser cardParser;

    private List<CardParser.SetupCard> setupCards;
    private List<CardParser.PunchlineCard> punchlineCards;

    private int currentSetup = 0;
    private int currentPunchline = 0;

    [Tooltip("How many of the cards are for the audience.")]
    public int goodCards = 2;

    [Tooltip("Enable all categories for testing")]
    public bool useAllCategories = false;
    public bool useAudienceCategories = false;


    void OnEnable()
    {
        if (useAllCategories || useAudienceCategories)
        {
            GameOrganizer.Instance.OnNextRound += OnNextRound;
        }
    }

    void OnDisable()
    {
        if (useAllCategories || useAudienceCategories)
        {
            GameOrganizer.Instance.OnNextRound -= OnNextRound;
        }
    }

    public void Start()
    {
        SetupIfNotYet();
        if (useAllCategories)
        {
            RegenerateCards(categoryReader.categories);
        }
        else if (useAudienceCategories)
        {
            RegenerateCards(GameOrganizer.Instance.audience.GetCurrentAudienceCategories());
        }
    }

    private void OnNextRound()
    {
        Debug.Log("----- on next round");
        if (useAllCategories)
        {
            RegenerateCards(categoryReader.categories);
        }
        else if (useAudienceCategories)
        {
            RegenerateCards(GameOrganizer.Instance.audience.GetCurrentAudienceCategories());
        }
    }

    private void Setup()
    {
        cardParser = new CardParser();
        cardParser.ReadFiles(categoryReader);

    }

    // Test from editor
    public void SetupIfNotYet()
    {
        if (cardParser == null)
        {
            Setup();
        }
    }

    public void RegenerateCards(List<Category> audienceCategories)
    {
        if (audienceCategories.Count == 0)
        {
            audienceCategories = GameOrganizer.Instance.audience.GetCurrentAudienceCategories();
        }
        /*setupCards = new List<CardParser.SetupCard>();
        for (int i = 0; i < setupCardsCount; i++)
        {
            var setup = cardParser.GetRandomSetup();
            setupCards.Add(setup);
        }*/
        var allCategories = categoryReader.categories;

        setupCards = cardParser.GetRandomSetups(setupCardsCount, audienceCategories, allCategories, goodCards);

        /*punchlineCards = new List<CardParser.PunchlineCard>();
        for (int i = 0; i < punchlineCardCount; i++)
        {
            var punchline = cardParser.GetRandomPunchline();
            punchlineCards.Add(punchline);
        }*/
        punchlineCards = cardParser.GetRandomPunchlines(punchlineCardCount, allCategories);

        currentSetup = 0;
        currentPunchline = 0;

        DisplayCurrentCards();
    }

    private void DisplayCurrentCards()
    {
        var setup = setupCards[currentSetup];
        var punchline = punchlineCards[currentPunchline];
        onCardsChanged.Invoke(setup, punchline);
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
        if (currentSetup < 0)
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
        if (currentPunchline < 0)
        {
            currentPunchline = punchlineCards.Count - 1;
        }
        DisplayCurrentCards();
    }

    public void Submit()
    {
        var setup = setupCards[currentSetup];
        var punchline = punchlineCards[currentPunchline];
        onCardSubmitted.Invoke(setup, punchline);
    }
}
