using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    public GameObject spriteParent;

    [Header("Important")]
    public Text PetNameText;
    public Text MoneyText;

    [Header("Hunger / thirst sliders")]
    public Slider HungerSlider;
    public Slider ThirstSlider;

    [Header("Buttons")]
    public Button FoodButton;
    public Button WaterButton;

    [Header("Texts (mostly debug?)")]
    public Text playtimeText;
    public Text clickCountText;

    
    [Header("Sprites")]
   
    public Sprite dog1;
    public Sprite dog2;
    public Sprite dog3;
    public Sprite cat1;
    public Sprite cat2;
    public Sprite cat3;
   



    [Header("Misc")]
    public GameObject uranium;

    public AudioClip Music; //used as backup for debug init

}

