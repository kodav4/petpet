using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pet : MonoBehaviour
{
    //base class for all pets.
    //this file should only have stuff that all 3 pet breeds share


    protected GameManager gm;
    CanvasUI canvas; //inspector 

    

    public int hunger;
    public int thirst;
    public int money; //used to feed / water pet
    public int love;
    public int clickCount;

    //used to determine how much to decrease each one by, every n seconds
    public int decreaseHungerInt;
    public int decreaseThirstInt;
    public int decreaseLoveInt;

    public int resourceCost; //how much money it costs to feed / water

    int maxStat = 100; //max hunger / thirst values
    int minStat = 0; // min 

    protected GameObject PetNameText; //actual text shown on the main game screen
    protected AudioSource petAudio; //audio component on the pet

    private Vector3 normalScale; //normal size for the pet for onmouseexit

    public GameObject toy; //must be initialized in child class



    protected virtual void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        canvas = gm.mainGameCanvas;
        petAudio = GetComponent<AudioSource>();
        normalScale = transform.localScale;

        InvokeRepeating("DecreaseStats", 1, 1); //DecreastStats() runs every b seconds, a second after Start()

        hunger = maxStat;
        thirst = maxStat;
        love = maxStat;
        clickCount = 0;

        money = 0;
        resourceCost = 10;

        UpdateMoney();

        //how much to decrease hunger/thirst every x seconds by default
        decreaseHungerInt = 1; 
        decreaseThirstInt = 3;
        decreaseLoveInt = 1;
    }

    protected virtual void Update()
    {
        CheckForButtonPress();
    }





    private float hoverScale = 1.1f; //how big to make the pet when hovering over
    private float clickScale = 1.05f; //size on click
    private float yVelocity = 0f; //
    [SerializeField]
    private float smoothTime = 0.1f; //time it takes to change size

    private bool changedSize = false; //makes sure the function only runs once


    private void OnMouseOver() //on hover, make pet a smidge bigger to encourage interaction
    {
        if (!gm.paused)
        {
            if (changedSize == false)
            {
                Debug.Log("Hovered over pet");
            }

            //anim


            float hoverXScale = Mathf.SmoothDamp(transform.localScale.x, hoverScale, ref yVelocity, smoothTime);
            float hoverYScale = Mathf.SmoothDamp(transform.localScale.y, hoverScale, ref yVelocity, smoothTime);

            transform.localScale = new Vector3(hoverXScale, hoverYScale, 1);

            changedSize = true;
        }
        
    }

    private void OnMouseExit() //on un-hover, return to normal size
    {
        if (!gm.paused)
        {
            transform.localScale = normalScale; //reset to normal size
            changedSize = false;
        }

    }

    protected virtual void OnMouseDown() //click to love, also change size
    {
        if (!gm.paused)
        {
            LovePet();

            float clickXScale = Mathf.SmoothDamp(transform.localScale.x, clickScale, ref yVelocity, smoothTime);
            float clickYScale = Mathf.SmoothDamp(transform.localScale.y, clickScale, ref yVelocity, smoothTime);

            transform.localScale = new Vector3(clickXScale, clickYScale, 1);
        }
    }

    //PET INTERACTION -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public void LovePet() //clicking on pet triggers this
    {
        if (!gm.paused)
        {
            clickCount++; //add to clickcount, which determines click-based events
            money++;

            UpdateMoney();

            MakeNoise(); //bark. meow. chirp.
        }

    }

    bool fed;
    bool watered;

    public int buttonAddAmount = 10; //how much to restore on button press. instead of altering min / max values, this might be easier to alter?

    public void FeedPet() //mainly triggered by pressing FEED button
    {
        if (!gm.paused && money >= resourceCost)
        {
            SpendMoney(resourceCost);

            hunger += buttonAddAmount;
            CheckForLimit(ref hunger); //make sure value isnt going over 100
            fed = true;
        }
        else if (money < resourceCost)
        {
            MoneyError();
        }

    }
    public void WaterPet() //mainly triggered by pressing WATER button
    {
        if (!gm.paused && money >= resourceCost)
        {
            SpendMoney(resourceCost);

            thirst += buttonAddAmount;
            CheckForLimit(ref thirst);
            watered = true;
        }
        else if (money < resourceCost)
        {
            MoneyError();
        }

    }

    void UpdateMoney()
    {
        canvas.MoneyText.text = money.ToString();
    }

    void SpendMoney(int cost)
    {
        money -= cost;
        UpdateMoney();
    }

    void MoneyError()
    {
        Debug.Log("NOTICE: Not enough money!");
    }

    void CheckForLimit(ref int stat) //ref needs to be there. still not clear on what it does
    {
        if(stat > maxStat)
        {
            stat = maxStat;
        }
    }

    public void CheckForButtonPress() //activated by update and also Z,X, Space buttons
    {
        //changes bar values instantly if the buttons are pressed
        if (fed == true)
        {
            canvas.HungerSlider.value = hunger;
            fed = false;
        }
        if (watered == true)
        {
            canvas.ThirstSlider.value = thirst;
            watered = false;
        }
    }


    public void DecreaseStats()
    {
        if (gm.tutorialEnded)
        {
            //Debug.Log("Running DecreaseStats(). Decreasing Hunger by [[" + decreaseHungerInt + "]], Thirst by [[" + decreaseThirstInt + "]], and Love by [[" + decreaseLoveInt + "]].");
            if (hunger <= minStat)
            {
                hunger = minStat;
            }
            else
            {
                hunger -= decreaseHungerInt;
            }
            if (thirst <= minStat)
            {
                thirst = minStat;
            }
            else
            {
                thirst -= decreaseThirstInt;

            }
            if (love <= minStat)
            {
                love = minStat;
            }
            else
            {
                love -= decreaseLoveInt;
            } 

            canvas.HungerSlider.value = hunger;
            canvas.ThirstSlider.value = thirst;
        }

        
    }


    protected virtual void MakeNoise()
    {
        //For some reason this empty function has to be here so it can be overridden by child Pet classes.
        //Otherwise, the spacebar doesnt make the pet bark / meow / chirp
    }



}
