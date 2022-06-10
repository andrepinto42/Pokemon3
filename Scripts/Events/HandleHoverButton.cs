using UnityEngine;
using Unity;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class HandleHoverButton : MonoBehaviour{

    public Transform buttonMainTransform;

    void Start()
    {
        for (int i = 0; i < buttonMainTransform.childCount; i++)
        {
            var transChild =  buttonMainTransform.GetChild(i);
            for (int j = 0; j < transChild.childCount; j++)
            {
                var button = transChild.GetChild(j).GetComponent<Button>();
                if (button == null)
                    continue;
                
                //TODO
                //MAKE A BETTER ON CLICK ENTER FUNCTION
                // ButtonFlash.AddListeners(button);                
            }
        }
    }

   
}