using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : View
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    public override void Initialize()
    {
        _continueButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            ViewManager.Instance.Show<GameOverlayView>();
        });
        _settingsButton.onClick.AddListener(() => ViewManager.Instance.Show<SettingsMenuView>());
        _mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.QuitActiveGame(ViewManager.Instance.GetView<MainMenuView>());
            Time.timeScale = 1;
        });
        _quitButton.onClick.AddListener(() => ViewManager.Instance.Show<ConfirmExitMenuView>());
    }

    private void Update()
    {
        //Unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            ViewManager.Instance.Show<GameOverlayView>();
        }
    }

}
