using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    private void Update()
    {
        GrabAcorns();
    }

    private void GrabAcorns()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, 1<<10);
        foreach (Collider collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }
}
