using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Makes the position readable
    public Vector3 Coords;
    //Holds all the Node connections
    public List<Node> ConnectedNodes;
    //Contains all special functions of the Node
    public List<NodeDecorator> Decorators;

    private void OnEnable()
    {
        if (ConnectedNodes == null)
        {
            ConnectedNodes = new List<Node>();
        }
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

    public (Vector3, List<Node>, List<NodeDecorator>) RegisterIntoManager()
    {
        return (Coords, ConnectedNodes, Decorators);
    }
}
