using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Serializable]
    public struct Map
    {
        public List<Node> MapNodes;
    }

    [SerializeField]
    private Map currentMap;

    private List<Node> FindNodes()
    {
        List<Node> nodesFound = new List<Node>();
        
        GameObject MapContainer = GameObject.FindWithTag("MapContainer");
        Node[] nodes = MapContainer.GetComponentsInChildren<Node>();
        foreach (Node node in nodes)
        {
            nodesFound.Add(node);
        }
        
        return nodesFound;
    }

    public bool CreateMap;
    private Map ConstructMap()
    {
        currentMap = new Map();
        currentMap.MapNodes = FindNodes();

        if(currentMap.MapNodes.Count <= 0)
        {
            Debug.LogWarning(this.name + " " + "No nodes found on ObjectToMap");
        }

        return currentMap;
    }

    private void Update()
    {
        if(CreateMap)
        {
            ConstructMap();
            CreateMap = false;
        }
    }
}