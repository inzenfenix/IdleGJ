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

    private void AcornAnim()
    {
        animator.Play("Leaves1AcornAnim");
    }
}
