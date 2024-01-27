using UnityEngine;
using UnityEngine.Events;

public class GameOrganizer : Singleton<GameOrganizer>
{
    public UnityAction OnGameOver;
    public UnityAction<int> OnScoreChanged;

    [SerializeField] private GameSetup gameSetup;
    [SerializeField] public Audience audience;
    public int CurrentScore => currentScore;
    private int currentScore = 0;

    private int currentRoundIndex = 0;

    private void Start()
    {
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

    public void AddScore(int score)
    {
        currentScore = Mathf.Clamp(currentScore + score, gameSetup.lowerScoreLimit, gameSetup.upperScoreLimit);
        if (OnScoreChanged != null)
        {
            OnScoreChanged.Invoke(score);
        }
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