using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolWindow : EditorWindow
{
    [MenuItem("Tools/PathTool")]
    public static void ShowTool()
    {
        GetWindow<ToolWindow>("Path Tool");
    }

    private string promptMessage = "Hello!";
    private void OnSceneGuiInstructions(SceneView sceneView)
    {
        Event e = Event.current;

        switch (generatedSteps.Count)
        {
            case 0: promptMessage = "Hello! Click Generate Step to Start!"; break;

            case >0: promptMessage = "Connect next Step..."; break;
        }
    }

    public GameObject StepPrefab;
    public List<GameObject> generatedSteps = new List<GameObject>();
    private bool isEnabled = false;
    public int StepIndex = 0;
    public Step OriginStep = null;
    public Step DestinationStep = null;

    //Into Initialize goes any check to control the possibility of generation, assignement and correct functioning of the Tool
    public GameObject myMap;
    private GameObject myStepsContainer;
    private GameObject myConnectionsContainer; 
    private Collector myCollector;
    private void Initialize()
    {
        if (myMap == null) return;  // Verifica se myMap è null, se sì esci dalla funzione

        generationPosition = myMap.transform.position;
        // Aggiungi o ottieni il componente Collector
        myCollector = myMap.GetComponent<Collector>() ?? myMap.AddComponent<Collector>();

        // Gestisci la creazione dei container (StepsContainer e ConnectionsContainer)
        myStepsContainer = EnsureContainer("StepsContainer");
        myConnectionsContainer = EnsureContainer("ConnectionsContainer");
    }

    private GameObject EnsureContainer(string containerName)
    {
        // Cerca il container già esistente
        Transform containerTransform = myMap.transform.Find(containerName);
        
        // Se non esiste, crea un nuovo contenitore
        if (containerTransform == null)
        {
            GameObject container = new GameObject(containerName);
            container.transform.SetParent(myMap.transform);  // Imposta il parent al mio oggetto map
            return container;
        }
        
        return containerTransform.gameObject;  // Restituisci il contenitore esistente
    }

    private Vector3 generationPosition;
    public Vector3 yOffset = new Vector3(0, 0.5f, 0);
    private void CreateStep()
    {
        if(OriginStep != null)
        {
            generationPosition = OriginStep.transform.position;
        }

        GameObject newStep = Instantiate(StepPrefab, generationPosition, Quaternion.identity, myStepsContainer.transform);
        Step newStepComponent = newStep.GetComponent<Step>();
        newStep.name = $"Step {StepIndex}";
        newStepComponent.myStepInfo.myIndex = StepIndex;
            
        generatedSteps.Add(newStep);

        if(generatedSteps.Count >= 2)
        {
            CreateConnection(OriginStep.gameObject, newStep);
        }
 
        myCollector.stepsCollected.Add(newStepComponent);

        OriginStep = newStepComponent;
        StepIndex++;
    }

    public GameObject ConnectionPrefab;
    private List<GameObject> connections = new List<GameObject>();
    private void CreateConnection(GameObject Origin, GameObject Destination)
    {
        GameObject newObj = Instantiate(ConnectionPrefab, myConnectionsContainer.transform);
        connections.Add(newObj);

        Step originStep = Origin.GetComponent<Step>();
        Step destinationStep = Destination.GetComponent<Step>();
        originStep.myStepInfo.Connections.Add(destinationStep);
        destinationStep.myStepInfo.Connections.Add(originStep);

        Connection newConnection = newObj.AddComponent<Connection>();
        newConnection.Initialize(Origin, Destination);
    }

    private Step trapdoorConnection;
    private void SetTrapdoorConnection()
    {
        if(trapdoorConnection == null)
        {
            OriginStep.myStepInfo.TrapdoorConnection.myStepInfo.TrapdoorConnection = trapdoorConnection;
            OriginStep.myStepInfo.TrapdoorConnection = trapdoorConnection;
        }
        else
        {
            OriginStep.myStepInfo.TrapdoorConnection = trapdoorConnection;
            OriginStep.myStepInfo.TrapdoorConnection.myStepInfo.TrapdoorConnection = OriginStep;
        }
    } 

    private void OnIsEnabled()
    {
        
        GUILayout.Label(promptMessage, EditorStyles.boldLabel);

        GUILayout.Label("Step Prefab", EditorStyles.boldLabel);    
        StepPrefab = (GameObject)EditorGUILayout.ObjectField(StepPrefab, typeof(GameObject), true);
        GUILayout.Label("Connection Prefab", EditorStyles.boldLabel);
        ConnectionPrefab = (GameObject)EditorGUILayout.ObjectField(ConnectionPrefab, typeof(GameObject), true);

        GUILayout.Label("Map Selected", EditorStyles.boldLabel);
        myMap = (GameObject)EditorGUILayout.ObjectField(myMap, typeof(GameObject), true);
        GUILayout.Label("Origin Step", EditorStyles.boldLabel);
        OriginStep = (Step)EditorGUILayout.ObjectField(OriginStep, typeof(Step), true);
        GUILayout.Label("Destination Step", EditorStyles.boldLabel);
        DestinationStep = (Step)EditorGUILayout.ObjectField(DestinationStep, typeof(Step), true);
        if(GUILayout.Button("Create Connection"))
        {   
            Initialize();

            if(myCollector != null)
            {
                if(DestinationStep == null)
                {
                    CreateStep();
                }
                else
                {
                    CreateConnection(OriginStep.gameObject, DestinationStep.gameObject);
                }
            }

            DestinationStep = null;
        }

        GUILayout.Label("Generated Steps :", EditorStyles.boldLabel);
        GUILayout.Label(generatedSteps.Count.ToString(), EditorStyles.boldLabel);
        
        GUILayout.Label("Origin Step Info", EditorStyles.boldLabel);
        trapdoorConnection = (Step)EditorGUILayout.ObjectField(trapdoorConnection, typeof(Step), true);
        if(GUILayout.Button("Establish Trapdoor Connection"))
        {
            SetTrapdoorConnection();
        }

        if(GUILayout.Button("Clear"))
        {
            if(generatedSteps.Count != 0)
            {
                foreach(GameObject step in generatedSteps)
                {
                    DestroyImmediate(step);
                }
            }

            foreach(GameObject connection in connections)
            {
                DestroyImmediate(connection);
            }

            if(myCollector != null && myCollector.stepsCollected.Count != 0)
            {
                myCollector.stepsCollected.Clear();
            }

            connections.Clear();
            generatedSteps.Clear();
            StepIndex = 0;
            DestinationStep = null;
            generationPosition = Vector3.zero;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("", EditorStyles.boldLabel);

        if(isEnabled)
        {
            OnIsEnabled();
        }

        if(GUILayout.Button(isEnabled ? "Tool Off" : "Tool On"))
        {
            isEnabled = !isEnabled;
            SceneView.duringSceneGui -= OnSceneGuiInstructions;

            if(isEnabled)
            {
                SceneView.duringSceneGui += OnSceneGuiInstructions;
            }
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGuiInstructions;
        myCollector = null;

        //Destroy Preview
    }
}
