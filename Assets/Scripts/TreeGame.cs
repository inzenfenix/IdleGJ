using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGame : MonoBehaviour
{
    //Object with the acorn
    [SerializeField] GameObject acorn;
    //Spawn points
    [SerializeField] Transform[] spawnPos;
    private bool spawning = false;
    private void Start()
    {
        //We give a random name to the tree
        this.name = $"Tree{Random.value * Random.Range(5, 1000000)}";
        //We add a listener so if the user clicks this tree, we activate the function
        EventManager.AddListener("Clicked", ThrowAcorn);
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
        GameObject.Instantiate(acorn,
        spawnPos[Random.Range(0, spawnPos.Length)]);
        yield return new WaitForSeconds(.2f);
        spawning = false;
    }
}
