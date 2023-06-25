using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentAcorns = 0;
    public static int currentSquirrels = 0;
    public static int currentLevel = 0;
    public static int acornsGrabbed = 0;
    [SerializeField] Transform buyPoint;

    private void Start()
    {
        EventManager.AddListener("AcornGrabbed", UpdateAcorns);
        EventManager.AddListener("BoughtSquirrel", BoughtSquirrel);
    }

    private void UpdateAcorns()
    {
        currentAcorns += 1;
        acornsGrabbed += 1;
        EventManager.TriggerEvent("UpdateAcornUI", currentAcorns);
    }

    private void LevelUp()
    {
        currentLevel += 1;
        EventManager.TriggerEvent("LevelUp");
    }

    private void BoughtSquirrel(object squirrel)
    {
        Squirrels curSquirrel = (Squirrels)squirrel;
        currentSquirrels += 1;
        GameObject finalSquirrel = Instantiate(curSquirrel.squirrel, buyPoint.position, Quaternion.identity);
        finalSquirrel.GetComponent<SpriteRenderer>().color = curSquirrel.color;
    }
}
