using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSFX : MonoBehaviour
{

    [SerializeField] private AudioSource treeSource;
    [SerializeField] private AudioClip[] treeAudios;

    [SerializeField] private AudioSource leavesSource;
    [SerializeField] private AudioClip[] leavesAudios;

    private void Awake()
    {
        if (TryGetComponent<TreeUnit>(out TreeUnit treeUnit))
        {
            treeUnit.onClickedTree += TreeUnit_OnClickedTree;
        }
    }

    private void TreeUnit_OnClickedTree(object sender, EventArgs e)
    {
        PlaySfx();
    }

    private void PlaySfx()
    {
        int randomTreeSoundIndex = UnityEngine.Random.Range(0, treeAudios.Length);
        int randomLeavesSoundIndex = UnityEngine.Random.Range(0, leavesAudios.Length);

        treeSource.clip = treeAudios[randomTreeSoundIndex];
        treeSource.Play();

        leavesSource.clip = treeAudios[randomLeavesSoundIndex];
        treeSource.Play();
    }
}
