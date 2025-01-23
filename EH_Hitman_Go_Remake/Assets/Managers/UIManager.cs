using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public enum GameUIType
    {
        NONE,
        MainMenu,
        Gameplay,
        Bindings,
        Options,
        EndGame,
        Pause,
    }
    private Dictionary<GameUIType, GameUI> registeredUIs = new Dictionary<GameUIType, GameUI>();
    private GameUI currentUI = null;
    public Transform UIContainer; // riferimento al canva che contiene le UI

    private void Awake()
    {
        foreach (GameUI enumeratedUI in UIContainer.GetComponentsInChildren<GameUI>(true))
        {
            RegisterUI(enumeratedUI.GetUIType(), enumeratedUI);
        }
        ShowUI(GameUIType.NONE);
    }

    public void RegisterUI(GameUIType UIType, GameUI UIToRegister)
    {
        registeredUIs.Add(UIType, UIToRegister);
        UIToRegister.Init();
    }

    public void ShowUI(GameUIType UIType)
    {
        if (currentUI == null)
        {

            foreach (KeyValuePair<GameUIType, GameUI> kvp in registeredUIs)
            {
                if (UIType == GameUIType.Options)
                {
                    if (GameStateManager.Instance.currentGameStateName == GameStateManager.GameStates.MainMenu)
                    {
                        kvp.Value.SetActive(kvp.Key == UIType, MainMenuReturn);
                    }
                    else if (GameStateManager.Instance.currentGameStateName == GameStateManager.GameStates.Pause)
                    {
                        kvp.Value.SetActive(kvp.Key == UIType, PauseReturn);
                    }
                }
                else
                {
                    kvp.Value.SetActive(kvp.Key == UIType);
                }
            }
        }
        else
        {
            registeredUIs[currentUI.gameUIType].SetActive(false);
            registeredUIs[UIType].SetActive(true);
            currentUI = registeredUIs[UIType];

        }
    }

    #region Options Return Variant

    public void MainMenuReturn()
    {
        ShowUI(GameUIType.MainMenu);
    }

    public void PauseReturn()
    {
        ShowUI(GameUIType.Pause);
    }

    #endregion

    public void ResetCurrentUI()
    {
        currentUI = null;
    }
}
