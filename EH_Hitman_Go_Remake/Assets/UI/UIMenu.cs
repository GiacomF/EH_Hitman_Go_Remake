using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : GameUI
{
    public Button startButton;
    public Button optionButton;
    public Button quitGame;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        optionButton.onClick.AddListener(OnOptionClick);
        quitGame.onClick.AddListener(OnQuitClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Gameplay);
    }


    public void OnOptionClick()
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.Options);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
