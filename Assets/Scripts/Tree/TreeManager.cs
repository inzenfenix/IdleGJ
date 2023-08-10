using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;

    private List<TreeUnit> treeUnitList;

    public void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There is more than one Tree Manager.");
            Destroy(gameObject);
            return;
        }
        Instance = this;


    }

    private void Start()
    {
        TreeUnit.onAnyTreeSpawned += TreeUnit_OnAnyTreeSpawned;
    }

    private void TreeUnit_OnAnyTreeSpawned(object sender, EventArgs e)
    {
        TreeUnit treeUnit = sender as TreeUnit;
        treeUnitList.Add(treeUnit);
    }


    public List<TreeUnit> TreeUnitList
    {
        get
        {
            return treeUnitList;
        }
    }
}
