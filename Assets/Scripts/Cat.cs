using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Pet
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    [Header("Audio")]
    public AudioClip meow1;
    public AudioClip meow2;

    //public AudioClip toySound;

    private int randomMeow;

    protected override void MakeNoise()
    {
        base.MakeNoise();


        //choose a random audio clip, and play it
        randomMeow = Random.Range(0, 2);

        switch (randomMeow) //TODO: add more meows ?
        {
            case 0:
                petAudio.PlayOneShot(meow1, gm.sfxVolume);
                break;
            case 1:
                petAudio.PlayOneShot(meow2, gm.sfxVolume);
                break;
            default:
                petAudio.PlayOneShot(meow1, gm.sfxVolume);
                Debug.LogWarning("randomMeow is invalid. Value: " + randomMeow);
                break;
        }
    }


}
