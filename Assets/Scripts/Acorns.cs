using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Acorns : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    
    private void Awake()
    {
        EventManager.AddListener("Clicked", DestroyAcorn);
    }

    private void Update()
    {
        if(transform.position.y < -10f)
            Destroy(gameObject);
    }

    private void DestroyAcorn(object name)
    {
        if ((string)name == gameObject.name)
        {
            EventManager.TriggerEvent("AcornGrabbed");
            ReleaseObject();
        }
    }

    public void ReleaseObject() => Pool.Release(gameObject);
}
