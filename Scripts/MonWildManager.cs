using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;

public class MonWildManager : MonoBehaviour
{
    public MonGame monGame;
    public bool isInWild = true;
    MonMeshManager monMeshManager;

    const int frames = 60;
    const float interpolation = 1/(float)frames;

    void Start()
    {
        InstantiateMon();

        DisengageBattle();
    }

    private void InstantiateMon()
    {
        var monGameObject = Instantiate(monGame.GetMonMeshManager().gameObject,this.transform.position,
        Quaternion.identity,this.transform);

        monMeshManager = monGameObject.GetComponent<MonMeshManager>();
    }

    public async void EngageBattle()
    {
        isInWild = false;

        //Get the player reference from a Singleton, might be dangerous        
        var playerTransform = GamePlayerStatusHandler.Singleton.playerGameObject.transform;
        
        //This takes a while to get done
        await RotateTorwardsPlayer(playerTransform);
        
        //Display the HUD after the rotation is done
        monMeshManager.pivotHUD.gameObject.SetActive(true);
        
        //Change the stats of the Wild Mon Here
        monGame.currentHealth = monGame.maxHealth;
        monGame.currentStance = monGame.maxStance;
        monGame.currentStamina = monGame.maxStamina;


        //Offset the logic to another component
        //Important that we send the monMeshManager gameobject to be set as a child of MonManager otherwise engine will break?
        GameBattleLoader.Singleton.StartBattleWildEnemy(GameStatusManager.Singleton.enemy,monGame,monMeshManager.gameObject);
    }

    public void DisengageBattle()
    {
        isInWild = true;
        monMeshManager.pivotHUD.gameObject.SetActive(false);
        
        //Start looking for player
        var tokenCancellation = new CancellationTokenSource();
        Application.quitting += tokenCancellation.Cancel;
        taskLookForPlayer = LookForPlayer(tokenCancellation);
    }
    public float radiusOfCheckingPlayer= 20f;
    public LayerMask layerToCollide = 6;
    public int checkForPlayerFrequence = 500;
    [HideInInspector]public Task taskLookForPlayer;
    public async Task LookForPlayer(CancellationTokenSource cancelToken)
    {
        while(!cancelToken.IsCancellationRequested)
        {
            bool foundPlayer =Physics.CheckSphere(transform.position, radiusOfCheckingPlayer,layerToCollide );
            
            if (foundPlayer)
                break;
            
            await Task.Delay(checkForPlayerFrequence);
        }
        Application.quitting -= cancelToken.Cancel;
        
        Debug.Log("Found player!!");
        EngageBattle();
    }

    public async Task RotateTorwardsPlayer(Transform playerTransform)
    {
        float currentInterpolation = 0f;
        // frames of the mon changing its rotation to look at the player
        for (int i = 0; i < frames; i++)
        {
            //Get the direction
            var lookPos = playerTransform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
    
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,currentInterpolation);

            //Increase interpolation so in the next frame it will be looking more closely to the player
            currentInterpolation +=interpolation;
            await Task.Yield();
        }
    }
}
