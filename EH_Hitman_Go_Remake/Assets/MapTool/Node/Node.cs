using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 Coords;
    public List<Node> ConnectedNodes;

    private void OnEnable()
    {
        ConnectedNodes = new List<Node>();
        Coords = transform.position;
    }

    public void SetCoords(Vector3 newCoords)
    {
        Coords = newCoords;
    }
    public Vector3 GetCoords()
    {
        return Coords;
    }

    public List<Node> GetConnections()
    {
        return ConnectedNodes;
    }

    private void RegisterIntoManager()
    {

    }
}
