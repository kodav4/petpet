using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    [Header("Audio")]
    //private AudioSource musicAudioSource;
    //private AudioSource sfxAudioSource;
    public AudioClip TitleScreenMusic;
    public AudioClip InGameMusic;
    public AudioClip[] soundEffects; //TODO: convert this to dictionary?
        //0: menu button press
        //1: menu button press cancel
        //2: bigger button press for more important buttons
        //3: bark
        //4: different bark
        //5: meow
    public AudioSource[] audioSources;
    public float musicVolume; //between 0 and 1
    public float sfxVolume; //between 0 and 1

    [Header("Miscellaneous")]
    private GameObject otherGM; //used to see if there are more than 2 GMs in scene

    public string petName;
    //public string playerName;

    /*
    public enum Breeds { DOG, CAT };
    public Breeds ChosenPet;
    */

    public string chosePet; //for some reason the enum resets when loading a new scene...

    public Pet pet; //defined once the pet sprite appears

    [HideInInspector]
    public string previousScene; //used in options menu when trying to exit

    [HideInInspector]
    public CanvasUI mainGameCanvas; //initialized on main game screen

    public int playtime; //time since MAIN game start (doesnt include pregame)

    [Header("Booleans")]
    public bool tutorialEnded;
    public bool BadMode;
    public bool gameActive;
    public bool paused;
    bool loadedMainMenu; //why is there a warning here???

    //public bool DEBUGMODE;




    void Start()
    {
        Screen.SetResolution(800, 600, false);
        QualitySettings.antiAliasing = 0;


        musicVolume = 0.5f;
        sfxVolume = 0.5f;

        PlayMusic(TitleScreenMusic);


        if (!loadedMainMenu)
        {
            DontDestroyOnLoad(this.gameObject); //preserves GM across the whole game
        }

        loadedMainMenu = true;
        gameActive = false;

    }

    [HideInInspector]
    public string additiveScene; //

    //instead of changing scenes, just load another without messing with the current scene
    public void LoadAdditiveScene(string newSceneName)
    {
        Debug.Log("Loading " + newSceneName + " scene additively");

        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

        additiveScene = newSceneName;
    }

    public void ChangeScene(string newSceneName)
    {
        //saves the previous scene so it can be returned to later, like when exiting the options menu
        previousScene = SceneManager.GetActiveScene().name;

        Debug.Log("previousScene has been set to " + previousScene);


        if (newSceneName != null)
        {
            SceneManager.LoadScene(newSceneName.ToString());
        }
        else
        {
            Debug.LogError("newSceneName is null!");
        } //catch for not filling in nSN in inspector

        if (newSceneName == "MainMenu")
        {
            Destroy(gameObject);
        }

    }

    public void LoadLastScene(bool destroyGM)
    {
        SceneManager.LoadScene(previousScene);

        if (destroyGM) //for removing repeat GMs after returning to main menu
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame(bool pausedBool)
    {

        if (pausedBool)
        {
            paused = true;

            Time.timeScale = 0;
        }
        else
        {
            paused = false;

            Time.timeScale = 1;
        }
        
    }

    public void ActivateBadMode()
    {
        BadMode = true;

        pet.hunger = 0;
        pet.thirst = 0;

        pet.buttonAddAmount = 1;

        BadModeActivation();

        MoreTBA();
    }

    IEnumerator BadModeActivation()
    {
        yield return new WaitForSeconds(2);
    }

    public void QuitGame() //quits the game ***in builds only***
    {
        Application.Quit();
    }

    public void PlayMusic(AudioClip song)
    {
        if (song == null)
        {
            Debug.LogWarning("WARN: song is null!");
        }
        else
        {
            audioSources[0].clip = song;
            audioSources[0].volume = musicVolume;
            audioSources[0].Play();
        }
    }


    public void PlaySFX(int effectNum) //plays a sound effect from gm's soundEffects[] array
    {
        audioSources[1].PlayOneShot(soundEffects[effectNum], sfxVolume);
    }


    void MoreTBA()
    {
        ChangeScene("IncompleteScreen");
    }
}


