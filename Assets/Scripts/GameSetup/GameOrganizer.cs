using UnityEngine;

public class GameOrganizer : Singleton<GameOrganizer>
{
    [SerializeField] private GameSetup gameSetup;
    [SerializeField] private Audience audience;

    private int currentRoundIndex = 0;

    private void Start()
    {
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

    public void NextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex >= gameSetup.gameRounds.Count)
        {
            Debug.Log("Game Over");
            return;
        }
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

}