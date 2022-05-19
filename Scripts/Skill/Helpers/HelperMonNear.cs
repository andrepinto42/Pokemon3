using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//This class is causing some unnecessary bloat by storing variables but
//  its ok because the code looks more modular
public class HelperMonNear{

    const int timeTotal_MS = 200;
    const int numberOfFramesNeeded = 20;
    const float targetDestinationPercent = 0.7f;
    const int delay = timeTotal_MS / numberOfFramesNeeded;
    const float step = targetDestinationPercent / numberOfFramesNeeded;

    //After its done doing its functions this variables will be set to null backAgain 
    private static Transform allyT = null;
    private static Vector3 initialPosition = Vector3.zero;
    private static Vector3 enemyPosition = Vector3.zero;
    private static Vector3 allyIntermediatePosition = Vector3.zero;


    public static async void StartGettingNearEnemy(MonManager ally,MonManager enemy)
    {
        //Load it to a global variable
        allyT =ally.gameObject.transform;
        initialPosition = allyT.position;
        enemyPosition =enemy.gameObject.transform.position;

        //Dont reach the center of the enemy just get near there
        for (float i = 0f; i < targetDestinationPercent; i+=step)
        {
            allyT.position = Vector3.Lerp(initialPosition,enemyPosition,i);
            await Task.Delay(delay);
        }
        allyIntermediatePosition = allyT.position;
    }

    public static async Task GoAwayFromEnemy()
    {
        Debug.Log(allyIntermediatePosition +" "+initialPosition+ " "+ allyT.position);
        for (float i = 0; i <1f; i+=step)
        {
            allyT.position = Vector3.Lerp(allyIntermediatePosition,initialPosition,i);
            await Task.Delay(delay);
        }
        
        //Reset the variables back to 0
        allyT = null;
        allyIntermediatePosition = Vector3.zero;
        initialPosition = Vector3.zero;
        enemyPosition = Vector3.zero;
    }
}