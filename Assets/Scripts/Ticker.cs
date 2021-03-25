using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticker : MonoBehaviour
{
    [SerializeField]
    private GameObject TickerPrefab; //prefab to instantiate

    GameObject currentTicker; //the instantiated ticker itself
    Animator currentTickerAnim; //cT's animator component
    Text currentTickerText; //cT's Text component (on its child)

    public bool currentlyShowingMsg; //is a ticker currently being shown?
    public float TimeToDisplayTicker; //how long should the tickers be displayed for?

    public Queue<string> tickerQueue = new Queue<string>(); //queue of strings in the case of multiple ticker messages

    void Start()
    {
        TimeToDisplayTicker = 3f;

        /*
        ShowNewMessage("PING 1");
        ShowNewMessage("PONG 2"); //should be queued
        ShowNewMessage("PUNG 3"); //should be displayed last
        */
    }

    public void ShowNewMessage(string tickerMessage, bool glitched = false)
    {
        if(!currentlyShowingMsg)
        {
            currentTicker = Instantiate(TickerPrefab, transform); //create the ticker as a child of the canvas, otherwise it wont appear
            Debug.Log("New Ticker message loaded.");
            currentTickerAnim = currentTicker.GetComponent<Animator>();
            currentTickerText = currentTicker.GetComponentInChildren<Text>();

            currentTickerText.text = tickerMessage;

            StartCoroutine(DisplayingTicker());
        }
        else if (!currentlyShowingMsg && glitched == true) //GLITCH TICKER xxxxx
        {
            currentTicker = Instantiate(TickerPrefab, transform); //create the ticker as a child of the canvas, otherwise it wont appear
            Debug.Log("New Glitch Ticker message loaded.");
            currentTickerAnim = currentTicker.GetComponent<Animator>();
            currentTickerText = currentTicker.GetComponentInChildren<Text>();

            currentTickerText.color = Color.red;

            currentTickerText.text = tickerMessage;

            StartCoroutine(DisplayingTicker());
        }
        else //if there is already a message being displayed, add the summoned message to a queue to be displayed ASAP
        {
            Debug.Log("NOTICE: Cannot show two tickers at once. Queuing message: " + tickerMessage);
            tickerQueue.Enqueue(tickerMessage);
        }
    }


    void HideMessage()
    {
        currentTickerAnim.SetTrigger("HideTicker"); //activates the vanishing state in the animator
        currentlyShowingMsg = false;
        Destroy(currentTicker,5f); //automatically destroy hidden tickers
    }

    IEnumerator DisplayingTicker() //wait for TtDT seconds, then hide message
    {
        currentlyShowingMsg = true;
        yield return new WaitForSecondsRealtime(TimeToDisplayTicker);
        HideMessage();
        CheckForQueue();
    }

    IEnumerator WaitForAnimEnd() //time to wait in between ticker changes. the queue should dequeue as soon as the previous ticker vanishes
    {
        yield return new WaitForSeconds(currentTickerAnim.GetCurrentAnimatorStateInfo(0).length);
        //yield return new WaitForSeconds(2f);
        ShowNewMessage(tickerQueue.Dequeue());
    }

    void CheckForQueue()
    {
        if (tickerQueue.Count != 0) //if there is a queue...
        {
            StartCoroutine(WaitForAnimEnd());
        }
    }


}
