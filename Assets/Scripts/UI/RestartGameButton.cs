using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartGameButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        GameManager.Instance.LoadGameScene((int)SceneIndexes.GAME_SCENE_ONE);
    }
}
