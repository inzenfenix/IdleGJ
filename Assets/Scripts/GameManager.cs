using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int currentAcorns = 0;
    [SerializeField] List<Squirrels> squirrels;
    private int currentSquirrels = 0;

    private void Start()
    {
        EventManager.AddListener("AcornGrabbed", UpdateAcorns);
    }

    private void UpdateAcorns()
    {
        currentAcorns += 1;
        EventManager.TriggerEvent("UpdateAcornUI", currentAcorns);
    }

    private void UpdateBuyableSquirrels()
    {
        foreach(var squirrel in squirrels)
        {
            EventManager.TriggerEvent("UpdateSquirrelShop", squirrel);
        }
    }
}
