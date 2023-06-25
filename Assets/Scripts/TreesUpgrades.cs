using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class TreesUpgrades : MonoBehaviour
{
    [SerializeField] GameObject[] trees;

    private int currentLevel = 0;

    private void Start()
    {
        EventManager.AddListener("LevelUp", LevelUp);
    }

    private void LevelUp()
    {
        currentLevel += 1;
        switch(currentLevel)
        {
            case 1:
                foreach(GameObject tree in trees)
                {
                    foreach(Transform child in tree.transform)
                    {
                        if (child.name == "LeavesV1")
                        {
                            child.gameObject.SetActive(true);
                            break;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }
}
