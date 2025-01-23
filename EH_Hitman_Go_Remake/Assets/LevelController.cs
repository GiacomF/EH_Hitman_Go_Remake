using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Spawns")]
    public Transform playerSpawn;
    
    public void StartLevel()
    {
        LevelManager.Instance.playerInstance = GameObject.Instantiate(LevelManager.Instance.playerPrefab, playerSpawn.position, Quaternion.identity);
        GameplayManager manager = GameObject.FindFirstObjectByType<GameplayManager>();
        manager.Player = LevelManager.Instance.playerInstance;
        //Debug.Log(LevelManager.Instance.playerInstance);
        //LevelManager.Instance.playerPoints = 0;
    }
}
