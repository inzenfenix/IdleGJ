using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TreeGame : MonoBehaviour
{
    //Object with the acorn
    [SerializeField] GameObject acorn;
    //Spawn points
    Vector3[] spawnPos = new Vector3[2];
    [SerializeField] int acornRate = 1;
    [SerializeField] string treeName;
    private bool spawning = false;
    private Animator animator;

    //Pooling
    public static IObjectPool<GameObject> acornPool { get; set; }

    private void Awake()
    {
        acornPool = new UnityEngine.Pool.ObjectPool<GameObject>(SpawnAcorn, OnTakeFromPool, OnReturnedToPool,
              OnDestroyPoolObject, true, 100, 100000);
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        //We give a random name to the tree
        this.name = treeName;
        //We add a listener so if the user clicks this tree, we activate the function
        EventManager.AddListener("Clicked", ThrowAcorn);
        spawnPos[0] = this.transform.position + new Vector3(1.25f, 1, 0);
        spawnPos[1] = this.transform.position + new Vector3(-1.25f, 1, 0);
    }

    private GameObject SpawnAcorn()
    {
        GameObject newAcorn = Instantiate(acorn, spawnPos[Random.Range(0, spawnPos.Length)],
                               Quaternion.identity);
        newAcorn.GetComponent<Acorns>().Pool = acornPool;

        return newAcorn;
    }
    private void ThrowAcorn(object name)
    {
        for (int i = 0; i < acornRate; i++)
        {
            string objName = (string)name;
            if (!spawning && objName == this.name)
            {
                StartCoroutine(SpawnAcorns());
            }
        }
        
    }

    IEnumerator SpawnAcorns()
    {
        animator.SetBool("TakingAcorn", true);
        //We use a coroutine to make time
        acorn.name = $"acorn{Random.value * Random.Range(5,1000000)}";
        spawning= true;
        GameObject newAcorn = acornPool.Get();
        newAcorn.transform.position = spawnPos[Random.Range(0, spawnPos.Length)];
        yield return new WaitForSeconds(.25f);
        spawning = false;
        animator.SetBool("TakingAcorn", false);
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj) => Destroy(obj);
}
