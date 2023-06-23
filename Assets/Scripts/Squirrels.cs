using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Squirrel", menuName = "ScriptableObjects/Squirrel", order = 1)]
public class Squirrels : ScriptableObject
{
    public string description;
    public int defaultPrice;
    public GameObject squirrel;
    public Color color;
    public int requiredLevel;
    public int index;
}
