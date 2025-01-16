using System;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UIManager.GameUIType gameUIType;

    public GameUI()
    {
        
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void SetActive(bool active, Action action = null)
    {
        gameObject.SetActive(active);
    }

    public UIManager.GameUIType GetUIType()
    {
        return gameUIType;
    }

}
