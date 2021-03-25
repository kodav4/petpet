using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    GameManager gm;
    CanvasUI canvas;
    Ticker ticker;
    Pet pet;

    private string petBreed;

    GameObject activeToy; 

    //animator bools
    int Stage2;
    int Stage3;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();


        canvas = GetComponent<CanvasUI>();
        ticker = GetComponent<Ticker>();
        pet = GameObject.FindGameObjectWithTag("Pet").GetComponent<Pet>();
        petBreed = gm.chosePet.ToLower(); //"Dog" => "dog"

        UpgradePet(1);

        InvokeRepeating("PetCheck", 20, 1);
        InvokeRepeating("EverySecond", 1, 1);
    }



    bool sentHungryTicker;
    bool sentThirstyTicker;
    bool sentStarvingTicker;
    bool sentDehydratedTicker;
    bool sentDyingTicker;

    int alertLimit = 25; // int to warn player of pet needs at
    int secondAlertLimit = 5;

    void PetCheck() //runs every second to alert player to hunger / thirst
    {
        if (gm.tutorialEnded)
        {
            //Normal hunger/thirst scenarios
            if(pet.hunger > 0 || pet.thirst > 0)
            {
                //HUNGER ---------
                // > hungry
                if (pet.hunger <= alertLimit && !sentHungryTicker) //if pet hunger is <=25, send a hunger ticker
                {
                    ticker.ShowNewMessage(gm.petName + " is hungry.");
                    sentHungryTicker = true;
                }
                else if (sentHungryTicker && pet.hunger > alertLimit) //reset bool when hunger goes above 25 again
                {
                    sentHungryTicker = false;
                }

                // > starvation
                if (pet.hunger <= secondAlertLimit && !sentStarvingTicker)//if pet hunger is at minumum, send a starving ticker
                {
                    ticker.ShowNewMessage(gm.petName + " is starving.");
                    sentStarvingTicker = true;
                }
                else if (sentStarvingTicker && pet.hunger > secondAlertLimit)
                {
                    sentStarvingTicker = false;
                }

                //THIRST -----------
                // > thirsty
                if (pet.thirst <= alertLimit && !sentThirstyTicker)
                {
                    ticker.ShowNewMessage(gm.petName + " is thirsty.");
                    sentThirstyTicker = true;
                }
                else if (sentThirstyTicker && pet.thirst > alertLimit)
                {
                    sentThirstyTicker = false;
                }

                // > dehydration
                if (pet.thirst <= secondAlertLimit && !sentDehydratedTicker)//if pet hunger is at minumum, send a starving ticker
                {
                    ticker.ShowNewMessage(gm.petName + " is dehydrated.");
                    sentDehydratedTicker = true;
                }
                else if (sentDehydratedTicker && pet.thirst > secondAlertLimit)
                {
                    sentDehydratedTicker = false;

                }
            } 
            else //both hunger and thirst are at 0
            {
                if (!sentDyingTicker)
                {
                    sentDyingTicker = true;
                    //ticker.ShowNewMessage(gm.petName + " is dying.");
                    pet.GetComponent<Image>().color = Color.black;
                    ticker.ShowNewMessage(gm.petName + " has died. The game will reset shortly.");

                    StartCoroutine(ResetGame());
                }
                else if (sentDyingTicker && pet.hunger > secondAlertLimit && pet.thirst > secondAlertLimit)
                {
                    sentDyingTicker = false;
                    ticker.tickerQueue.Clear();
                }
                
            }
            
        }

    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5f);
        gm.ChangeScene("MainMenu");
    }


    void Update()
    {
        ClickEvents(); //manage clicking events

        //TimeEvents(); //==moved to EverySecond()

        CheckForKeypress(); //check for keyboard input - alternative input

        CheckForClick(); //check for clicking - keep track of click count on screen
    }

    void EverySecond() //runs every second
    {
        TimeEvents();
        Timer();
    }


    private bool noClicks;
    private bool oneClick;
    private bool twentyClicks;
    private bool fiftyClicks;
    private bool hundredClicks;
    private bool hundredFiftyClicks;
    private bool twoHundredClicks;
    private bool threeHundredClicks;
    private bool fourHundredClicks;
    private bool fourTwentyClicks;
    private bool fiveHundredClicks;
    //ticker events that happen based on clicking the pet
    void ClickEvents() 
    {
        switch (pet.clickCount) //click-based ticker messages
        {
            case 0:
                if (!noClicks)
                {
                    noClicks = true;
                    gm.tutorialEnded = false;
                    ticker.ShowNewMessage("Welcome to PetPet!");
                    //ticker.ShowNewMessage("This is a simple, virtual pet game.");
                    ticker.ShowNewMessage("Your new " + petBreed + ", " + gm.petName + ", is so happy to see you!");
                    ticker.ShowNewMessage("You can pet " + gm.petName + " by clicking on it. Give it a try if you haven't already!");
                }
                break;
            case 1:
                if (!gm.tutorialEnded && !oneClick)
                {
                    oneClick = true;
                    ticker.ShowNewMessage("Make sure to do this a lot! Petting " + gm.petName + " will give you much-needed money.");
                    ticker.ShowNewMessage("You should use money to buy <b>FOOD</b> and <b>WATER</b> for your companion using the buttons in the HUD.");
                    //ticker.ShowNewMessage("This ends the tutorial. Good luck!");
                    StartCoroutine("TutorialEnumerator", 10); //wait n seconds before resetting click count and starting stat decrease
                }
                break;
            case 25:
                if (!twentyClicks && gm.tutorialEnded)
                {
                    twentyClicks = true;
                    
                }
                break;
            case 50:
                if (!fiftyClicks && gm.tutorialEnded)
                {
                    fiftyClicks = true;
                    ticker.ShowNewMessage("TIP: You can use the Z and X buttons to feed and water your pet!");
                    ticker.ShowNewMessage("You can also use the SPACEBAR to PET " + gm.petName + ".");
                    ticker.ShowNewMessage("The \"P\" and ESCAPE buttons also open the PAUSE menu.");
                }
                break;
            case 100:
                if (!hundredClicks && gm.tutorialEnded)
                {
                    hundredClicks = true;
                    
                }
                break;
            case 150:
                if (!hundredFiftyClicks && gm.tutorialEnded)
                {
                    hundredFiftyClicks = true;
                    ticker.ShowNewMessage("TIP: While LOVE is great, too much can lead to unusual effects!");
                    ticker.ShowNewMessage("Be ready if your pet suddenly shows changes.");
                }
                break;
            case 200: //UPGRADE TO STAGE 2
                if (!twoHundredClicks && gm.tutorialEnded)
                {
                    twoHundredClicks = true;

                    UpgradePet(2);

                    ticker.ShowNewMessage("Your " + petBreed + " grew from all the <b>LOVE</b> you've given it!");
                }
                break;
            case 300:
                if (!threeHundredClicks && gm.tutorialEnded)
                {
                    threeHundredClicks = true;
                    ticker.ShowNewMessage("TIP: Pausing completely stops time, and allows you to change your pet's name!");
                }
                break;
            case 400:
                if (!fourHundredClicks && gm.tutorialEnded)
                {
                    fourHundredClicks = true;
                    ticker.ShowNewMessage(gm.petName + " seems to be searching for something.");
                }
                break;
            case 420:
                if(!fourTwentyClicks && gm.tutorialEnded)
                {
                    fourTwentyClicks = true;
                    ActivateUranium();
                    ticker.ShowNewMessage(gm.petName + " found (and ate) some uranium! Yum!");
                }
                break;
            case 500: //UPGRADE TO STAGE 3
                if (!fiveHundredClicks && gm.tutorialEnded)
                {
                    fiveHundredClicks = true;

                    UpgradePet(3);

                    ticker.ShowNewMessage(gm.petName + " grew more!");
                }
                break;
            case 600:
                Victory();
                break;
            default:
                break;
        }
    }


    //events that happen based on playtime
    void TimeEvents()
    {
        switch (gm.playtime)
        {
            case 60:
                //oneMinutePassed = true;
                ActivateToy();
                ticker.ShowNewMessage(gm.petName + " found a toy!");
                ticker.ShowNewMessage("Your pet seems proud of itself.");
                break;
            case (60 * 2):
                ticker.ShowNewMessage(gm.petName + " wants more <b>LOVE</b>!");
                break;
            case (60 * 3):
                ticker.ShowNewMessage("The effects of <b>LOVE</b> are often similar to that of radiation!");
                break;
            case (60 * 4):
                ticker.ShowNewMessage(gm.petName + " loves you very much!");
                break;
            case (60 * 5):
                ticker.ShowNewMessage(gm.petName + " is daydreaming.");
                break;
            default:
                break;
        }
    }

    //Checks for keyboard input thru update
    void CheckForKeypress()
    {
        if (Input.GetKeyDown(KeyCode.Z)) //HUNGER
        {
            pet.FeedPet();
            pet.CheckForButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.X)) //WATER
        {
            pet.WaterPet();
            pet.CheckForButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Space)) //LOVE
        {
            pet.LovePet();
            pet.CheckForButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            gm.LoadAdditiveScene("Options");
        }
    }

    //Checks for clicking the pet to update on screen debug text
    void CheckForClick()
    {
        //DEBUG: canvas.clickCountText.text = "Click count: " + pet.clickCount.ToString();
    }

    //updates every second to change timer on-screen
    void Timer()
    {
        gm.playtime++;
        //DEBUG: canvas.playtimeText.text = "Playtime (seconds): " + gm.playtime.ToString();
    }

    //resets click counter to prevent ticker issues
    IEnumerator TutorialEnumerator(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gm.tutorialEnded = true;
        pet.clickCount = 0; //reset so that early click events can be seen
        
    }

    void ActivateToy()
    {
        activeToy = Instantiate(pet.toy, gameObject.transform);
    }

    GameObject uranium; 

    void ActivateUranium()
    {
        uranium = Instantiate(canvas.uranium, gameObject.transform);
    }


    //Animator petAnim;
    //Sprite petImg;
    Vector3 newPosition;

    void UpgradePet(int stage)
    {
        newPosition = new Vector3(0, 0, 1);


        if (gm.chosePet == "Dog")
        {
            switch (stage)
            {
                case 1:
                    pet.GetComponent<Image>().sprite = canvas.dog1;
                    
                    break;
                case 2:
                    pet.GetComponent<Image>().sprite = canvas.dog2;

                    

                    newPosition.y = 50;
                    break;
                case 3:
                    pet.GetComponent<Image>().sprite = canvas.dog3;

                    newPosition.y = 50;
                    break;

                default:
                    break;
            }
        }
        else if (gm.chosePet == "Cat")
        {
            switch (stage)
            {
                case 1:
                    pet.GetComponent<Image>().sprite = canvas.cat1;

                    newPosition.y = 0f;
                    break;
                case 2:
                    pet.GetComponent<Image>().sprite = canvas.cat2;

                    newPosition.y = 60;
                    break;
                case 3:
                    pet.GetComponent<Image>().sprite = canvas.cat3;
                    newPosition.y = 60;
                    break;

                default:
                    break;
            }
        }

        pet.transform.localPosition = newPosition;


        pet.GetComponent<Image>().SetNativeSize();

    }

    bool wonGame;
    public void Victory()
    {
        if (!wonGame)
        {
            pet.hunger = 100;
            pet.thirst = 100;
            pet.decreaseThirstInt = 0;
            pet.decreaseHungerInt = 0;

            wonGame = true;
            ticker.ShowNewMessage(gm.petName + " has reached maturity. Congratulations!");
            ticker.ShowNewMessage("You can continue to <b>LOVE</b> your pet, and it will no longer go hungry or thirsty.");
            ticker.ShowNewMessage("You can also restart the game from the Pause menu.");
            ticker.ShowNewMessage("Thank you for playing!");
        }
    }

}
