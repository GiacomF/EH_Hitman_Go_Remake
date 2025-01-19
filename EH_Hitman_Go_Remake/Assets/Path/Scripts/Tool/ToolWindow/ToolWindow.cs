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

            case >0: promptMessage = "Generate new Step or drag a Step into Destination Step to connect existing nodes"; break;
        }
    }

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
    
    public Step OriginStep = null;
    public int StepIndex = 0;
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

    private void ActivateInstructions()
    {
        SceneView.duringSceneGui += OnSceneGuiInstructions;
        GUILayout.Label(promptMessage, EditorStyles.boldLabel);
    }

    private void PlaceMenuSpacer()
    {
        GUILayout.Label("", EditorStyles.boldLabel);
    }

    private void UpdateOriginAndDestination()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Origin Step", EditorStyles.boldLabel);
        OriginStep = (Step)EditorGUILayout.ObjectField(OriginStep, typeof(Step), true);

        GUILayout.Label("Destination Step", EditorStyles.boldLabel);
        DestinationStep = (Step)EditorGUILayout.ObjectField(DestinationStep, typeof(Step), true);

        PlaceMenuSpacer();

        GUILayout.Label("Origin is data source, Destination is operations subject", EditorStyles.wordWrappedLabel);
        
        GUILayout.EndVertical();
    }

    private void CreateConnectionButtonFunc()
    {
        if(GUILayout.Button("Create Connection"))
        {   
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
    }

    private void TrackGenerationIndex()
    {
        GUILayout.Label("Generated Steps :", EditorStyles.boldLabel);
        GUILayout.Label(generatedSteps.Count.ToString(), EditorStyles.boldLabel);
    }

    private void SetIsMist()
    {
        if(GUILayout.Button("Place Mist"))
        {
            OriginStep.myStepInfo.isMist = !OriginStep.myStepInfo.isMist;
        }
    }
    
    public Step DestinationStep = null;
    private void SetTrapdoorConnection()
    {
        if(DestinationStep == null)
        {
            OriginStep.TrapdoorConnection.TrapdoorConnection = DestinationStep;
            OriginStep.TrapdoorConnection = DestinationStep;
        }
        else
        {
            OriginStep.TrapdoorConnection = DestinationStep;
            OriginStep.TrapdoorConnection.TrapdoorConnection = OriginStep;
        }
    }

    private void TrapdoorButtonFunc()
    {
        GUILayout.Label("Connect DestiantionStep using Trapdoor Connection", EditorStyles.boldLabel);
        if(GUILayout.Button("Set Trapdoor Connection"))
        {
            SetTrapdoorConnection();
        }
    }

    private void SetEntityDirection()
    {
        if(OriginStep.myStepInfo.Connections.Count != 0)
        {
            if(OriginStep.myEntityDirection == null)
            {
                OriginStep.myEntityDirection = OriginStep.myStepInfo.Connections[0];
            } 
            else
            {
                int currentIndex = OriginStep.myStepInfo.Connections.IndexOf(OriginStep.myEntityDirection);

                if (currentIndex == -1)
                {
                    OriginStep.myEntityDirection = OriginStep.myStepInfo.Connections[0];
                }
                else
                {
                    int nextIndex = (currentIndex + 1) % OriginStep.myStepInfo.Connections.Count;
                    OriginStep.myEntityDirection = OriginStep.myStepInfo.Connections[nextIndex];
                }
            }
        }
    }

    public Entity.EntityBehaviour myBehaviourSet;
    private void SetEntityButtonFunc()
    {
        if(GUILayout.Button("Set Entity"))
        {
            if(myBehaviourSet == Entity.EntityBehaviour.None)
            {
                OriginStep.myEntityBehaviour = Entity.EntityBehaviour.None; OriginStep.myEntityDirection = null;
            }
            else
            {
                OriginStep.myEntityBehaviour = myBehaviourSet;
                SetEntityDirection();
            } 
        }
        myBehaviourSet = (Entity.EntityBehaviour)EditorGUILayout.EnumPopup(myBehaviourSet);
    }

    SerializedObject serializedStep;
    private void ViewStepInfo()
    {
        if(OriginStep == null) return;

        serializedStep = new SerializedObject(OriginStep);

        serializedStep.Update(); 
        GUILayout.Label("View Origin Step Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedStep.FindProperty("myStepInfo"), true); 
        serializedStep.ApplyModifiedProperties(); 
    }

    private void ResetTool()
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

    private void ResetButtonFunc()
    {
        if(GUILayout.Button("ResetTool"))
        {
            ResetTool();
        }
    }

    private bool isEnabled = false;
    private void OnIsEnabled()
    {
        if(!isEnabled) return;

        Initialize();

        ActivateInstructions();

        //Menu spacer
        PlaceMenuSpacer();

        GUILayout.BeginHorizontal();

        UpdateOriginAndDestination();

        GUILayout.BeginVertical("box");

        TrackGenerationIndex();

        CreateConnectionButtonFunc();

        //Menu spacer
        PlaceMenuSpacer();

        /*ViewStepInfo();*/
        SetIsMist();

        TrapdoorButtonFunc();

        SetEntityButtonFunc();

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        ResetButtonFunc();
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
        OnIsEnabled();

        EnableButtonFunc();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGuiInstructions;
        myCollector = null;
    }
}