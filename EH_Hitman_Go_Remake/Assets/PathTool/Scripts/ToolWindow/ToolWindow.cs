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

    public GameObject ConnectionPrefab;
    private List<GameObject> connections;
    private Vector3 generationPosition = Vector3.zero;
    private void CreateStep()
    {
        if(OriginStep != null)
        {
            generationPosition = OriginStep.transform.position;
        }

        GameObject newStep = Instantiate(StepPrefab, generationPosition, Quaternion.identity);
        Step newStepComponent = newStep.GetComponent<Step>();
        newStep.name = $"Step {StepIndex}";
            
        generatedSteps.Add(newStep);

        if(generatedSteps.Count >= 2)
        {
            CreateConnection(OriginStep.gameObject, newStep);
        }

        OriginStep = newStepComponent;
        StepIndex++;
    }

    private void CreateConnection(GameObject Origin, GameObject Destination)
    {
        GameObject newObj = Instantiate(ConnectionPrefab);
        connections.Add(newObj);

        Step originStep = Origin.GetComponent<Step>();
        Step destinationStep = Destination.GetComponent<Step>();
        originStep.Connections.Add(destinationStep);
        destinationStep.Connections.Add(originStep);

        Connection newConnection = newObj.AddComponent<Connection>();
        newConnection.Initialize(Origin, Destination);
    } 

    private void OnIsEnabled()
    {
        
        GUILayout.Label(promptMessage, EditorStyles.boldLabel);
            
        StepPrefab = (GameObject)EditorGUILayout.ObjectField(StepPrefab, typeof(GameObject), true);
        ConnectionPrefab = (GameObject)EditorGUILayout.ObjectField(ConnectionPrefab, typeof(GameObject), true);

        GUILayout.Label("Origin Step", EditorStyles.boldLabel);
        OriginStep = (Step)EditorGUILayout.ObjectField(OriginStep, typeof(Step), true);
        GUILayout.Label("Destination Step", EditorStyles.boldLabel);
        DestinationStep = (Step)EditorGUILayout.ObjectField(DestinationStep, typeof(Step), true);
        if(GUILayout.Button("Create Connection"))
        {   
            if(DestinationStep == null)
            {
                CreateStep();
            }
            else
            {
                CreateConnection(OriginStep.gameObject, DestinationStep.gameObject);
            }

            DestinationStep = null;
        }

        GUILayout.Label("Generated Steps :", EditorStyles.boldLabel);
        GUILayout.Label(generatedSteps.Count.ToString(), EditorStyles.boldLabel);
        if(GUILayout.Button("Clear List"))
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

            connections.Clear();
            generatedSteps.Clear();
            StepIndex = 1;
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

        //Destroy Preview
    }
}
