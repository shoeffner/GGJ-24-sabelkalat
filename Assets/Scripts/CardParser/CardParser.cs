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
        public string category;
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
        public string counterCategory;
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
        [JsonProperty]
        public string category;

        // Only filled in when the card is instantiated.
        public string counterCategory;
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


    public void ReadFiles()
    {
        ReadNouns();
        ReadSetups();
        ReadPunchlines();
    }

    private void ReadNouns()
    {
        string jsonString = ReadTextFile(nounFile);
        NounJson nounJson = JsonConvert.DeserializeObject<NounJson>(jsonString);

        nouns = new Dictionary<string, List<Noun>>();
        allNouns = new List<Noun>();

        foreach (var category in nounJson.categories)
        {
            foreach (var noun in category.nouns)
            {
                noun.category = category.category;
            }
            nouns[category.category] = category.nouns;
            allNouns.AddRange(category.nouns);
        }
    }

    private void ReadSetups()
    {
        string jsonString = ReadTextFile(setupFile);
        SetupJson setupJson = JsonConvert.DeserializeObject<SetupJson>(jsonString);
        setupCards = setupJson.cards;
    }

    private void ReadPunchlines()
    {
        string jsonString = ReadTextFile(punchlineFile);
        PunchlineJson punchlineJson = JsonConvert.DeserializeObject<PunchlineJson>(jsonString);
        punchlineCards = punchlineJson.cards;
    }


    private string ReadTextFile(string path)
    {
        //return File.ReadAllText("Assets/Resources/" + path + ".json");
        return Resources.Load<TextAsset>(path).text;
    }


    public List<SetupCard> GetRandomSetups(int count)
    {
        var setups = new List<SetupCard>();
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < CARD_ALREADY_EXIST_TRIES; j++)
            {
                var setup = GetRandomSetup();
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
        return setups;
    }

    public SetupCard GetRandomSetup()
    {
        Noun noun = GetRandomNoun();
        SetupCard card = setupCards[Random.Range(0, setupCards.Count)];
        return new SetupCard
        {
            text = card.text,
            noun = noun
        };
    }

    // TODO: GetRandomNoun for categories
    private Noun GetRandomNoun()
    {
        return allNouns[Random.Range(0, allNouns.Count)];
    }

    public PunchlineCard GetRandomPunchline()
    {
        return punchlineCards[Random.Range(0, punchlineCards.Count)];
    }
}
