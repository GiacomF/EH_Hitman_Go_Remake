using UnityEngine;

public class MapMaker : MonoBehaviour
{
    //I'm designing all the script in pseudo-code, and proceed by replacing

    public bool MapEditingMode = false;
    
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    public GameObject MapNodePrefab;
    public void PlaceNode()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f))
            {
                Instantiate(MapNodePrefab, hit.transform.position, Quaternion.identity);
                /*
                initialize the MovementNode (defining its parameters and communicating its presence to a Movement/Map Manager)
                Write a log communicating the Designer a new node was placed*/
            }

        //otherwise write a log stating ("Can't place a node here!")

    }
    /*
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