using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSMainMenu : IGameState
{
    public  void OnStateEnter() 
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.MainMenu);
    }

    public  void OnStateUpdate() { }

    public  void OnStateExit() 
    {
    }
}
