using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void AcornAnim()
    {
        StartCoroutine(PlayAnimation("TakingAcorn"));
    }

    private IEnumerator PlayAnimation(string name)
    {
        animator.SetBool(name, true);
        yield return new WaitForSeconds(.225f);
        animator.SetBool(name, false);
    }
}
