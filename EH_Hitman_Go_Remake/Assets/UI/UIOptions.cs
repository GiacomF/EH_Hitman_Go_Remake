using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIOptions : GameUI
{
    [SerializeField] private Button returnButton;

    [SerializeField] private GameObject musicPanel;
    [SerializeField] private GameObject sfxPanel;

    private Action returnFunc;

    protected void Start()
    {
        returnButton.onClick.AddListener(OnReturnClick);
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

    public void OnMusicVolumeChange(float value)
    {
        //SoundMixerManager.Instance.SetMusicVolume(value);
    }

    public void OnSoundFXVolumeChange(float value)
    {
        //SoundMixerManager.Instance.SetSoundFXVolume(value);
    }

}
