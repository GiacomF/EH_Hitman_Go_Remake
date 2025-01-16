public interface IGameUI
{
    public void Init();

    public void SetActive(bool active);

    public UIManager.GameUIType GetUIType();
}