using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//script for the options menu FUNCTIONALITY


public class MenuScript : MonoBehaviour
{
    GameManager gm;
    Pet pet;

    void Start()
    {
        gm = FindObjectOfType<GameManager>(); //DNR - makes main menu buttons work
        //gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if(SceneManager.GetActiveScene().name == "MainGame")
        {
            pet = GameObject.FindGameObjectWithTag("Pet").GetComponent<Pet>();
        }

        CheckForActiveGame();

        SetDefaultValues();

        if (!gm.gameActive)
        {
            Destroy(MainMenuButton.gameObject);
        }
    }

    void CheckForActiveGame() //make sure the game is active before certain options become available
    {
        if (!gm.gameActive)
        {
            breedDropdown.interactable = false;
            changeNameField.interactable = false;
        }
        else
        {
            breedDropdown.interactable = true;
            changeNameField.interactable = true;
        }
    }

    void SetDefaultValues()
    {
        musicVolumeSlider.value = gm.musicVolume;
        sfxVolumeSlider.value = gm.sfxVolume;
    }



    //options functions

    [Header("Options Variables")]
    public Dropdown breedDropdown;
    public InputField changeNameField;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [SerializeField]
    private Text errorText;

    [Header("Pet Changing Stuff")]
    [SerializeField]
    private GameObject DogSpritePrefab;
    [SerializeField]
    private GameObject CatSpritePrefab; //TODO assign this once complete
    [SerializeField]
    private GameObject BirdSpritePrefab; //TODO assign this once complete

    [Header("Miscellaneous")]
    [SerializeField]
    Button MainMenuButton;


    private int tempHunger;
    private int tempThirst;
    private int tempLove;
    private int tempClickCount;

    private Vector3 v3;

    public void ChangePetBreed() //the whole pet has to be destroyed, so save variables, reinitialize, and set the variables again
    {
        if (gm.gameActive)
        {

            //v3 = pet.transform.position;

            tempClickCount = pet.clickCount;
            tempHunger = pet.hunger;
            tempThirst = pet.thirst;
            tempLove = pet.love;

            switch (breedDropdown.value)
            {
                case 0:
                    gm.chosePet = "Dog";
                    Destroy(pet.gameObject);
                    pet = Instantiate(DogSpritePrefab).GetComponent<Pet>();
                    break;
                case 1:
                    gm.chosePet = "Cat";
                    Destroy(pet.gameObject);
                    pet = Instantiate(CatSpritePrefab).GetComponent<Pet>();
                    break;
                default:
                    Debug.LogError("ERROR: Change breed dropdown is invalid?");
                    break;
            }

            //set variables to what they were before
            pet.clickCount = tempClickCount;
            pet.hunger = tempHunger;
            pet.thirst = tempThirst;
            pet.love = tempLove;

            //pet.transform.position = v3;
        }
        else
        {
            //breedDropdown.interactable = false;
        }
    }

    public void ChangePetName() //allow the player to change the pet's name
    {
        if (gm.gameActive)
        {
            if (changeNameField.text.Length <= 0) //if somehow zero or less characters are inputted, display an error message
            {
                errorText.text = "ERROR: Name must be at least 1 character long.";
            }
            else
            {
                errorText.text = "";
                gm.petName = changeNameField.text;
                GameObject.Find("Pet Name Text").GetComponent<Text>().text = gm.petName;

                errorText.text = "Your pet's new name may take a minute to completely reflect in-game.";
            }
        }
        else
        {
            //changeNameField.interactable = false;
        }

    } 
    
    //both the variable and the actual component volume are changed here. the variable controls the volume of upcoming audio, while the second line controls current audio
    public void ChangeMusicVolume()
    {
        gm.musicVolume = musicVolumeSlider.value;
        gm.audioSources[0].volume = musicVolumeSlider.value;
    }

    public void ChangeSFXVolume()
    {
        gm.sfxVolume = sfxVolumeSlider.value;
        gm.audioSources[1].volume = sfxVolumeSlider.value;
    }


    // for quitting 

    private int quitCount = 0;

    public void VerifyQuit()
    {
        quitCount++;

        switch (quitCount)
        {
            case 1:
                //change to "are you sure?"
                MainMenuButton.GetComponentInChildren<Text>().text = "Really quit?";
                break;
            case 2:
                //actually quit
                QuitToMainMenu();
                break;
            default:
                break;
        }
    }

    public void QuitToMainMenu()
    {
        gm.ChangeScene("MainMenu");
    }


    //alpha finish screen
    public void ALPHAMainMenu()
    {
        Destroy(gm);
        SceneManager.LoadScene("MainMenu");
    }

    public void ALPHAFormLink()
    {
        Application.OpenURL("https://forms.gle/HWy2JWK5oDE2HRB47");
    }

}
