using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Connection: MonoBehaviour
{
    GameObject Origin;
    GameObject Destination;

    public void Initialize (GameObject origin, GameObject destination)
    {
        Origin = origin;
        Destination = destination;
    }

    private void UpdateConnection(SceneView sceneView)
    {
        Event e = Event.current;
        if(Origin != null || Destination != null)
        {
            Vector3 positionA = Origin.transform.position;
            Vector3 positionB = Destination.transform.position;

            //Get median point
            Vector3 position = (positionA + positionB) / 2;
            //Get direction
            Vector3 direction = positionB - positionA;
            //Get rotation
            Quaternion rotation = Quaternion.LookRotation(direction);
            //Get distance
            float distance = Vector3.Distance(positionA, position);

            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
        }
    }

    private void OnEnable ()
    {
        SceneView.duringSceneGui += UpdateConnection;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui += UpdateConnection;
    }
}