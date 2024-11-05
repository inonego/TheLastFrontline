using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Randomizer
{
    public float minValue;
    public float maxValue;
    
    public float value => Random.Range(minValue, maxValue);
}
