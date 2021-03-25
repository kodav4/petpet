using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniButtonScript : MonoBehaviour
{
    //This is a simplified version of the menu script, for pre-menus and credits
    //MenuScript is specifically for the actual options menu in-game
    //this script should be attached to buttons leading to options and credits

    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>(); // DNR
    }

    public void GoToOptions() //for ingame
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>(); //DNR - makes ingame options button work. yes it has be be here twice


        if (!gm.paused) //prevents options menu from being opened twice
        {
            gm.PauseGame(true); //pauses and sets paused to true
            gm.LoadAdditiveScene("Options");
        }
    }

    public void Exit() //this method is also in MenuScript for the options menu
    {
        gm.PauseGame(false);

        SceneManager.UnloadSceneAsync(gm.additiveScene);
    }

    public void PointToGMPlaySFX(int i) //makes GM play a sound
    {
        gm.PlaySFX(i);
    }

    /* ///this doesnt work since it gets destroyed before it can play
    private AudioSource canvasAudioSource;
    public void ExitSound()
    {
        canvasAudioSource = GetComponent<AudioSource>();
        canvasAudioSource.PlayOneShot(canvasAudioSource.clip, gm.sfxVolume); //this is so convoluted.
    } 
    */
}
