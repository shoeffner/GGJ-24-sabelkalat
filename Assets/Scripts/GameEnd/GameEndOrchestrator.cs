using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndOrchestrator : MonoBehaviour
{
    [NotNull]
    public Curtain curtain;

    [Header("Game Lose")]
    public float gameLoseDelay = 1;
    [Header("Game Win")]
    public float gameWinDelay = 1;


    void Start()
    {
        GameOrganizer.Instance.OnGameWin += OnGameWin;
        GameOrganizer.Instance.OnGameLose += OnGameLose;
    }

    private void OnGameWin()
    {
        StartCoroutine(GameWinCoroutine());
    }

    private IEnumerator GameWinCoroutine()
    {
        Debug.Log("asdf");
        yield return new WaitForSeconds(gameWinDelay);
        Debug.Log("tet");
        curtain.Close();
    }

    private void OnGameLose()
    {
        StartCoroutine(GameLoseCoroutine());
        ViewManager.Instance.Show<GameWinView>();
    }

    private IEnumerator GameLoseCoroutine()
    {

        Debug.Log("lose");
        yield return new WaitForSeconds(gameWinDelay);
        Debug.Log("tet");
        ViewManager.Instance.Show<GameWinView>();
    }
}
