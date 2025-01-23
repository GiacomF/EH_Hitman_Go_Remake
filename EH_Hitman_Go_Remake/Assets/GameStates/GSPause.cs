using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSPause : IGameState
{
    public void OnStateEnter() 
    {
        GameManager.Instance.Pause(true);
        UIManager.Instance.ShowUI(UIManager.GameUIType.Pause);


    }

    public void OnStateUpdate() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Gameplay);
        }
    }

    public void OnStateExit() 
    {
        GameManager.Instance.Pause(false);
    }

}
