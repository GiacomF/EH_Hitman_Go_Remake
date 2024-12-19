using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dimensions_SO mapDimensions;

    [Serializable]
    private struct Node
    {
        public Vector2 Coords;
    }

    private struct Grid
    {
        public Dictionary<Node, Vector2> NodesToCoords;

        public void Initialize()
        {
            NodesToCoords = new Dictionary<Node, Vector2>();
        }
    }

    public int Spacing = 1;
    private Grid myGrid;
    [SerializeField] int nodesInGrid;
    private void OnEnable()
    {
        myGrid.Initialize();

        if(mapDimensions != null)
        {
            int columns = (int)mapDimensions.Dimensions.x/(Spacing * 2);
            int rows = (int)mapDimensions.Dimensions.y/(Spacing * 2);

            for(int i = 0; i < columns; i++)
            {
                Node newNode = new Node();
                newNode.Coords = new Vector2(i, 0);
                myGrid.NodesToCoords.Add(newNode, newNode.Coords);
            }

            nodesInGrid = myGrid.NodesToCoords.Count;
        }

        else
            Debug.LogWarning("Missing Dimensions!");
    }
}
