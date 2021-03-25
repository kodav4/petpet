using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is used on the adoption screen.
//When the player clicks a breed, ChooseBreedScreen goes inactive, and VerifyBreedScreen appears.
//When verified, NameScreen appears and ChooseBreedScreen goes inactive. 

public class AdoptPet : MonoBehaviour
{
    GameManager gm;

    [Header("Sprites")]
    [SerializeField]
    private GameObject DogSprite; //onscreen dog sprite
    [SerializeField]
    private GameObject CatSprite; //onscreen cat sprite

    [Header("Screens")]
    //3 screens for the adoption menus
    public GameObject ChooseBreedScreen;
    public GameObject NameScreen;


    [Header("Anim")]
    public Animator spritesAnimator;
    public Animator nameFieldAnimator;


    [Header("Misc")]
    public Text PetNameInputText;

    public Button ContinueButton;

    private string tempPetName;


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (!gm)
        {
            Debug.LogWarning("There's no GameManager in this scene. Some functionality may be unavailable. Try loading from the main menu.");
        }

    }



    public void ChoosePet(string breed) //choose a breed of pet
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        switch (breed)
        {
            case "Dog":
                ChoseDog();

                gm.chosePet = "Dog";
                break;
            case "Cat":
                ChoseCat();

                gm.chosePet = "Cat";
                break;
            default:
                Debug.LogError("\"breed\" is invalid! breed must be equal to \"Dog\" or \"Cat\"! Current value: " + breed);
                break;
        }

    }


    //for controlling animations
    void ChoseDog()
    {
        spritesAnimator.SetBool("ChoseDog", true);
        nameFieldAnimator.SetBool("nameFieldChoseDog", true);

        gm.chosePet = "Dog";

        gm.PlaySFX(0);

        DisableSpriteButtons();
    }
    void ChoseCat()
    {
        spritesAnimator.SetBool("ChoseCat", true);
        nameFieldAnimator.SetBool("nameFieldChoseCat", true);

        gm.chosePet = "Cat";

        gm.PlaySFX(0);

        DisableSpriteButtons();
    }


    void DisableSpriteButtons() //disables button components on the sprites, prevents them from being clicked again
    {
        DogSprite.GetComponent<Button>().enabled = false;

        CatSprite.GetComponent<Button>().enabled = false;
    }


    public void Exit()
    {
        //reset anim bools...
        spritesAnimator.SetBool("ChoseDog", false);
        spritesAnimator.SetBool("ChoseCat", false);
        nameFieldAnimator.SetBool("nameFieldChoseCat", false);
        nameFieldAnimator.SetBool("nameFieldChoseDog", false);

        gm.ChangeScene("MainMenu");
    }

    public void ActivateContinueButton() //turns on the continue button after something has been inputted into the name field.
    {
        //TODO: for some reason, many references go null at this function, and have to be found again.... WHY??
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        PetNameInputText = GameObject.Find("InputtedName").GetComponent<Text>();
        ContinueButton = GameObject.Find("ContinueButton").GetComponent<Button>();


        if (PetNameInputText.text.Length > 0) //if the inputted text has a length of 1 or more...
        {
            ContinueButton.interactable = true;

            tempPetName = PetNameInputText.text;
            Debug.Log("tempPetName set to " + tempPetName);
        }
        else
        {
            ContinueButton.interactable = false; //if the player inputs something, exits, then deletes input. 
            Debug.LogWarning("PetNameField was exited without any input.");
        }
    }




    public void VerifyPetName() 
    {
        gm.petName = tempPetName; //solidify pet name

        gm.ChangeScene("MainGame");
    }

     
    public void BackToMainMenu()
    {
        gm.LoadLastScene(true);
    }

}
