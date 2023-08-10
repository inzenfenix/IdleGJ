using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (TryGetComponent<TreeUnit>(out TreeUnit treeUnit))
        {
            treeUnit.onClickedTree += TreeUnit_OnClickedTree;
        }
    }

    private void TreeUnit_OnClickedTree(object sender, EventArgs e)
    {
        PlayClickAnimation();
    }

    private void PlayClickAnimation()
    {
        animator.SetTrigger("OnClick");
    }
}
