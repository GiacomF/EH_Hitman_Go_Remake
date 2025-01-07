using System;
using System.Collections.Generic;
using UnityEngine;

//This code will create a functioning Grid upon any Parent Object under which this object is placed
public class MapGenerator : MonoBehaviour
{
    //Struct that synthesizes and makes available all informations from objectToMap
    [Serializable]
    struct objectToMap
    {
        public Vector3 parentSize;

        public objectToMap(Vector3 size)
        {
            parentSize = size;
        }

        public Vector3 GetSize()
        {
            return parentSize;
        }
    }

    //Struct that synthesizes and makes available all informations from Grid
    [Serializable]
    struct Grid
    {
        public Vector2 XY;
        public Vector3 gridOrigin;

        public Grid(Vector2 xy, Vector3 origin)
        {
            XY = xy;
            gridOrigin = origin; 
        }

        public Vector3 GetPosition()
        {
            return gridOrigin;
        }
    }

    //Method to Map the objToMap and create a Grid on top
    objectToMap objToMap;
    [SerializeField] Grid myGrid;
    public int Spacing = 1;
    public GameObject NodePrefab;
    void GenerateGrid()
    {
        if(NodePrefab.GetComponent<Node>() == null)
        {
            Debug.LogWarning("Prefab assigned doesn't contain Node script");
            return;
        }

        Transform parentObject = gameObject.transform.parent;

        //Using the Local Scale as measures assures correct position
        objToMap = new objectToMap(parentObject.localScale);

        //Moving this transform to the gridOrigin for simpler nodes distribution
        myGrid = new Grid(CalculateDimensions(), FindOriginLocation());
        transform.position = myGrid.GetPosition();

        for(int i= 0; i <= myGrid.XY.x; i += 1 * Spacing)
        {
            for(int z = 0; z <= myGrid.XY.y; z += 1 * Spacing)
            {
                GameObject NodeInstance = Instantiate(NodePrefab);
                Transform NodeInstanceTransform = NodeInstance.transform;
                Node NodeScript = NodeInstance.gameObject.GetComponent<Node>();
                NodeInstanceTransform.parent = this.transform;
                Vector3 position = myGrid.GetPosition() + new Vector3(i, 0, z);
                NodeInstanceTransform.position = position;
                NodeScript.SetCoords(NodeInstanceTransform.position);
            }
        }
    }

    public GameObject ConnectionPrefab;
    private void CreateConnection(Node Origin, Node Destination)
    {
        Vector3 positionA = Origin.GetCoords();
        Vector3 positionB = Destination.GetCoords();

        //Get median point
        Vector3 position = (positionA + positionB) / 2;
        //Get direction
        Vector3 direction = positionB - positionA;
        //Get rotation
        Quaternion rotation = Quaternion.LookRotation(direction);
        //distance == predetermined Spacing
        float distance = Spacing * 3;

        //Instantiate object using calculated parameters
        GameObject connection = Instantiate(ConnectionPrefab, position, rotation, Origin.transform);
        //Scale object according to distance
        connection.transform.localScale = new Vector3(connection.transform.localScale.x, connection.transform.localScale.y, distance);                                                             
    }

    private void ElaborateConnections()
    {
        Node[] allNodes = FindObjectsOfType<Node>();
        foreach (Node node in allNodes)
        {
            List<Node> allConnections = node.GetConnections();
            if (allConnections == null || allConnections.Count == 0)
            continue;

            for (int i = 0; i < allConnections.Count; i++)
            {
                Node currentConnection = allConnections[i];
                CreateConnection(node, currentConnection);
            }
        }
    }

    public bool CreateConnections;
    private void ConnectNodes()
    {
        if (CreateConnections == true)
        {
            ElaborateConnections();
            CreateConnections = false;
        }
    }

    void Update()
    {
        ConnectNodes();
    }

    void OnEnable()
    {
        if(gameObject.transform.parent == null) {Debug.LogWarning("Place MapComponent as MapObject son"); return;}
        
        GenerateGrid();    
    }

    void OnDisable()
    {
        int numberOfNodes = this.transform.childCount;

        for (int i = 0; i < numberOfNodes; i++)
        {
            GameObject obj = this.transform.GetChild(i).gameObject;
            Destroy(obj);
        }
    }

    #region GenerateGrid Methods
    //Calculate the dimensions of the object upon which to draw the map
    Vector2 CalculateDimensions()
    {
        Vector2 dimensionsFound = new Vector2();
        dimensionsFound.x = objToMap.GetSize().x;
        dimensionsFound.y = objToMap.GetSize().z;

        return dimensionsFound;
    }

    //Calculate the origin of the matrix, extrapolated from the origin of parent * Vector3 offset
    Vector3 FindOriginLocation()
    {
        Vector3 size = objToMap.GetSize();
        Vector3 offset = new Vector3(-size.x / 2, size.y, -size.z / 2);
        Vector3 gridOrigin = transform.parent.transform.position + offset;

        return gridOrigin;
    }
    #endregion
}