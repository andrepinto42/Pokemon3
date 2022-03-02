using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class ButtonFlash
{
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
            StopButtonFlash(button); 
        });
        eventTrigger.triggers.Add(pointExit);
    }

    private static void StartButtonFlash(Button button)
    {
        keepFlashing = true;
        Flash(button);
        // var colors =button.colors;
        // colors.highlightedColor = new Color32(150,150,150,255);
        // button.colors = colors;
    
    }

    const byte MIN_VALUE_COLOR = 150;
    static bool keepFlashing = true;
    private static async void Flash(Button button)
    {
        byte i = 0;
        int steps = 255-MIN_VALUE_COLOR;
        bool increase = true;
        while(keepFlashing)
        {
            if (i >= steps)
                increase = false;
            else if (i <= 0)
                increase = true;

            i = (increase) ? (byte) (i+1) : (byte) (i -1);
            byte nc = (byte) ( MIN_VALUE_COLOR + i);
            
            if (button == null)
                return;
            var colors =button.colors;
            colors.highlightedColor = new Color32(nc,nc,nc,255);
            button.colors = colors;

            await Task.Delay( (int) (Time.deltaTime * 1000) );
        }
    } 

    private static void StopButtonFlash(Button button)
    {
        var colors =button.colors;
        keepFlashing = false;

        colors.highlightedColor = new Color32(255,255,255,255);
        button.colors = colors;
    }
}