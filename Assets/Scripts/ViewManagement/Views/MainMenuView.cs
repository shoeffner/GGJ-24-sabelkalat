using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _quitButton;
    public override void Initialize()
    {
        _startGameButton.onClick.AddListener(() => {
            GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);

        });
        _settingsButton.onClick.AddListener(() => ViewManager.Instance.Show<SettingsMenuView>());
        _creditsButton.onClick.AddListener(() => ViewManager.Instance.Show<CreditsMenuView>());
        _quitButton.onClick.AddListener(() => ViewManager.Instance.Show<ConfirmExitMenuView>());

    }
}
