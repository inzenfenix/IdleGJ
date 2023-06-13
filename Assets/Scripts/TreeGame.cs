using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGame : MonoBehaviour
{
    [SerializeField] GameObject acorn;
    [SerializeField] Transform[] spawnPos;
    private bool spawning = false;
    private void Start()
    {
        this.name = $"Tree{Random.value * Random.Range(5, 1000000)}";
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
        acorn.name = $"acorn{Random.value * Random.Range(5,1000000)}";
        spawning= true;
        GameObject.Instantiate(acorn,
        spawnPos[Random.Range(0, spawnPos.Length)]);
        yield return new WaitForSeconds(.2f);
        spawning = false;
    }
}
