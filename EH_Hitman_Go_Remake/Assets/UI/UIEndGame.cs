using UnityEngine.UI;

public class UIEndGame : GameUI
{
    public Button menuButton;
    public Button playAgainButton;
    //public TMP_Text points;
    
    void Start()
    {
        menuButton.onClick.AddListener(OnMenuClick);
        playAgainButton.onClick.AddListener(OnPlayAgainClick);
    }

    public void OnMenuClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.MainMenu);
    }

    public void OnPlayAgainClick()
    {
        GameStateManager.Instance.SetCurrentGameState(GameStateManager.GameStates.Gameplay);
    }
}
