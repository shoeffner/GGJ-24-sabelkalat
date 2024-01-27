using UnityEngine;
using UnityEngine.UI;

public class ConfirmExitMenuView : View
{
    [SerializeField] private Button _keepPlayingButton;
    [SerializeField] private Button _exitButton;
    public override void Initialize()
    {
        _exitButton.onClick.AddListener(() => {
            Debug.Log("Exit game");
            Application.Quit();
        });
        _keepPlayingButton.onClick.AddListener(() =>
        {
            ViewManager.Instance.ShowLast();
        });
    }
}
