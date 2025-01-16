using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    [Header("Level Parameters")]
    public GameObject levelPrefab;
    [HideInInspector] public LevelController levelControllerInstance;
    [HideInInspector] public GameObject levelInstance;

    [Header("Internal Variables")]
    [HideInInspector] public bool isLevelStarted = false;
    [HideInInspector] public int playerPoints = 0;
    [HideInInspector] public List<GameObject> coins = new List<GameObject>();

    [Header("Level Objects")]
    public GameObject playerPrefab;


    [Header("Level Objects Instancies")]
    public GameObject playerInstance;


    public void LoadLevel()
    {
        levelInstance = GameObject.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        levelInstance.SetActive(true);
        levelControllerInstance = levelInstance.GetComponent<LevelController>();
        levelControllerInstance.StartLevel();
        isLevelStarted = true;
        //playerInstance.GetComponent<Player>().GetHammersLeft();
    }

    public void DestroyCurrentLevel()
    {
        Debug.Log("Destroy");
        levelInstance.SetActive(false);
        GameObject.Destroy(levelInstance);
        levelInstance = null;
        levelControllerInstance = null;
        isLevelStarted = false;
    }

    public void AddPointsToPlayer(int points)
    {
        playerPoints += points;
    }

    public void AddCoinToCoins(GameObject coin)
    {
        coins.Add(coin);
    }
    
    public void DestroyAllCoins()
    {
        foreach (GameObject coin in coins)
        {
            Destroy(coin);
        }
    }
}
