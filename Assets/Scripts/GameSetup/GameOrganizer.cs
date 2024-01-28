using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOrganizer : Singleton<GameOrganizer>
{
    public UnityAction OnNextRound;
    public UnityAction OnGameWin;
    public UnityAction OnGameLose;
    public UnityAction<int> OnScoreChanged;

    public Audience audience;
    [SerializeField] private GameSetup gameSetup;
    [SerializeField] private float timeBetweenRounds = 4f;
    [SerializeField] private float switchIntensityThreshhold = 1;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    [Header("Music")]
    [SerializeField] private AudioSource basicTrack;
    [SerializeField] private AudioSource intenseTrack;
    [SerializeField] private AudioSource moreIntenseTrack;
    [SerializeField] private AudioSource superIntenseTrack;

    private int currentIntensity = 0;
    private int deadJokeSeqCounter = 0;
    private int goodJokeSeqCounter = 0;

    private int currentScore = 0;
    private int currentRoundIndex = 0;


    private void Start()
    {
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
        basicTrack.Play();
    }

    public void AddScore(int score)
    {
        Debug.Log("Score added: " + score);
        foreach (Viewer viewer in audience.GetViewers())
        {
            viewer.ChangesCategory = false;
        }
        currentScore = Mathf.Clamp(currentScore + score, gameSetup.lowerScoreLimit, gameSetup.upperScoreLimit);
        OnScoreChanged?.Invoke(score);

        CheckForTrackIntensity(score);
        StartCoroutine(NextRoundTrigger());
    }

    private IEnumerator NextRoundTrigger()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
        NextRound();
    }

    public int GetCurrentScore() => currentScore;

    public void NextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex >= gameSetup.gameRounds.Count || currentIntensity == 4)
        {
            OnGameWin?.Invoke();
            return;
        }
        if (currentScore < -10)
        {
            OnGameLose?.Invoke();
            return;
        }
        OnNextRound?.Invoke();
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }

    // Every 2 dead jokes in a row, the track gets more intense
    // Every 2 good jokes in a row, the track gets less intense
    private void CheckForTrackIntensity(int score)
    {
        if (score > 0)
        {
            goodJokeSeqCounter++;
            deadJokeSeqCounter = 0;
        }
        else
        {
            deadJokeSeqCounter++;
            goodJokeSeqCounter = 0;
        }

        if (deadJokeSeqCounter >= switchIntensityThreshhold)
        {
            deadJokeSeqCounter = 0;
            currentIntensity = Mathf.Clamp(++currentIntensity, 0, 4);
            if (currentIntensity == 1)
            {
                Debug.Log("intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(intenseTrack, fadeInTime));
            }
            else if (currentIntensity == 2)
            {
                Debug.Log("more intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(moreIntenseTrack, fadeInTime));
            }
            else if (currentIntensity == 3)
            {
                Debug.Log("super intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(superIntenseTrack, fadeInTime));
            }
        }
        else if (goodJokeSeqCounter >= switchIntensityThreshhold)
        {
            goodJokeSeqCounter = 0;
            currentIntensity = Mathf.Clamp(--currentIntensity, 0, 4);
            if (currentIntensity == 0)
            {
                Debug.Log("basic Track");
                StartCoroutine(AudioManager.Instance.FadeOut(intenseTrack, fadeOutTime));
            }
            else if (currentIntensity == 1)
            {
                Debug.Log("intense Track");
                StartCoroutine(AudioManager.Instance.FadeOut(moreIntenseTrack, fadeOutTime));
            }
            else if (currentIntensity == 2)
            {
                Debug.Log("more intense Track");
                StartCoroutine(AudioManager.Instance.FadeOut(superIntenseTrack, fadeOutTime));
            }
        }
    }
}
