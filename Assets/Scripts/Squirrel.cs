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
    [SerializeField] Animator squirrelAnimator;

    private bool followingAcorn = false;


    private void Start()
    {
        agent.updateRotation = false;
        //agent.destination = new Vector3(17.30736f, 0, 4.462439f);
    }
    private void Update()
    {
        GrabAcorns();
        FollowAcornsAndTrees();
    }

    private void GrabAcorns()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, 1<<10);
        foreach (Collider collider in colliders)
        {
            EventManager.TriggerEvent("Clicked", collider.name);
        }
    }

    private void FollowAcornsAndTrees()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, followRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.activeInHierarchy)
            {
                if (collider.gameObject.layer == 10 && !followingAcorn)
                {
                    agent.destination = collider.transform.position;
                    StartCoroutine(FollowAcorn(collider.gameObject));
                    if(collider.transform.position.x < transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
                return;
        }
    }

    IEnumerator FollowAcorn(GameObject acorn)
    {
        followingAcorn = true;
        squirrelAnimator.SetBool("Walking", true);
        while(acorn.activeInHierarchy)
        {
            agent.destination = acorn.transform.position;
            yield return new WaitForSeconds(.25f);
        }
        squirrelAnimator.SetBool("Walking", false);
        followingAcorn = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position,followRadius);
    }
}
