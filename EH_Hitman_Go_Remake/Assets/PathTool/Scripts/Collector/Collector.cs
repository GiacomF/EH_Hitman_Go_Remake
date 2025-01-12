using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<Step> stepsCollected;

    public List<Step> GetStepsCollected()
    {
        return stepsCollected;
    }
}
