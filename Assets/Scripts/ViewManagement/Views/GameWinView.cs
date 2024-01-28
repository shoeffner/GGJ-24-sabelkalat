using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinView : View
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
        GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);
    }

    private void QuitGame()
    {
        ViewManager.Instance.Show<ConfirmExitMenuView>();
    }
}
