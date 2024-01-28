using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndOrchestrator : MonoBehaviour
{
    [NotNull]
    public Curtain curtain;
    [NotNull]
    public AudioSource gameEndAudio;

    [Header("Game Lose")]
    public float gameLoseDelay = 1;
    public AudioClip[] loseSounds;
    [Header("Game Win")]
    public float gameWinDelay = 1;
    public AudioClip[] winSounds;


    void Start()
    {
        GameOrganizer.Instance.OnGameWin += OnGameWin;
        GameOrganizer.Instance.OnGameLose += OnGameLose;
    }

    private void OnGameWin()
    {
        Debug.Log("Start game win");
        StartCoroutine(GameWinCoroutine());
    }

    private IEnumerator GameWinCoroutine()
    {

        // ggf. activate view audience

        Debug.Log("Play win sound");
        gameEndAudio.clip = winSounds[Random.Range(0, winSounds.Length)];
        gameEndAudio.loop = false;
        gameEndAudio.Play();

        // Fade out music
        Debug.Log("Fading out music");
        yield return GameOrganizer.Instance.FadeOutMusic();


        yield return new WaitForSeconds(gameWinDelay);
        Debug.Log("Closing curtain");
        curtain.Close();
        yield return new WaitForSeconds(curtain.moveDuration);
        Debug.Log("Showing menu");

        TransitionManager.Instance.TransitionScenes(() =>
        {
            ViewManager.Instance.Show<GameWinView>();
        });
    }

    private void OnGameLose()
    {
        StartCoroutine(GameLoseCoroutine());
    }

    private IEnumerator GameLoseCoroutine()
    {


        Debug.Log("Play lose sound");
        gameEndAudio.clip = loseSounds[Random.Range(0, loseSounds.Length)];
        gameEndAudio.loop = false;
        gameEndAudio.Play();

        // Fade out music
        Debug.Log("Fading out music");
        yield return GameOrganizer.Instance.FadeOutMusic();


        yield return new WaitForSeconds(gameLoseDelay);
        // TODO Fade to black
        Debug.Log("Showing menu");
        TransitionManager.Instance.TransitionScenes(() =>
        {
            ViewManager.Instance.Show<GameWinView>();
        });

    }
}
