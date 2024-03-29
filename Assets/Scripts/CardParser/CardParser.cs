using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

public class CardParser
{

    public enum Gender
    {
        [EnumMember(Value = "m")]
        Male,
        [EnumMember(Value = "f")]
        Female,
        [EnumMember(Value = "n")]
        Neuter
    };

    public class Noun
    {
        [JsonProperty]
        public string name;
        [JsonProperty("sex")]
        public Gender gender;
        [JsonProperty("aan")]
        public string indefiniteArticle;
        [JsonProperty]
        public string adjective;
        [JsonProperty("aanAdjective")]
        public string indefiniteArticleAdjective;
        // Filled in at runtime.
        public Category category;
    }

    class NounCategory
    {
        [JsonProperty]
        public string category;
        [JsonProperty]
        public List<Noun> nouns;
    }
    class NounJson
    {
        [JsonProperty]
        public List<NounCategory> categories { get; set; }
    }


    public class SetupCard
    {
        [JsonProperty]
        public string text;

        // Only filled in when the card is instantiated.
        public Noun noun;
        public Category counterCategory;
    }

    class SetupJson
    {
        [JsonProperty]
        public List<SetupCard> cards;
    }

    public class PunchlineCard
    {
        [JsonProperty]
        public string text;
        [JsonProperty("category")]
        public string category;

        // Only filled in when the card is instantiated.
        public Category goodCategory;
        public Category counterCategory;
    }

    class PunchlineJson
    {
        [JsonProperty]
        public List<PunchlineCard> cards;

    }
    private string nounFile = "GameData/nouns";
    private string setupFile = "GameData/setup";
    private string punchlineFile = "GameData/punchline";

    // dictionary from category to a list of nouns.
    private Dictionary<string, List<Noun>> nouns;
    private List<Noun> allNouns;

    private List<SetupCard> setupCards;

    private List<PunchlineCard> punchlineCards;

    // How many times we try to regenerate the card if it already exists.
    private int CARD_ALREADY_EXIST_TRIES = 10;

    public void ReadFiles(CategoryReader categoryReader)
    {
        ReadNouns(categoryReader);
        ReadSetups();
        ReadPunchlines(categoryReader);
    }

    private void ReadNouns(CategoryReader categoryReader)
    {
        string jsonString = ReadTextFile(nounFile);
        NounJson nounJson = JsonConvert.DeserializeObject<NounJson>(jsonString);

        nouns = new Dictionary<string, List<Noun>>();
        allNouns = new List<Noun>();

        foreach (var categoryJson in nounJson.categories)
        {
            var category = categoryReader.GetCategoryByName(categoryJson.category);
            foreach (var noun in categoryJson.nouns)
            {
                noun.category = category;
            }
            nouns[category.name] = categoryJson.nouns;
            allNouns.AddRange(categoryJson.nouns);
        }
    }

    private void ReadSetups()
    {
        string jsonString = ReadTextFile(setupFile);
        SetupJson setupJson = JsonConvert.DeserializeObject<SetupJson>(jsonString);
        setupCards = setupJson.cards;
    }

    private void ReadPunchlines(CategoryReader categoryReader)
    {
        string jsonString = ReadTextFile(punchlineFile);
        PunchlineJson punchlineJson = JsonConvert.DeserializeObject<PunchlineJson>(jsonString);
        punchlineCards = punchlineJson.cards;
        foreach (var punchline in punchlineCards)
        {
            if (punchline.category == null)
            {
                Debug.LogWarning($"Punchline {punchline.text} does not have a category assigned.");
                punchline.goodCategory = categoryReader.categories[0];
            }
            else
            {
                punchline.goodCategory = categoryReader.GetCategoryByName(punchline.category);
            }
        }
    }


    private string ReadTextFile(string path)
    {
        //return File.ReadAllText("Assets/Resources/" + path + ".json");
        return Resources.Load<TextAsset>(path).text;
    }


    public List<SetupCard> GetRandomSetups(int count, List<Category> audienceCategories, List<Category> allCategories, int goodCards)
    {
        var setups = new List<SetupCard>();
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < CARD_ALREADY_EXIST_TRIES; j++)
            {
                Category category;
                if (i < goodCards)
                {
                    //Debug.Log("Get a good card");
                    category = GetRandomCategory(audienceCategories, true);
                }
                else
                {
                    //Debug.Log("Get a possibly bad card");
                    category = GetRandomCategory(allCategories, true);
                }
                SetupCard setup = GetRandomSetup(category, allCategories);
                bool isSetupUnique = true;
                foreach (var otherSetup in setups)
                {
                    if (otherSetup.text == setup.text && otherSetup.noun == setup.noun)
                    {
                        isSetupUnique = false;
                        break;
                    }
                }
                if (isSetupUnique)
                {
                    setups.Add(setup);
                    break;
                }
            }
        }
        Debug.Log($"Generated {setups.Count} setups");
        setups.Shuffle();
        return setups;
    }

    private Category GetRandomCategory(List<Category> categories, bool isSetup)
    {
        for (int i = 0; i < 100; i++)
        {
            var category = categories[Random.Range(0, categories.Count)];
            if (category.isSetup == isSetup)
            {
                return category;
            }
        }
        Debug.LogWarning($"Could not find category isSetup: {isSetup}");
        foreach (var category in categories)
        {
            Debug.Log($"- {category.name} isSetup: {category.isSetup}");
        }
        return categories[0];
    }

    public SetupCard GetRandomSetup(Category category, List<Category> allCategories)
    {
        Noun noun = GetRandomNoun(category);
        SetupCard card = setupCards[Random.Range(0, setupCards.Count)];
        return new SetupCard
        {
            text = card.text,
            noun = noun,
            counterCategory = GetCounterCategory(category, allCategories)
        };
    }

    private Category GetCounterCategory(Category category, List<Category> allCategories)
    {
        for (int i = 0; i < 100; i++)
        {
            var candidate = allCategories[Random.Range(0, allCategories.Count)];
            if (candidate != category)
            {
                return candidate;
            }
        }
        return category;
    }

    // TODO: GetRandomNoun for categories
    private Noun GetRandomNoun(Category category)
    {
        if (!nouns.ContainsKey(category.name))
        {
            Debug.LogWarning($"No nouns defined for category {category}.");
            return GetCompletelyRandomNoun();
        }
        return nouns[category.name][Random.Range(0, nouns[category.name].Count)];
        //return allNouns[Random.Range(0, allNouns.Count)];
    }

    private Noun GetCompletelyRandomNoun()
    {
        return allNouns[Random.Range(0, allNouns.Count)];
    }

    private Noun GetRandomNounForCategory(string category)
    {
        if (!nouns.ContainsKey(category))
        {
            Debug.LogWarning($"No nouns defined for category {category}.");
            return GetCompletelyRandomNoun();
        }
        return nouns[category][Random.Range(0, nouns[category].Count)];
    }


    public List<PunchlineCard> GetRandomPunchlines(int count, List<Category> allCategories)
    {
        var punchlines = new List<PunchlineCard>();
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < CARD_ALREADY_EXIST_TRIES; j++)
            {
                var punchline = GetRandomPunchline();
                bool isPunchlineUnique = true;
                foreach (var otherPunchline in punchlines)
                {
                    if (otherPunchline.text == punchline.text)
                    {
                        isPunchlineUnique = false;
                        break;
                    }
                }
                if (isPunchlineUnique)
                {
                    punchlines.Add(new PunchlineCard
                    {
                        text = punchline.text,
                        goodCategory = punchline.goodCategory,
                        counterCategory = GetCounterCategory(punchline.goodCategory, allCategories)
                    });
                    break;
                }
            }
        }
        Debug.Log($"Generated {punchlines.Count} punchlines");
        return punchlines;
    }


    public PunchlineCard GetRandomPunchline()
    {
        return punchlineCards[Random.Range(0, punchlineCards.Count)];
    }

}
