using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Pet
{
    //Parent variables:

    //gm, gamemanager
    //PetNameText: GameObject component for the name on-screen
    //

    //public GameObject tennisball;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    [Header("Audio")]
    public AudioClip bark1;
    public AudioClip bark2;

    //public AudioClip toySound;

    private int randomBark;

    protected override void MakeNoise()
    {
        base.MakeNoise();

        //choose a random bark audio clip, and play it
        randomBark = Random.Range(0, 2);

        switch (randomBark)
        {
            case 0:
                petAudio.PlayOneShot(bark1, gm.sfxVolume);
                break;
            case 1:
                petAudio.PlayOneShot(bark2, gm.sfxVolume);
                break;
            default:
                petAudio.PlayOneShot(bark1, gm.sfxVolume);
                Debug.LogWarning("randomBark is invalid. Value: " + randomBark);
                break;
        }
    }



}
