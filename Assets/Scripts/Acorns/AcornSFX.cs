using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AcornSFX : MonoBehaviour
{

    [SerializeField] private AudioSource collisionAudioSource;
    [SerializeField] private AudioSource grabAudioSource;

    [SerializeField] private AudioClip[] collisionAudioClips;
    [SerializeField] private AudioClip[] grabAudioClips;


    private void OnEnable()
    {
        if (TryGetComponent<AcornUnit>(out AcornUnit acornUnit))
        {
            acornUnit.onAcornDespawned += AcornUnit_OnAcornClicked;
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent<AcornUnit>(out AcornUnit acornUnit))
        {
            acornUnit.onAcornDespawned -= AcornUnit_OnAcornClicked;
        }
    }

    private void AcornUnit_OnAcornClicked(object sender, EventArgs e)
    {
        PlayClickedSFX();
    }

    private void PlayClickedSFX()
    {
        int randomGrabClipIndex = UnityEngine.Random.Range(0, grabAudioClips.Length);
        grabAudioSource.clip = grabAudioClips[randomGrabClipIndex];
        grabAudioSource.PlayDelayed(.4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == 18)
        {
            int randomCollisionClipIndex = UnityEngine.Random.Range(0, collisionAudioClips.Length);
            collisionAudioSource.clip = collisionAudioClips[randomCollisionClipIndex];
            collisionAudioSource.Play();
        }
    }
}
