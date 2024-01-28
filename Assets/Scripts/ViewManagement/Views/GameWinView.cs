using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinView : View
{
    [NotNull]
    public Button restartButton;
    [NotNull]
    public Button mainMenuButton;

    public override void Initialize()
    {
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
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

    private void MainMenu()
    {
        GameManager.Instance.QuitActiveGame();
    }

    private void QuitGame()
    {
        ViewManager.Instance.Show<ConfirmExitMenuView>();
    }
}
