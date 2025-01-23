using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GameplayManager : Singleton<GameplayManager>
{
    public enum Turn
    {
        Player,
        Enemies
    }

    public Turn m_currentTurn;
    [HideInInspector] public GameObject Player;
    public Transform startPosition;
    public Transform entityStartingPosition;
    private float yOffset;
    public Step playerPosition;

    private GameObject myMap;
    public Collector myCollector;
    public List<Step> StepsInMap;
    public LayerMask InteractableLayer;
    public float AnimationMoveDuration;

    public List<Entity> m_foundEntities;

    public AudioClip movementClip;
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

    private bool CheckValidConnection(Step step)
    {
        bool connection = false;
        if (playerPosition.myStepInfo.Connections.Contains(step)){
            connection = true;
        }
        
        return connection;
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
                Step selectedStep = selectedObject.GetComponent<Step>();
                if (CheckValidConnection(selectedStep))
                {
                    AudioManager.instance.PlaySFX(movementClip);
                    MoveTowardsWithDOTween(Player, selectedObject.transform);
                    m_currentTurn = Turn.Enemies;
                    playerPosition = selectedStep;
                }
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

    public void Initialize()
    {
        StepsInMap = myCollector.stepsCollected;

        if(Player != null)
        {
            yOffset = Player.transform.localScale.y;
        }

        if(startPosition != null)
        {
            Player.transform.position = startPosition.position + new Vector3(0, yOffset, 0);
        }

        /*
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
        */
    }

    public void PositionEntities(GameObject entity, Transform position)
    {
        entity.transform.position = position.position + new Vector3(0, yOffset, 0);
    }

    private IEnumerator waitForTime(float time)
    {
        yield return new WaitForSeconds(time);

    }

    private void CheckDownRaycast(Entity entity)
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        if(Physics.Raycast(origin, direction, out RaycastHit hit, 1f, InteractableLayer))
        {
            entity.myPosition = hit.collider.gameObject.GetComponent<Step>();
        }
    }

    private void Update()
    {
        /*
        if (m_currentTurn == Turn.Enemies)
        {
            Debug.Log(m_currentTurn);
            foreach (Entity entity in m_foundEntities)
            {
                if (entity.m_entityBehaviour == Entity.EntityBehaviour.Patrol)
                {
                    Debug.Log(entity.myPosition);
                    if (entity.myPosition != entity.myDirection)
                    {
                        MoveTowardsWithDOTween(entity.gameObject, entity.myDirection.transform);
                        StartCoroutine(waitForTime(AnimationMoveDuration));
                        CheckDownRaycast(entity);    
                    }
                    else 
                    if(entity.myPosition == entity.myDirection)
                    {
                        MoveTowardsWithDOTween(entity.gameObject, entity.myOriginPosition.transform);
                    }
                }
            }
            m_currentTurn = Turn.Player;
            Debug.Log(m_currentTurn);
        }
        */

        if (m_currentTurn == Turn.Enemies)
        {
            StartCoroutine(HandleEnemyTurn());
        }
    }

    private Step GetNextStep(Step currentStep, Step targetStep)
    {
        // Trova il prossimo step tra le connessioni
        foreach (Step connection in currentStep.myStepInfo.Connections)
        {
            // Verifica se questa connessione si avvicina alla destinazione
            if (connection == targetStep || connection.myStepInfo.Connections.Contains(targetStep))
            {
                return connection;
            }
        }

        // Se nessuna connessione valida è trovata, rimani sullo step corrente
        return currentStep;
    }

    private IEnumerator HandleEnemyTurn()
    {
        foreach (Entity entity in m_foundEntities)
        {
            if (entity.m_entityBehaviour == Entity.EntityBehaviour.Patrol)
            {
                // Calcola il prossimo step
                Step nextStep = GetNextStep(entity.myPosition, entity.myDirection);

                // Muovi il nemico verso il prossimo step
                MoveTowardsWithDOTween(entity.gameObject, nextStep.transform);
                yield return new WaitForSeconds(AnimationMoveDuration);

                // Aggiorna la posizione attuale del nemico
                entity.myPosition = nextStep;

                // Controlla se il nemico ha raggiunto la destinazione finale
                if (entity.myPosition == entity.myDirection)
                {
                    // Cambia direzione (ritorna all'origine)
                    entity.myDirection = entity.myOriginPosition;
                    entity.myOriginPosition = nextStep; // Inverti per andare avanti e indietro
                }
            }
        }

        // Passa al turno del giocatore
        m_currentTurn = Turn.Player;
    }
}
