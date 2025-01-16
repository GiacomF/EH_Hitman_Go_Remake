using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIOptions : GameUI
{

    [SerializeField] private Button returnButton;
    [SerializeField] private Button bindingsButton;

    [SerializeField] private GameObject musicPanel;
    [SerializeField] private GameObject sfxPanel;

    private Action returnFunc;

    protected void Start()
    {
        returnButton.onClick.AddListener(OnReturnClick);
        bindingsButton.onClick.AddListener(OnBindingsClick);

    }

    void Update()
    {
        
    }

    public override void SetActive(bool active, Action action = null)
    {
        base.SetActive(active, action);
        if (action != null) 
        {
            returnFunc = action;
        }
    }
    private void OnReturnClick()
    {
        returnFunc?.Invoke();
    }

    private void OnBindingsClick()
    {
        UIManager.Instance.ShowUI(UIManager.GameUIType.Bindings);
    }

}
