using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TreeGame : MonoBehaviour
{
    //Object with the acorn
    [SerializeField] GameObject acorn;
    //Spawn points
    Vector3[] spawnPos = new Vector3[2];
    public int acornRate = 1;
    [SerializeField] string treeName;
    [SerializeField] GameObject leavesObject;
    private LeafBehaviour leaves;
    private bool spawning = false;
    private Animator animator;
    [SerializeField] AudioClip[] audios;
    private AudioSource source;

    [SerializeField] VisualEffect spark;

    //Pooling
    public static IObjectPool<GameObject> acornPool { get; set; }

    private void Awake()
    {
        if(acornPool == null)
            acornPool = new UnityEngine.Pool.ObjectPool<GameObject>(SpawnAcorn, OnTakeFromPool, OnReturnedToPool,
                  OnDestroyPoolObject, true, 100, 100000);
        animator = GetComponent<Animator>();
        leaves = leavesObject.GetComponent<LeafBehaviour>();
        source = gameObject.GetComponent<AudioSource>();
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

    private void OnEnable()
    {
        this.name = treeName;
    }

    private GameObject SpawnAcorn()
    {
        GameObject newAcorn = Instantiate(acorn, spawnPos[Random.Range(0, spawnPos.Length)],
                               Quaternion.identity);
        newAcorn.GetComponent<Acorns>().Pool = acornPool;

        return newAcorn;
    }
    public void ThrowAcorn(object name)
    {
        string objName = (string)name;
        if (!spawning && objName == this.name)
        {
                StartCoroutine(SpawnAcorns());
        }        
    }

    IEnumerator SpawnAcorns()
    {
        for (int i = 0; i < acornRate; i++)
        {
            spark.Play();
            source.clip = audios[Random.Range(0, audios.Length)];
            source.Play();

            animator.SetBool("TakingAcorn", true);
            if (leaves.gameObject.activeInHierarchy)
                leaves.AcornAnim();
            //We use a coroutine to make time
            acorn.name = $"acorn{Random.value * Random.Range(5, 1000000)}";
            spawning = true;
            GameObject newAcorn = acornPool.Get();
            newAcorn.transform.position = spawnPos[Random.Range(0, spawnPos.Length)];
            yield return new WaitForSeconds(.5f);
            spawning = false;
            animator.SetBool("TakingAcorn", false);
            yield return new WaitForEndOfFrame();
        }
    }

    public void TakeDownTree()
    {
        animator.SetBool("ChangedTree", true);
        leaves.TakeDownTree();
        StartCoroutine(DownTree());
    }

    private IEnumerator DownTree()
    {
        yield return new WaitForSeconds(.4f);
        gameObject.SetActive(false);
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
