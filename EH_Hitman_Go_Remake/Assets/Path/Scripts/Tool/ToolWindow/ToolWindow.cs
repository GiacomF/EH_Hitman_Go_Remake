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

    private bool isEnabled = false;
    public int StepIndex = 0;
    public Step OriginStep = null;
    public Step DestinationStep = null;

    //Into Initialize goes any check to control the possibility of generation, assignement and correct functioning of the Tool

    private GameObject FindGameObjectWithComponent<T>() where T : Component
    {
        T component = FindObjectOfType<T>();
        return component != null ? component.gameObject : null;
    }

    private GameObject StepPrefab;
    private GameObject ConnectionPrefab;
    private GameObject EntityPrefab;
    private void LoadPrefabs()
    {
        StepPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Path/Prefabs/StepPrefab.prefab");
        ConnectionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Path/Prefabs/Connection.prefab");
        EntityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Entities/Prefab/Enemy.prefab");
    }

    private GameObject myMap;
    private GameObject myStepsContainer;
    private GameObject myConnectionsContainer; 
    private Collector myCollector;
    private void Initialize()
    {
        LoadPrefabs();

        myMap = FindGameObjectWithComponent<Collector>();
        Debug.Log(myMap);

        if (myMap != null)
        {
            generationPosition = myMap.transform.position;
            
            myCollector = myMap.GetComponent<Collector>() ?? myMap.AddComponent<Collector>();
            myCollector = myMap.GetComponent<Collector>();

            myStepsContainer = EnsureContainer("StepsContainer");
            myConnectionsContainer = EnsureContainer("ConnectionsContainer");
        }
    }

    private GameObject EnsureContainer(string containerName)
    {
        Transform containerTransform = myMap.transform.Find(containerName);
        
        if (containerTransform == null)
        {
            GameObject container = new GameObject(containerName);
            container.transform.SetParent(myMap.transform);
            return container;
        }
        
        return containerTransform.gameObject;
    }

    public List<GameObject> generatedSteps = new List<GameObject>();
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
        newStepComponent.myMistObj = newStep.transform.Find("Mist").gameObject;
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

    private void SetTrapdoorConnection()
    {
        if(DestinationStep == null)
        {
            OriginStep.myStepInfo.TrapdoorConnection.myStepInfo.TrapdoorConnection = DestinationStep;
            OriginStep.myStepInfo.TrapdoorConnection = DestinationStep;
        }
        else
        {
            OriginStep.myStepInfo.TrapdoorConnection = DestinationStep;
            OriginStep.myStepInfo.TrapdoorConnection.myStepInfo.TrapdoorConnection = OriginStep;
        }
    }

    private bool isMist = false;
    private void SetIsMist()
    {
        isMist = EditorGUILayout.Toggle("Is this node Mist", isMist);
        
        if(OriginStep != null)
        {
            OriginStep.myStepInfo.isMist = isMist;
        }
    } 

    private void OnIsEnabled()
    {
        if(!isEnabled) return;

        SceneView.duringSceneGui += OnSceneGuiInstructions;
        
        GUILayout.Label(promptMessage, EditorStyles.boldLabel);

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
        
        GUILayout.Label("", EditorStyles.boldLabel);
        GUILayout.Label("Connect DestiantionStep using Trapdoor Connection", EditorStyles.boldLabel);
        if(GUILayout.Button("Set Trapdoor Connection"))
        {
            SetTrapdoorConnection();
        }

        SetIsMist();

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

    private void EnableButtonFunc()
    {
        if(GUILayout.Button(isEnabled ? "Tool Off" : "Tool On"))
        {
            isEnabled = !isEnabled;

            //As a safe measure, deactivates the Instructions whatever the case
            SceneView.duringSceneGui -= OnSceneGuiInstructions;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("", EditorStyles.boldLabel);

        OnIsEnabled();

        EnableButtonFunc();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGuiInstructions;
        myCollector = null;
    }
}