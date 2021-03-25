using System.Collections;
using System.Collections.Generic;
//using UnityEditor;  this cannot be used in builds
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    GameManager gm;
    CanvasUI canvas;

    public GameObject DebugGO; //will have GM put on it if no GM exists

    public GameObject DogSpritePrefab;
    public GameObject CatSpritePrefab; 

    private Pet loadedPet;

    

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<CanvasUI>();

        gm = FindObjectOfType<GameManager>();


        //This allows you to skip the main menu and just go into the game for playtesting.
        //It checks to see if there is a gamemanager. If not, add a GM onto the debug GO and set to default pet, "Spot DB" the dog
        if (!gm)
        {
            DebugGO.AddComponent<GameManager>();
            gm = DebugGO.GetComponent<GameManager>();
            SetDefaults();
        }

        canvas.PetNameText.text = gm.petName;

        switch (gm.chosePet)
        {
            case "Dog":
                Instantiate(DogSpritePrefab, canvas.spriteParent.transform);
                Debug.Log("Loaded " + DogSpritePrefab.name + " prefab!");
                break;
            case "Cat":
                Instantiate(CatSpritePrefab, canvas.spriteParent.transform);
                Debug.Log("Loaded " + CatSpritePrefab.name + " prefab!");
                break;
            default:
                break;
        }


        /*
        switch (gm.ChosenPet) //switch case based on what pet breed was chosen - determines which sprite to show
        {

            
            case GameManager.Breeds.DOG:
                Instantiate(DogSpritePrefab, canvas.spriteParent.transform);
                Debug.Log("Loaded " + DogSpritePrefab.name + " prefab!");
                break;
            case GameManager.Breeds.CAT:
                Instantiate(CatSpritePrefab, canvas.spriteParent.transform);
                Debug.Log("Loaded " + CatSpritePrefab.name + " prefab!");
                break;
            default:
                break;
            
        } //instantiate correct prefab pet sprite based on gm.ChosenPet
        */


        //set up feed and water buttons ----------------

        loadedPet = FindObjectOfType<Pet>();

        gm.pet = loadedPet;

        gm.mainGameCanvas = canvas; //make reference to canvas accessible in other scripts

        canvas.FoodButton.onClick.AddListener(PointToFeed);
        canvas.WaterButton.onClick.AddListener(PointToWater);


        gm.PlayMusic(gm.InGameMusic);


        gm.gameActive = true;

    }

    void PointToFeed() //used for food button, since its uninitializable in inspector
    {
        loadedPet.FeedPet(); //points to the FeedPet() function for the pet
    }
    void PointToWater() //points to water button
    {
        loadedPet.WaterPet(); //points to WaterPet function
    }

    void SetDefaults() //used for debug, makes a debug GO the new GM
    {
        gm.gameObject.tag = "GameController";
        gm.petName = "SpotDB";
        gm.chosePet = "Cat";
    }


}
