using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class ButtonFlash
{
    static Button currentButtonFlashing;
    internal static void AddListeners(Button button)
    {                
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry pointEntry = new EventTrigger.Entry();
        pointEntry.eventID = EventTriggerType.PointerEnter;
        pointEntry.callback.AddListener( (eventData) => {
            StartButtonFlash(button); 
        });
        eventTrigger.triggers.Add(pointEntry);
        
        
        EventTrigger.Entry pointExit = new EventTrigger.Entry();
        pointExit.eventID = EventTriggerType.PointerExit;
        pointExit.callback.AddListener( (eventData) => {
            StopButtonFlash(); 
        });
        eventTrigger.triggers.Add(pointExit);
    }

    private static void StartButtonFlash(Button button)
    {
        lock(threadObject)
        {
            keepFlashing = true;
        }
        Flash(button);
    }

    const byte MIN_VALUE_COLOR = 150;
    const int STEPS = 255-MIN_VALUE_COLOR;

    const int DELAY = 20;
    static bool keepFlashing = true;

    static int numberThreads = 0;
    static object threadObject = new object();
    private static async void Flash(Button button)
    {

        lock (threadObject)
        {
            //There is a thread here somewhere
            if (numberThreads >= 1)
                return;
            numberThreads+=1;
        }
        byte i = 0;
        bool increase = true;

        float increaseScale = 1f /(float)STEPS;

        currentButtonFlashing = button;
        while(keepFlashing)
        {
        
            if (i >= STEPS)
                increase = false;
            else if (i <= 0)
                increase = true;

            i = (increase) ? (byte) (i+5) : (byte) (i -5);
            byte nc = (byte) ( MIN_VALUE_COLOR + i);

            lock(threadObject)
            {
                if (numberThreads > 1)
                    return;
            }

            currentButtonFlashing.transform.localScale = Vector3.one * ( (1+ increaseScale * i * 0.2f) );

            var colors =currentButtonFlashing.colors;
            colors.highlightedColor = new Color32(nc,nc,nc,255);
            currentButtonFlashing.colors = colors;

            await Task.Delay( DELAY );
        }
        
        lock(threadObject)
        {
            numberThreads -= 1;
            var colors =currentButtonFlashing.colors;

            Debug.Log("Exiting");
            //Return to normal color currentButtonFlashing
            colors.highlightedColor = new Color32(255,255,255,255);
            //Return to normal size currentButtonFlashing
            currentButtonFlashing.transform.localScale = Vector3.one;
            currentButtonFlashing.colors = colors;
            currentButtonFlashing = null;
        }
    } 

    //TODO
    private static void StopButtonFlash()
    {
        lock(threadObject)
        {
            keepFlashing = false;
        }
    }
}