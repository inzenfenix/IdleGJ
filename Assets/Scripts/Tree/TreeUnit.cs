using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TreeUnit : MonoBehaviour, IInteractable
{

    public static event EventHandler onAnyTreeSpawned;
    public static event EventHandler onAnyTreeUpgraded;


    public event EventHandler onClickedTree;

    [SerializeField] private GameObject acorn;
    [SerializeField] private GameObject leavesObject;
    [SerializeField] private Transform[] spawnTransformPositionArray;
    [SerializeField] private int level;

    private int acornAmount;
    private int acornsPerClick = 1;
    private float timer;
    private float cooldown;
    private bool spawning = false;

    private AcornPool acornPool;


    private void Awake()
    {
        acornPool = new AcornPool(acorn, spawnTransformPositionArray);
    }

    private void Start()
    {
        onAnyTreeSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

        if (!spawning)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0 && cooldown <= 0f)
        {
            SpawnAcorns();
        }


    }

    private void SpawnAcorns()
    {
        timer = .25f;

        GameObject newAcorn = acornPool.TreeAcornPool.Get();
        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnTransformPositionArray.Length);

        newAcorn.transform.position = spawnTransformPositionArray[randomSpawnPointIndex].position;

        onClickedTree?.Invoke(this, EventArgs.Empty);

        acornAmount++;

        if (acornAmount == acornsPerClick)
        {
            SpawnComplete();
        }
    }

    private void SpawnStart()
    {
        spawning = true;
        timer = 0;
        acornAmount = 0;
    }

    private void SpawnComplete()
    {
        spawning = false;
        cooldown = .25f;
    }

    public void ClickInteraction()
    {
        SpawnStart();
    }

    public int Level
    {
        get { return level; }
    }

}
