using UnityEngine;
using UnityEngine.Events;

public class GameOrganizer : Singleton<GameOrganizer>
{
    public UnityAction OnGameOver;

    [SerializeField] private GameSetup gameSetup;
    [SerializeField] private Audience audience;
    private int currentScore = 0;

    private int currentRoundIndex = 0;

    private void Start()
    {
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

    public void AddScore(int score)
    {
        currentScore = Mathf.Clamp(currentScore + score, gameSetup.lowerScoreLimit, gameSetup.upperScoreLimit);
    }

    public int GetCurrentScore() => currentScore;

    public void NextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex >= gameSetup.gameRounds.Count)
        {
            OnGameOver?.Invoke();
            return;
        }
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

}