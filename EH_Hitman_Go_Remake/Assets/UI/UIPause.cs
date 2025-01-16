using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : GameUI
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button titleScreenButton;

    protected void Start()
    {
        optionsButton?.onClick.AddListener(OnOptionsClick);
        titleScreenButton?.onClick.AddListener(OnTitleScreenClick);
        resumeButton?.onClick.AddListener(ResumGameplay);

    }

    private void OnOptionsClick()
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.Options);
    }

    private void OnTitleScreenClick()
    {
        GameManager.Instance.GameOver();
    }

    private void ResumGameplay()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Gameplay);
    }
}
