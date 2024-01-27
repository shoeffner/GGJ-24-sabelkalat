using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

public class CardParserTest : MonoBehaviour
{
    [Tooltip("Which categories the audience is currently requesting.")]
    public List<string> audienceCategories;
    [NotNull]
    public CardDealer cardDealer;

    [Header("UI")]

    [NotNull]
    public TextMeshProUGUI setupCardText;
    [NotNull]
    public TextMeshProUGUI setupCardCategoriesText;
    [NotNull]
    public TextMeshProUGUI punchlineCardText;
    [NotNull]
    public TextMeshProUGUI punchlineCardCategoriesText;

    void Start()
    {
        cardDealer.SetupIfNotYet();
    }

    public void RegenerateCards()
    {
        List<Category> actualAudienceCategories = new List<Category>();
        foreach (var category in audienceCategories)
        {
            actualAudienceCategories.Add(cardDealer.categoryReader.GetCategoryByName(category));
        }
        cardDealer.RegenerateCards(actualAudienceCategories);
    }

    public void DisplayCards(CardParser.SetupCard setup, CardParser.PunchlineCard punchline)
    {
        setupCardText.text = CardPrinter.GetSetupText(setup);
        setupCardCategoriesText.text = $"+ {setup.noun.category.name}\n- {setup.counterCategory.name}";
        punchlineCardText.text = CardPrinter.GetPunchlineText(setup, punchline);
        punchlineCardCategoriesText.text = $"+ {punchline.goodCategory.name}\n- {punchline.counterCategory.name}";
    }

    // Test from editor
    public void Test()
    {
        cardDealer.SetupIfNotYet();
        RegenerateCards();
    }

}
