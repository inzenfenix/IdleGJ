using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeGame : MonoBehaviour
{
    //Object with the acorn
    [SerializeField] GameObject acorn;
    //Spawn points
    Vector3[] spawnPos = new Vector3[2];
    private bool spawning = false;
    private void Start()
    {
        //We give a random name to the tree
        this.name = $"Tree{Random.value * Random.Range(5, 1000000)}";
        //We add a listener so if the user clicks this tree, we activate the function
        EventManager.AddListener("Clicked", ThrowAcorn);
        spawnPos[0] = this.transform.position + new Vector3(1.25f, 1, 0);
        spawnPos[1] = this.transform.position + new Vector3(-1.25f, 1, 0);
    }
    private void ThrowAcorn(object name)
    {
        string objName = (string)name;
        if(!spawning && objName == this.name)
        {
            StartCoroutine(SpawnAcorns());
        }
        
    }

    IEnumerator SpawnAcorns()
    {
        //We use a coroutine to make time
        acorn.name = $"acorn{Random.value * Random.Range(5,1000000)}";
        spawning= true;
        GameObject.Instantiate(acorn,spawnPos[Random.Range(0, spawnPos.Length)], 
                               Quaternion.identity, this.transform);
        yield return new WaitForSeconds(.15f);
        spawning = false;
    }
}
