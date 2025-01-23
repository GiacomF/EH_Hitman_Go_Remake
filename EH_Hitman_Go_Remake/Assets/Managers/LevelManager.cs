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
    //[HideInInspector] public int playerPoints = 0;
    //[HideInInspector] public List<GameObject> coins = new List<GameObject>();

    [Header("Level Objects")]
    public GameObject playerPrefab;
    public GameObject entityPrefab;


    [Header("Level Objects Instancies")]
    public GameObject playerInstance;

    [Header("Spawns")]
    public Transform playerSpawn;


    public void LoadLevel()
    {
        levelInstance = GameObject.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        Collector co = GetLevelCollector(levelInstance);
        GameplayManager gm = GetGameplayManager();
        playerSpawn = co.startPosition.transform;
        gm.myCollector = co;
        gm.startPosition = co.startPosition.transform;
        gm.Player = GameObject.Instantiate(LevelManager.Instance.playerPrefab, playerSpawn.position, Quaternion.identity);
        gm.playerPosition = co.startPosition;
        gm.Initialize();
        SpawnEnemies(co, gm);
        levelInstance.SetActive(true);
        //levelControllerInstance = levelInstance.GetComponent<LevelController>();
        //levelControllerInstance.StartLevel();
        isLevelStarted = true;
        //playerInstance.GetComponent<Player>().GetHammersLeft();
    }

    public void SpawnEnemies(Collector co, GameplayManager gm)
    {
        foreach (Step step in co.stepsCollected)
        {
            if(step.myEntityBehaviour == Entity.EntityBehaviour.Patrol)
            {
                GameObject entity = Instantiate(entityPrefab);    
                Entity script = entity.GetComponent<Entity>();
                script.m_entityBehaviour = step.myEntityBehaviour;
                script.myDirection = step.myEntityDirection;
                script.myPosition = step;
                script.myOriginPosition = step;
                gm.PositionEntities(entity, step.gameObject.transform);
            }
        }    
    }

    public GameplayManager GetGameplayManager()
    {
        GameplayManager gm = GameObject.FindFirstObjectByType<GameplayManager>();
        return gm;
    }

    public Collector GetLevelCollector(GameObject level)
    {
        Collector c = level.GetComponent<Collector>();
        //playerInstance = GameObject.Instantiate(LevelManager.Instance.playerPrefab, playerSpawn.position, Quaternion.identity);
        //GameplayManager manager = GameObject.FindFirstObjectByType<GameplayManager>();
        //manager.Player = LevelManager.Instance.playerInstance;
        return c;
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

    /*
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
    */
}
