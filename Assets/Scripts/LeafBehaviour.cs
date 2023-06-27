using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioClip[] audios;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        EventManager.AddListener("Clicked", ThrowAcorn);
    }
    private void ThrowAcorn(object name)
    {
        if ((string)name == this.name)
            transform.parent.gameObject.GetComponent<TreeGame>().ThrowAcorn(transform.parent.name);
    }
    public void AcornAnim()
    {
        StartCoroutine(PlayAnimation("TakingAcorn"));
    }

    public void TakeDownTree()
    {
        StartCoroutine(PlayAnimation("ChangedTree"));
    }

    public void PlayAudio()
    {
        source.clip = audios[Random.Range(0, audios.Length)];
        source.Play();
    }

    private IEnumerator PlayAnimation(string name)
    {
        source.clip = audios[Random.Range(0, audios.Length)];
        source.Play();
        yield return new WaitForSeconds(.225f);
        animator.SetBool(name, false);
    }
}
