using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSOptions : IGameState
{
    public void OnStateEnter()
    {
       
        GameManager.Instance.Pause(true);
    }

    public void OnStateUpdate() { }

    public void OnStateExit() 
    {
        GameManager.Instance.Pause(false);

    }

}
