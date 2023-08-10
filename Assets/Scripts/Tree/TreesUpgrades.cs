using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class TreesUpgrades : MonoBehaviour
{
   /* [SerializeField] GameObject[] trees;
    [SerializeField] GameObject buildings;

    private int currentLevel = 0;

    private void Start()
    {
        EventManager.AddListener("LevelUp", LevelUp);
    }

    private void LevelUp()
    {
        currentLevel += 1;
        if(currentLevel >= 1 && currentLevel < 4)
            EventManager.TriggerEvent("Achievement", $"Congrats! You are now Level {currentLevel}");
        if(currentLevel == 4)
            EventManager.TriggerEvent("Achievement", $"Congrats! You have finished the game and created THE SQUIRREL SOCIETY!");
        switch (currentLevel)
        {
            case 1:
                foreach(GameObject tree in trees)
                {
                    foreach(Transform child in tree.transform)
                    {
                        if (child.name.Contains("V1"))
                            //child.GetComponent<TreeUnit>().acornRate += 1;

                        foreach(Transform grandChild in child)
                            if (grandChild.name.Contains("LeavesV1"))
                            {
                                grandChild.gameObject.SetActive(true);
                                grandChild.gameObject.GetComponent<LeafBehaviour>().PlayAudio();
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
                            child.GetComponent<TreeUnit>().TakeDownTree();
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
                            child.GetComponent<TreeUnit>().TakeDownTree();
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
    }*/
}
