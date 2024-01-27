using UnityEngine;

public class CardManager : MonoBehaviour
{
    private CardParser cardParser = new CardParser();

    void Awake()
    {
        cardParser.ReadFiles();
    }
}
