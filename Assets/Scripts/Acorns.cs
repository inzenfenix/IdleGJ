using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Acorns : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private Animator animator;
    private bool clicked = false;
    private AudioSource source;
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioClip[] grabClips;


    private void Awake()
    {
        EventManager.AddListener("Clicked", DestroyAcorn);
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        clicked = false;
        StopAllCoroutines();
        transform.localScale = new Vector3(1, 1, 1);
        animator.SetBool("GrabbedAcorn", false);
        animator.SetBool("GotAcorn", true);
    }

    private void Update()
    {
        if(transform.position.y < -10f)
            ReleaseObject();
    }

    private void DestroyAcorn(object name)
    {
        if (!clicked && (string)name == gameObject.name)
        {
            clicked = true;
            animator.SetBool("GrabbedAcorn", true);
            animator.SetBool("GotAcorn", false);
            EventManager.TriggerEvent("AcornGrabbed");
            StartCoroutine(DestroyTimed());
        }
    }

    private IEnumerator DestroyTimed()
    {
        yield return new WaitForSeconds(0.6f);
        source.clip = grabClips[Random.Range(0, grabClips.Length)];
        source.Play();
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("GrabbedAcorn", false);
        ReleaseObject();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.layer == 18)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
    }

    public void ReleaseObject() => Pool.Release(gameObject);
}
