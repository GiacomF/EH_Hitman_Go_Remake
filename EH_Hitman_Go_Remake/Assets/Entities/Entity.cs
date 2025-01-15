using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityBehaviour
    {
        Guard,
        Sentinel,
        Patrol,
        Dog
    }

    public EntityBehaviour m_entityBehaviour;

    private void OnEnable()
    {
        if(GameplayManager.Instance != null)
        {
            GameplayManager.Instance.RegisterEntity(this);
        }
    }
}
