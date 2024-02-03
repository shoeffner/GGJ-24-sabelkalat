using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GameOverView : View
{
    [NotNull]
    public Button restartButton;
    [NotNull]
    public Button mainMenuButton;

    public TextMeshProUGUI resultText;
    [NotNull]
    public TextMeshProUGUI scoreText;
    [NotNull]
    public TextMeshProUGUI scoreResultText;

    public override void Initialize()
    {
        restartButton.onClick.AddListener(() => GameManager.Instance.QuitActiveGame(ViewManager.Instance.GetView<LevelSelectView>()));
        mainMenuButton.onClick.AddListener(() => GameManager.Instance.QuitActiveGame(ViewManager.Instance.GetView<MainMenuView>()));
    }

    public void SetResultText(bool win)
    {
        resultText.text = win ? "You survived" : "You died, because your jokes were too bad!";
    }

    public void SetScoreText(int score)
    {
        scoreText.text = "Audience score:\r\n" + score.ToString();
        if (score < -10)
        {
            scoreResultText.text = "You are such a bad comedian! The crowd eats you alive!";
        }
        else if (score < 1)
        {
            scoreResultText.text = "Nobody thinks you are funny!";
        }
        else if (score < 4)
        {
            scoreResultText.text = "Maybe you are funny, but your jokes are not that good!";
        }
        else if (score < 7)
        {
            scoreResultText.text = "The crowd thinks your jokes are ok, but practice a little more.";
        }
        else if (score < 10)
        {
            scoreResultText.text = "You earned some good laughs. The crowd is almost satisfied.";
        }
        else
        {
            scoreResultText.text = "I can't! The audience finds you hilarious!";
        }

    }
}
