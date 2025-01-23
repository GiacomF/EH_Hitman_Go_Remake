using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    Dictionary<GameStates, IGameState> registeredGameStates = new Dictionary<GameStates, IGameState>();
    public enum GameStates
    {
        MainMenu,
        Gameplay,
        EndGame,
        Pause,
    }

    public IGameState currentGameState = null;
    public GameStates currentGameStateName = GameStates.MainMenu;

    public void RegisterState(GameStates gstate, IGameState state)
    {
        registeredGameStates.Add(gstate, state);

    }
    public void SetCurrentGameState(GameStates gstate)
    {
        if (currentGameState != null)
        {
            currentGameState.OnStateExit();
        }
        IGameState newState = registeredGameStates[gstate];
        newState.OnStateEnter();
        currentGameState = newState;
        currentGameStateName = gstate;
    }

    void Update()
    {
        currentGameState?.OnStateUpdate();
    }
}
