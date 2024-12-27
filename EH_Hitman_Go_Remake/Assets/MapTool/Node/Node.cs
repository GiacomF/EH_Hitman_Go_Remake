using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 Coords;
    public void SetCoords(Vector2 newCoords)
    {
        Coords = newCoords;
    }
    public Vector2 GetCoords()
    {
        return Coords;
    }

    private void RegisterIntoManager()
    {

    }
}
