using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornManager : MonoBehaviour
{
    public static AcornManager Instance;

    private List<AcornUnit> acornUnitList;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There is more than one acorn manager");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        acornUnitList = new List<AcornUnit>();
    }

    private void Start()
    {
        AcornUnit.onAnyAcornSpawned += AcornUnit_OnAnyAcornSpawned;
    }

    public void AcornUnit_OnAnyAcornSpawned(object sender, EventArgs e)
    {
        acornUnitList.Add(sender as AcornUnit);
    }

    public List<AcornUnit> AcornUnitList
    {
        get { return acornUnitList; }
    }


}
