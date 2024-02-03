using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameSetup easy;
    [SerializeField] private GameSetup medium;
    [SerializeField] private GameSetup deadSerious;

    public static bool activeGame { get; set; }
    private GameSetup currentGameSetup;

    protected override void Awake()
    {
        if (HasInstance) return;
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadSceneIfNotActive((int)SceneIndexes.MANAGER);
        LoadSceneIfNotActive((int)SceneIndexes.UI_OVERLAY);
    }

    private void LoadSceneIfNotActive(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex != sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        List<int> noGameScenes = new() { (int)SceneIndexes.MANAGER, (int)SceneIndexes.UI_OVERLAY };

        if (!noGameScenes.Contains(scene.buildIndex))
        {
            activeGame = true;
            SceneManager.SetActiveScene(scene);
            StartCoroutine(AudioManager.Instance.FadeOut(AudioManager.Instance.FindSoundByName("MenuMusic").source, 0.5f));

            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.UI_OVERLAY).isLoaded)
            {
                ViewManager.Instance.SetCameraState(false);
            }
        }
        
        if (scene.buildIndex == (int)SceneIndexes.UI_OVERLAY && !noGameScenes.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            ViewManager.Instance.SetStartingView(ViewManager.Instance.GetView<GameOverlayView>());
            ViewManager.Instance.SetCameraState(false);
            StartCoroutine(AudioManager.Instance.FadeOut(AudioManager.Instance.FindSoundByName("MenuMusic").source, 0.5f));
        }

        if (transform.parent == null) { SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MANAGER)); }
    }

    public void LoadGameScene(int sceneIndex)
    {
        TransitionManager.Instance.TransitionScenes(() =>
        {
            ViewManager.Instance.Show<GameOverlayView>();
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        });
    }

    public void QuitActiveGame(View showView)
    {
        TransitionManager.Instance.TransitionScenes(() =>
        {
            Scene gameScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MANAGER));
            SceneManager.UnloadSceneAsync(gameScene);
            ViewManager.Instance.Show(showView);
            ViewManager.Instance.SetCameraState(true);
            activeGame = false;
            StartCoroutine(AudioManager.Instance.FadeIn(AudioManager.Instance.FindSoundByName("MenuMusic").source, 0.5f));
        });
    }

    public void SetGameSetupEasy()
    {
        currentGameSetup = easy;
    }

    public void SetGameSetupMedium()
    {
        currentGameSetup = medium;
    }
    public void SetGameSetupDeadSerious()
    {
        currentGameSetup = deadSerious;
    }

    public GameSetup GetCurrentGameSetup()
    {
        return currentGameSetup == null ? easy : currentGameSetup;
    }

}
