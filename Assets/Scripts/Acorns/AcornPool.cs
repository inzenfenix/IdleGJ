using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class AcornPool
{

    private ObjectPool<GameObject> treeAcornPool;

    private Transform[] spawnTransformPositionArray;
    private GameObject acorn;

    public AcornPool(GameObject acorn, Transform[] spawnTransformPositionArray)
    {
        this.acorn = acorn;
        this.spawnTransformPositionArray = spawnTransformPositionArray;

        treeAcornPool = new ObjectPool<GameObject>(SpawnAcorn, OnTakeFromPool, OnReturnedToPool,
                  OnDestroyPoolObject, true, 25, 1000);
    }

    private GameObject SpawnAcorn()
    {

        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnTransformPositionArray.Length);

        Vector3 spawnPosition = spawnTransformPositionArray[randomSpawnPointIndex].position;
        GameObject newAcorn = GameObject.Instantiate(acorn, spawnPosition, Quaternion.identity);

        newAcorn.GetComponent<AcornUnit>().Pool = treeAcornPool;

        return newAcorn;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);

        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
        {
            float forceAmount = 275.0f;
            Vector3 force = Vector3.right * forceAmount * Time.deltaTime;
            rigidBody.AddForce(force);
        }
    }

    private void OnReturnedToPool(GameObject obj)
    {
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
        {
            rigidBody.velocity = Vector3.zero;
        }

        obj.SetActive(false);
        
    }

    private void OnDestroyPoolObject(GameObject obj) => GameObject.Destroy(obj);

    public Transform[] SpawnTransformPositionArray
    {
        get { return spawnTransformPositionArray; }
        set {  spawnTransformPositionArray = value; }
    }

    public ObjectPool<GameObject> TreeAcornPool
    {
        get { return treeAcornPool; }
    }

    public GameObject Acorn
    {
        set { acorn = value; }
    }
}