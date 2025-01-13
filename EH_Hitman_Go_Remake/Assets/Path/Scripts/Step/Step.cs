using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Step : MonoBehaviour
{
    [Serializable]
    public struct StepInfo
    {
        public int myIndex;
        public List<Step> Connections;
        public bool isTrapdoor;
        public Step TrapdoorConnection;
        public bool isMist;
    }

    public StepInfo myStepInfo;
    public GameObject myMistObj;

    private void CheckisMist()
    {
        myMistObj.SetActive(myStepInfo.isMist);
    }

    public void SetTrapdoorConnection(Step connection)
    {
        myStepInfo.TrapdoorConnection = connection;
    }

    private bool CheckisTrapdoor()
    {
        if(myStepInfo.TrapdoorConnection == null)
        {
            myStepInfo.isTrapdoor = false;
        }
        else
        {
            myStepInfo.isTrapdoor = true;
        }
        
        return myStepInfo.isTrapdoor;
    }

    public void UpdateStatus(SceneView sceneView)
    {
        Event e = Event.current;

        CheckisTrapdoor();
        CheckisMist();
    }

    private void OnEnable ()
    {
        SceneView.duringSceneGui += UpdateStatus;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= UpdateStatus;
    }
}
