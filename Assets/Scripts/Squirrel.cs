using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;

public class Squirrel : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float followRadius = 25f;
    [SerializeField] NavMeshAgent agent;

    private bool followingAcorn = false;

    private float yRotation = 0f;

    private void Start()
    {
        agent.updateRotation = false;
        //agent.destination = new Vector3(17.30736f, 0, 4.462439f);
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0,yRotation,0);
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
            if (collider != null)
            {
                if (collider.gameObject.layer == 10 && !followingAcorn)
                {
                    agent.destination = collider.transform.position;
                    StartCoroutine(FollowAcorn(collider.gameObject));
                }
            }
            else
                return;
        }
    }

    IEnumerator FollowAcorn(GameObject acorn)
    {
        followingAcorn = true;
        while(acorn != null)
        {
            agent.destination = acorn.transform.position;
            yield return new WaitForSeconds(.25f);
        }
        followingAcorn = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position,followRadius);
    }
}
