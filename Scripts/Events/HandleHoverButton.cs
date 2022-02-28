using UnityEngine;
using Unity;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class HandleHoverButton : MonoBehaviour{

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var transChild =  transform.GetChild(i);
            for (int j = 0; j < transChild.childCount; j++)
            {
                var button = transChild.GetChild(j).GetComponent<Button>();
                if (button == null)
                    continue;
                
                ButtonFlash.AddListeners(button);                
            }
        }
    }

   
}