using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Squirrel", menuName = "ScriptableObjects/Squirrel", order = 1)]
public class Squirrels : ScriptableObject
{
    [SerializeField] string description;
    [SerializeField] int defaultPrice;
    [SerializeField] GameObject squirrel;
    [SerializeField] Color color;
    [SerializeField] int requiredLevel;
}
