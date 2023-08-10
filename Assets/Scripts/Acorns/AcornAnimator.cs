using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornAnimator : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (TryGetComponent<AcornUnit>(out AcornUnit acornUnit))
        {
            acornUnit.onAcornDespawned += AcornUnit_OnAcornDespawned;
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent<AcornUnit>(out AcornUnit acornUnit))
        {
            acornUnit.onAcornDespawned -= AcornUnit_OnAcornDespawned;
        }
    }

    

    private void AcornUnit_OnAcornDespawned(object sender, EventArgs e)
    {
        animator.SetTrigger("OnGrabbed");
    }

}
