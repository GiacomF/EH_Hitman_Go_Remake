using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGame : GameUI
{
    public Button menuButton;
    public Button playAgainButton;
    public TMP_Text points;
    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(OnMenuClick);
        playAgainButton.onClick.AddListener(OnPlayAgainClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.MainMenu);
    }

    public void OnPlayAgainClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Gameplay);
    }
}
