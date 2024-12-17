using UnityEngine;

public class MapMaker : MonoBehaviour
{
    //I'm designing all the script in pseudo-code, and proceed by replacing

    /*

    public Tag ValidTerrain;
    public bool MapEditingMode = false;

    void Update()
        if MapEditingMode == true
            EditNodes()

    private void EditNodes()

        on left click
            ray check if there is a surface and if the surface is ValidTerrain
                place a MovementNode on the raycast contact point
                initialize the MovementNode (defining its parameters and communicating its presence to a Movement/Map Manager)
                Write a log communicating the Designer a new node was placed

            otherwise write a log stating ("Can't place a node here!")

        on right click
            check if the ray is hitting a MapNode
                call DeleteNodeFromMap on the MapNode

            otherwise write a log stating ("Couldn't find a node")
    
    */
}

public class MapNode : MonoBehaviour
{
    /*
    
    private Vector3 nodePosition;

    public void Initialize()
        make the nodePosition match this node world position
        AddNodeToMap()
        
    
    private void AddNodeToMap()
        call method in MapManager giving nodePosition as argument

    public void DeleteNodeFromMap()
        call method in Mapmanager giving nodePosition as argument

    */
}