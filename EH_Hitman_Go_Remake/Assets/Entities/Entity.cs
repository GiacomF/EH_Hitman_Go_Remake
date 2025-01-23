using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityBehaviour
    {
        None,
        Player,
        Guard,
        Sentinel,
        Patrol,
        Dog
    }
    public Step myDirection;
    public Step myPosition;
    public Step myOriginPosition;

    public EntityBehaviour m_entityBehaviour;

    private void OnEnable()
    {
        if(GameplayManager.Instance != null)
        {
            GameplayManager.Instance.RegisterEntity(this);
        }
    }
}
