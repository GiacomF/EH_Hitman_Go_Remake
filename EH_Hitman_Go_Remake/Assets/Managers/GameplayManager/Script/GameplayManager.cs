using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameplayManager : Singleton<GameplayManager>
{
    public enum Turn
    {
        Player,
        Enemies
    }

    public GameObject Player;
    public Transform startPosition;
    private float yOffset;

    private GameObject myMap;
    private Collector myCollector;
    public List<Step> StepsInMap;
    public LayerMask InteractableLayer;
    public float AnimationMoveDuration;

    public List<Entity> m_foundEntities;
    public void RegisterEntity(Entity entity)
    {
        m_foundEntities.Add(entity);
    }

    public void UnregisterEntity(Entity entity)
    {
        m_foundEntities.Remove(entity);
    }

    [SerializeField]
    private Entity m_currentEntity;
    private void CicleThroughEntities()
    {

    }

    private void RotateEntity(GameObject entity, Transform destination)
    {
        
    }

    private void MoveTowardsWithDOTween(GameObject entity, Transform destination)
    {
        Vector3 calibratedDestination = new Vector3(destination.position.x, destination.position.y + yOffset, destination.position.z);
        entity.transform.DOMove(calibratedDestination, AnimationMoveDuration);
    }

    public void OnInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer))
        {
            GameObject selectedObject = hit.collider.gameObject;

            MoveTowardsWithDOTween(Player, selectedObject.transform);
        }
    }

    private bool FindCollector()
    {
        bool collectorFound;

        myCollector = FindAnyObjectByType<Collector>();

        if(myCollector != null)
        {
            collectorFound = true;
            myMap = myCollector.gameObject;
        }
        else
        {
            collectorFound = false;
        }

        return collectorFound;
    }

    private void Initialize()
    {
        if(Player != null)
        {
            yOffset = Player.transform.localScale.y;
        }

        if(startPosition != null)
        {
            Player.transform.position = startPosition.position + new Vector3(0, yOffset, 0);
        }

        if(FindCollector())
        {
            StepsInMap = myCollector.stepsCollected;
        }
    }

    void Start()
    {
        Initialize();
    }
}
