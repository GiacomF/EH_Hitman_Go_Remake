using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GSEndGame : IGameState
{
    public void OnStateEnter()
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.EndGame);
    }

    public void OnStateUpdate() { }

    public void OnStateExit() { }
}
