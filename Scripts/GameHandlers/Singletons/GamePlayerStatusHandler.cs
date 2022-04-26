using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerStatusHandler : MonoBehaviour
{
    public static GamePlayerStatusHandler Singleton = null;
    public GameObject playerGameObject = null;
    private void Awake() {

        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);
   }
}
