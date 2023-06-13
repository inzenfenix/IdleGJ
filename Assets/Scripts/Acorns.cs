using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorns : MonoBehaviour
{

    private void Awake()
    {
        EventManager.AddListener("Clicked", DestroyAcorn);
    }

    private void DestroyAcorn(object name)
    {
        if((string)name == this.name)
            Destroy(this.gameObject);
    }
}
