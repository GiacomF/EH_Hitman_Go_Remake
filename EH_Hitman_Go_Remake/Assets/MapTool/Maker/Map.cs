using System;
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
    void GenerateGrid()
    {
        Transform parentObject = gameObject.transform.parent;

        //Using the Local Scale as measures assures corret position
        objToMap = new objectToMap(parentObject.localScale);

        //Moving this transform to the gridOrigin for simpler nodes distribution
        myGrid = new Grid(CalculateDimensions(), FindOriginLocation());
        transform.position = myGrid.GetPosition();

    }

    void OnEnable()
    {
        if(gameObject.transform.parent == null) {Debug.LogWarning("Place MapComponent as MapObject son"); return;}
        
        GenerateGrid();    
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
