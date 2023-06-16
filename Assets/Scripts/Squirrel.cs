using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squirrel : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float followRadius = 25f;
    [SerializeField] NavMeshAgent agent;

    private bool followingAcorn = false;

    private float yRotation = 0;

    private void Start()
    {
        agent.updateRotation = false;
    }
    private void Update()
    {
        //transform.rotation = Quaternion.Euler(0,yRotation,0);
        GrabAcorns();
        FollowAcornsAndTrees();
    }

    private void GrabAcorns()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, 1<<10);
        foreach (Collider collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }

    private void FollowAcornsAndTrees()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, followRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.layer == 10 && !followingAcorn)
            {
                agent.destination = collider.transform.position;
                StartCoroutine(FollowAcorn(collider.gameObject));
            }
        }
    }

    IEnumerator FollowAcorn(GameObject acorn)
    {
        followingAcorn = true;
        while(acorn != null || acorn.transform.position.y < -1f)
        {
            yield return new WaitForSeconds(.1f);
        }
        followingAcorn = false;
    }
}
