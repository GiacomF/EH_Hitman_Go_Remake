using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<Step> stepsCollected = new List<Step>();
    public Step startPosition;

    public List<Step> GetStepsCollected()
    {
        return stepsCollected;
    }

}
