using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOrganizer : Singleton<GameOrganizer>
{
    // trigger AddScore method from inspector

    public UnityAction OnGameOver;
    public UnityAction<int> OnScoreChanged;

    public Audience audience;
    [SerializeField] private GameSetup gameSetup;

    [Header("Music")]
    [SerializeField] private AudioSource basicTrack;
    [SerializeField] private AudioSource intenseTrack;
    [SerializeField] private AudioSource moreIntenseTrack;
    [SerializeField] private AudioSource superIntenseTrack;

    [Header("SFX")]
    [SerializeField] private List<AudioClip> goodJokeSounds;
    [SerializeField] private List<AudioClip> deadJokeSounds;
    private int currentIntensity = 0;
    private int deadJokeSeqCounter = 0;
    private int goodJokeSeqCounter = 0;

    private int currentScore = 0;
    private int currentRoundIndex = 0;
    private AudioSource sfxAudioSource;


    private void Start()
    {
        sfxAudioSource = GetComponent<AudioSource>();
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
        basicTrack.Play();
    }

    public void AddScore(int score)
    {
        Debug.Log("Score added: " + score);
        currentScore = Mathf.Clamp(currentScore + score, gameSetup.lowerScoreLimit, gameSetup.upperScoreLimit);
        OnScoreChanged?.Invoke(score);
        PlayReactionSound(score);

        CheckForTrackIntensity(score);
    }

    public int GetCurrentScore() => currentScore;

    public void NextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex >= gameSetup.gameRounds.Count || currentScore < -10)
        {
            OnGameOver?.Invoke();
            return;
        }
        audience.SetNewViewers(gameSetup.gameRounds[currentRoundIndex].viewers);
    }
    private void PlayReactionSound(int score)
    {
        var reactionSound = score > 0 ? goodJokeSounds[Random.Range(0, goodJokeSounds.Count)] : deadJokeSounds[Random.Range(0, deadJokeSounds.Count)];
        sfxAudioSource.clip = reactionSound;
        sfxAudioSource.PlayDelayed(0.5f);
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

        if (deadJokeSeqCounter >= 2)
        {
            deadJokeSeqCounter = 0;
            currentIntensity = Mathf.Clamp(++currentIntensity, 0 ,3);
            if (currentIntensity == 1)
            {
                Debug.Log("intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(intenseTrack, 0.5f));
            }
            else if (currentIntensity == 2)
            {
                Debug.Log("more intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(moreIntenseTrack, 0.5f));
            }
            else if (currentIntensity == 3)
            {
                Debug.Log("super intense Track");
                StartCoroutine(AudioManager.Instance.FadeIn(superIntenseTrack, 0.5f));
            }
        }
        else if (goodJokeSeqCounter >= 2)
        {
            goodJokeSeqCounter = 0;
            currentIntensity = Mathf.Clamp(--currentIntensity, 0, 3);
            if (currentIntensity == 0)
            {
                Debug.Log("basic Track");
                StartCoroutine(AudioManager.Instance.FadeOut(intenseTrack, 0.5f));
            }
            else if (currentIntensity == 1)
            {
                Debug.Log("intense Track");
                StartCoroutine(AudioManager.Instance.FadeOut(moreIntenseTrack, 0.5f));
            }
            else if (currentIntensity == 2)
            {
                Debug.Log("more intense Track");
                StartCoroutine(AudioManager.Instance.FadeOut(superIntenseTrack, 0.5f));
            }
        }
    }
}