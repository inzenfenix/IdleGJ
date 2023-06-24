using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentAcorns = 0;
    public static int currentSquirrels = 0;
    [SerializeField] Transform buyPoint;

    private void Start()
    {
        EventManager.AddListener("AcornGrabbed", UpdateAcorns);
        EventManager.AddListener("BoughtSquirrel", BoughtSquirrel);
    }

    private void UpdateAcorns()
    {
        currentAcorns += 1;
        EventManager.TriggerEvent("UpdateAcornUI", currentAcorns);
    }

    private void BoughtSquirrel(object squirrel)
    {
        GameObject curSquirrel = (GameObject)squirrel;
        currentSquirrels += 1;
        Instantiate(curSquirrel, buyPoint.position, Quaternion.identity);
    }
}
