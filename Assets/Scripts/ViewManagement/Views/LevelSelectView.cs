using UnityEngine;
using UnityEngine.UI;

public class LevelSelectView : View
{
    [SerializeField, NotNull] private Button _backButton;
    [SerializeField, NotNull] private Button _basicLevelButton;
    [SerializeField, NotNull] private Button _mediumLevelButton;
    [SerializeField, NotNull] private Button _difficultLevelButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.Instance.Show<MainMenuView>());
        _basicLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameSetupEasy();
            GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);
        });
        _mediumLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameSetupMedium();
            GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);
        });
        _difficultLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameSetupDeadSerious();
            GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);
        });
    }
}
