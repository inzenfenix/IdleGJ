using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeSquirrel : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float followRadius = 25f;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator squirrelAnimator;

    private bool pickingAcorn = false;
    [SerializeField] float acornRate = .35f;


    private void Start()
    {
        agent.updateRotation = false;
        //agent.destination = new Vector3(17.30736f, 0, 4.462439f);
    }
    private void Update()
    {
        PickAcorns();
        GoToTrees();
    }

    private void PickAcorns()
    {
        if (!pickingAcorn)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, 1 << 11);
            if(colliders.Length > 0)
                if(!colliders[0].GetComponent<TreeGame>().spawning)
                    StartCoroutine(PickedAcorn(colliders[0].name));
        }
    }

    IEnumerator PickedAcorn(string name)
    {
            pickingAcorn = true;
            yield return new WaitForSeconds(acornRate);
            EventManager.TriggerEvent("Clicked", name);
            pickingAcorn = false;
    }

    private void GoToTrees()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, followRadius);
        if (colliders.Length == 0)
            return;

        Collider collider = colliders[Random.Range(0, colliders.Length)];

            if (collider.gameObject.layer == 11)
            {
                agent.destination = collider.transform.position;
                StartCoroutine(FollowTree(collider.gameObject));

                if (collider.transform.position.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                    transform.rotation = Quaternion.Euler(0, 0, 0);
            }
    }

    IEnumerator FollowTree(GameObject tree)
    {
        squirrelAnimator.SetBool("Walking", true);
        while (Vector3.Distance(tree.transform.position,transform.position) > radius)
        {
            yield return new WaitForSeconds(.25f);
        }
        squirrelAnimator.SetBool("Walking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, followRadius);
    }
}
