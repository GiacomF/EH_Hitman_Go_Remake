using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameStateManager.GameStates startingGameState;
   

    private void Awake()
    {
        GameStateManager.Instance.RegisterState(GameStateManager.GameStates.MainMenu, new GSMainMenu());
        GameStateManager.Instance.RegisterState(GameStateManager.GameStates.Gameplay, new GSGameplay());
        GameStateManager.Instance.RegisterState(GameStateManager.GameStates.Pause, new GSPause());
        GameStateManager.Instance.RegisterState(GameStateManager.GameStates.EndGame, new GSEndGame());

    }

    private void Start()
    {
        GameStateManager.Instance.SetCurrentGameState(startingGameState);
    }

    public void GameOver()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.EndGame);
    }

    public void Pause(bool active)
    {
        if (active)
        {
            Time.timeScale = 0f;
        }else
        {
            Time.timeScale = 1f;
        }
    }
}