using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : GameUI
{
    //[SerializeField] private GameObject buildingMenuPanel;
    //[SerializeField] private GameObject buildButtonObject;
    [SerializeField] private Button pauseButton;
    //[SerializeField] private TMP_Text buildButtonText;
    //[SerializeField] private TMP_Text hammersText;
    //[SerializeField] private GameObject turretPanelsContainer;
    //[SerializeField] private GameObject turretPanelPrefab;
    //[SerializeField] private TMP_Text timerText;
    //[SerializeField] private TMP_Text pointsText;


    // Start is called before the first frame update
    void Start()
    {
        //buildingMenuPanel.SetActive(false);
        pauseButton.onClick.AddListener(OnPauseButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.isLevelStarted)
        {
            //hammersText.text = LevelManager.Instance.playerInstance.GetComponent<Player>().GetHammersLeft().ToString();
            //buildButtonText.text = BuildManager.Instance.currentCoins.ToString();
            //buildButtonObject.SetActive(BuildManager.Instance.canEnterBuildMode);
            //pointsText.text = LevelManager.Instance.playerPoints.ToString();
            //UpdateTimer();
        }
    }


    /*
    public void CloseBuildingPanel()
    {
        buildingMenuPanel.SetActive(false);
    }
    */

    private void OnPauseButtonClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Pause);
        
        /*
        if (buildingMenuPanel != null)
        {
            buildingMenuPanel.SetActive(!buildingMenuPanel.activeSelf);
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.openBuildingMenu, transform, 1);
        }
        */
        
    }

    /*
    private void UpdateTimer()
    {
        float currentTimeRemaining = LevelManager.Instance.GetCurrentWaveTimer();
        float minutes = Mathf.FloorToInt(currentTimeRemaining / 60);
        float seconds = Mathf.FloorToInt(currentTimeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    */
}
