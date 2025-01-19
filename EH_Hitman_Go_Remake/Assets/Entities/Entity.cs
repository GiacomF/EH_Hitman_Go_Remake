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

    public EntityBehaviour m_entityBehaviour;

    private void OnEnable()
    {
        if(GameplayManager.Instance != null)
        {
            GameplayManager.Instance.RegisterEntity(this);
        }
    }
}
