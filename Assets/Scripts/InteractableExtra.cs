using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]

public class InteractableExtra : MonoBehaviour
{
    AudioSource audioSource;

    //any noises the gameobject might make.  ***if the GO has just one noise, use PlayAudioSourceClip()

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioSourceClip()
    {
        audioSource.PlayOneShot(GetComponent<AudioSource>().clip);
    }

}
