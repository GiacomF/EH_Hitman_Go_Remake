using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Step : MonoBehaviour
{
    [Serializable]
    public struct ViewStepInfo
    {
        public bool isMist;
        public int myIndex;
        public List<Step> Connections;
    }

    public bool isTrapdoor;
    public Step TrapdoorConnection;
    public ViewStepInfo myStepInfo;
    public GameObject myMistObj;
    public Entity myEntity;
    private void CheckisMist()
    {
        myMistObj.SetActive(myStepInfo.isMist);
    }

    public void SetTrapdoorConnection(Step connection)
    {
        TrapdoorConnection = connection;
    }

    private bool CheckisTrapdoor()
    {
        if(TrapdoorConnection == null)
        {
            isTrapdoor = false;
        }
        else
        {
            isTrapdoor = true;
        }
        
        return isTrapdoor;
    }

    public void SetStepInfo(ViewStepInfo newInfo)
    {
        myStepInfo = newInfo;
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
