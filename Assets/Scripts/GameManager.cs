using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentAcorns = 0;
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
}
