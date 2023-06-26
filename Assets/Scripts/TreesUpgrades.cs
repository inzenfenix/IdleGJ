using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class TreesUpgrades : MonoBehaviour
{
    [SerializeField] GameObject[] trees;
    [SerializeField] GameObject buildings;

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
                        if (child.name.Contains("V1"))
                            child.GetComponent<TreeGame>().acornRate += 1;

                        foreach(Transform grandChild in child)
                            if (grandChild.name == "LeavesV1")
                            {
                                grandChild.gameObject.SetActive(true);
                                break;
                            }
                    }
                }
                break;

            case 2:
                foreach (GameObject tree in trees)
                {
                    foreach (Transform child in tree.transform)
                    {
                        if (child.name.Contains("V1"))
                        {
                            child.GetComponent<TreeGame>().TakeDownTree();
                        }

                        if (child.name.Contains("V2"))
                            child.gameObject.SetActive(true);
                    }
                }
                break;

            case 3:
                foreach (GameObject tree in trees)
                {
                    foreach (Transform child in tree.transform)
                    {
                        if (child.name.Contains("V2"))
                        {
                            child.GetComponent<TreeGame>().TakeDownTree();
                        }

                        if (child.name.Contains("V3"))
                            child.gameObject.SetActive(true);
                    }
                }
                break;

            case 4:
                buildings.SetActive(true);
                break;

            default:
                break;
        }
    }
}
