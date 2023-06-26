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
    //[SerializeField] int debugAcorns;

    private void Start()
    {
        EventManager.AddListener("AcornGrabbed", UpdateAcorns);
        EventManager.AddListener("BoughtSquirrel", BoughtSquirrel);
        EventManager.AddListener("LevelUp", LevelUp);
    }

    private void Update()
    {
        //currentAcorns = debugAcorns;
        //acornsGrabbed = debugAcorns;
    }
    private void UpdateAcorns()
    {
        currentAcorns += 1;
        acornsGrabbed += 1;
        EventManager.TriggerEvent("UpdateAcornUI");
    }

    private void LevelUp()
    {
        currentLevel += 1;
        EventManager.TriggerEvent("LevelUpTree");
    }

    private void BoughtSquirrel(object squirrel)
    {
        Squirrels curSquirrel = (Squirrels)squirrel;
        currentSquirrels += 1;
        GameObject finalSquirrel = Instantiate(curSquirrel.squirrel, buyPoint.position, Quaternion.identity);
        finalSquirrel.GetComponent<SpriteRenderer>().color = curSquirrel.color;
    }
}
