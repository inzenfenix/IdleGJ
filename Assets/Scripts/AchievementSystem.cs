using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] TextMeshProUGUI text;
    private Animator animator;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    private void Start()
    {
        source.clip = clip;
        animator = textObject.GetComponent<Animator>();
        EventManager.AddListener("Achievement", Achievement);
    }

    private void Achievement(object message)
    {
        source.Play();

        StartCoroutine(ReadMessage((string)message));
    }

    private IEnumerator ReadMessage(string message)
    {
        text.text = message;
        animator.SetBool("PopUpDown", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("PopUpDown", false);
        animator.SetBool("PopUpUp", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("PopUpUp", false);
    }

}
