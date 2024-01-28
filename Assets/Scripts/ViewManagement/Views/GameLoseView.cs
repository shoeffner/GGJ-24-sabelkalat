using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoseView : View
{
    [NotNull]
    public Button restartButton;
    [NotNull]
    public Button quitButton;

    public override void Initialize()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void RestartGame()
    {
        TransitionManager.Instance.TransitionScenes(async () =>
        {
            SceneManager.UnloadSceneAsync((int)SceneIndexes.GAME_SCENE_ONE).completed += (_) =>
            {
                ViewManager.Instance.Show<GameOverlayView>();
                SceneManager.LoadSceneAsync((int)SceneIndexes.GAME_SCENE_ONE, LoadSceneMode.Additive);
            };
        });
    }

    private void QuitGame()
    {
        ViewManager.Instance.Show<ConfirmExitMenuView>();
    }
}
