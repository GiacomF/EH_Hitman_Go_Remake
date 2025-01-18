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

    public Turn m_currentTurn;
    public GameObject Player;
    public Transform startPosition;
    public Transform entityStartingPosition;
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

    private void ExecuteBehaviour(Entity entity)
    {
        /*if(entity.m_entityBehaviour == Entity.EntityBehaviour.Patrol)
        {
            MoveTowardsWithDOTween(entity, )
        }*/
    }

    [SerializeField]
    private Entity m_currentEntity;
    private void CicleThroughEntities()
    {
        int entitiesFound = m_foundEntities.Count;
        for (int i = 0; i < entitiesFound; i++)
        {
            ExecuteBehaviour(m_foundEntities[i]);
        }

        m_currentTurn = Turn.Player;
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
        if(m_currentTurn == Turn.Player)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer))
            {
                GameObject selectedObject = hit.collider.gameObject;

                MoveTowardsWithDOTween(Player, selectedObject.transform);

                m_currentTurn = Turn.Enemies;
            }
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
        if(FindCollector())
        {
            StepsInMap = myCollector.stepsCollected;
        }

        if(Player != null)
        {
            yOffset = Player.transform.localScale.y;
        }

        if(startPosition != null)
        {
            Player.transform.position = startPosition.position + new Vector3(0, yOffset, 0);
        }

        if(m_foundEntities.Count != 0)
        {
            foreach(Entity e in m_foundEntities)
            {
                e.gameObject.transform.position = entityStartingPosition.position + new Vector3(0, yOffset, 0);
            }
        }

        if(m_currentTurn == Turn.Enemies)
        {
            CicleThroughEntities();
        }
    }

    void Start()
    {
        Initialize();
    }
}
