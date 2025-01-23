using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GSGameplay : IGameState
{
    public void OnStateEnter() 
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.Gameplay);
        if (!LevelManager.Instance.isLevelStarted)
        {
            LevelManager.Instance.LoadLevel();
        }
    }

    public void OnStateUpdate() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Pause);
        }
        
    }

    public void OnStateExit() { }

}
