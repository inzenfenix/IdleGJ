using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    void Start()
    {
        EventManager.AddListener("Clicked", AcornAnim);
    }

    private void AcornAnim(object name)
    {
        StartCoroutine(PlayAnimation("TakingAcorn"));
    }

    private IEnumerator PlayAnimation(string name)
    {
        animator.SetBool(name, true);
        yield return new WaitForSeconds(0.35f);
        animator.SetBool(name, false);
    }
}
