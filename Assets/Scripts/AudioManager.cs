using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject radio;
    private Animator radioAnim;
    [SerializeField] AudioMixer mixer;
    private bool volumeOn = true;

    private void Start()
    {
        EventManager.AddListener("ChangedVolume", ChangeVolume);
        radioAnim = radio.GetComponent<Animator>();
        radioAnim.SetBool("SoundOn", true);
    }

    private void ChangeVolume()
    {
        if (volumeOn)
        {
            radioAnim.SetBool("SoundOn", false);
            mixer.SetFloat("Volume", -80);
            volumeOn = false;
        }

        else
        {
            radioAnim.SetBool("SoundOn", true);
            mixer.SetFloat("Volume", 0);
            volumeOn = true;
        }
    }
}
