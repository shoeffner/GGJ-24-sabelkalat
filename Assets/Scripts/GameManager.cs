using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public static bool activeGame { get; set; }

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
        }
        if (!noGameScenes.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            ViewManager.Instance.SetStartingView(ViewManager.Instance.GetView<GameOverlayView>());
            ViewManager.Instance.SetCameraState(false);
        }

        if(transform.parent == null) { SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MANAGER)); }
    }

    public void LoadGameScene(int sceneIndex)
    {
        TransitionManager.Instance.TransitionScenes(() =>
        {
            ViewManager.Instance.Show<GameOverlayView>();
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        });
    }

    public void QuitActiveGame()
    {
        TransitionManager.Instance.TransitionScenes(() =>
        {
            Scene gameScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MANAGER));
            SceneManager.UnloadSceneAsync(gameScene);
            ViewManager.Instance.Show<MainMenuView>();
            ViewManager.Instance.SetCameraState(true);
            activeGame = false;
        });
    }

}
