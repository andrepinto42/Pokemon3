using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
[RequireComponent(typeof(GameStatusManager))]
public class GameBattleLoader : MonoBehaviour
{
    public static GameBattleLoader Singleton = null;
    public ParticleSystem allyParticlesSpawning;
    public LayerMask layerToCollide;
    [HideInInspector]public bool isBattling = false;

    [Header("Camera Settings 1 Stage")]
    public float speedZoomInFirst = 2f;
    [Header("Camera Settings 2 Stage")]
    public float increaseDistanceCam = 1f;
    public float increaseHeight_2_Stage = 1f;
    [Header("Camera Settings 3 Stage")]
    public float increaseHeightRotating = 2f;
    public float radiusRotating = 6f;
    public float speedUpCameraRotating = 0.25f;
    [HideInInspector] public MonManager ally;
    [HideInInspector] public MonManager enemy;
    [HideInInspector] public Trainer allyTrainer;
	[HideInInspector] public Trainer enemyTrainer;
    GameStatusManager gameStatusManager;
    CancellationTokenSource cts;
    void Awake()
    {
        gameStatusManager = GetComponent<GameStatusManager>();
    }

    void Start()
    {
        if (Singleton == null)
            Singleton = this;
    }

    public void StartBattleWildEnemy(MonManager enemy1,MonGame monGame,GameObject monGameObject)
    {

        isBattling = true;

        //Load into the enemy1 MonManager so it can handle it's health and UI
        enemy1.LoadNewMon(monGame,monGameObject);

        this.enemy = enemy1;
        this.enemyTrainer = null;
        
        GameStatusManager.Singleton.enemy = this.enemy;
        GameStatusManager.Singleton.enemyTrainer = this.enemyTrainer;


        StartBattleAlly();

    }

    public async void StartBattleTrainerEnemy(MonManager enemy1,Trainer enemyTrainer1)
    {
        this.enemy = enemy1;
        this.enemyTrainer = enemyTrainer1;
        GameStatusManager.Singleton.enemy = this.enemy;
        GameStatusManager.Singleton.enemyTrainer = this.enemyTrainer;

        TrainerHandler.RestartStatsMon(ally.MonMain);

        //Debug
        await Task.Delay(3000);

     
        //Initialize the enemy
        TrainerHandler.RestartStatsMon(enemy.MonMain);
        await enemy.InitializeMeshMon(enemy.MonMain,allyParticlesSpawning);


        //Reset the stats of all mons on the enemy trainer
        TrainerHandler.ResetMonTrainer(enemyTrainer);

        StartBattleAlly();

    }
    public async void StartBattleAlly() 
    {
        //Store global variables
        this.ally = gameStatusManager.ally;
        this.allyTrainer = gameStatusManager.allyTrainer;

        var p_object = GamePlayerStatusHandler.Singleton.playerGameObject;
        var pcam =p_object.GetComponent<PlayerCameraFollow>();
        var pmove =p_object.GetComponent<PlayerMovement>();
        
        pmove.StopMoving();
        pcam.canMove = false;
        
        Vector3 ePos = enemy.transform.position + Vector3.up*2f;
        Vector3 playerPos = p_object.transform.position;

        var middle = (ePos + playerPos) / 2f;
        var forwardPlayer =Vector3.Normalize(ePos - playerPos);

        await pcam.MoveCamera(middle,ePos,speedZoomInFirst);

        //Push dialog and at the same time move the camera around
        GameUILoader.Singleton.PushBattleBeginInterface();
        var t1= TextDialogManager.Singleton.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
        var t2 = pcam.MoveCamera(middle + Vector3.up * 2 + forwardPlayer * 2,ePos);
        
        await Task.WhenAll(t1,t2);





        /*
        ------------------------------------------------------------
                2ND STAGE
        -----------------------------------------------------------
        */ 

        //Restart the stats of the trainer mons Just for battling
        for (int i = 0; i < allyTrainer.allMons.Length; i++)
        {
            var monI =allyTrainer.allMons[i];
            if( monI == null)
                continue;
            
            monI.RestartStatsEnteringBattle();
        }

        //Pan the camera away from the player in the direction of the enemy
        Vector3 newPlayerSetCamera = playerPos + Vector3.up*increaseHeight_2_Stage - forwardPlayer * increaseDistanceCam;
        pcam.MoveCameraInstant(newPlayerSetCamera,ePos); 

        var t3 = TextDialogManager.Singleton.PushText("Let's fight "+ ally.MonMain.GetNameMon()+ " !!!");
        
        RaycastHit hit;
        if (Physics.Raycast(middle+Vector3.up,Vector3.down,out hit,5f,layerToCollide))
        {
            Debug.Log("We hit something");
            ally.transform.position = hit.point;
        }
        else ally.transform.position= middle;


        /*
        ----------------
        Start Loading ALLY
        ----------------
        */
        //Ally looks in the direction of the enemy
        ally.transform.LookAt(ePos);

        //TODO
        //add animations to look better the spawning
        var t4 =ally.InitializeMeshMon(ally.MonMain,allyParticlesSpawning);
        
        

        await Task.WhenAll(t3,t4);






        /*
        ------------------------------------------------------------
                3ND STAGE
        -----------------------------------------------------------
        */ 
        //Load buttons images
		HandleSkillButton.InitializeButtonsSkills(GameUILoader.Singleton.DisplayAllSkillsInBattle,ally.MonMain);
      
        cts = new CancellationTokenSource();
        StartRotatingCamera(pcam,(ePos + ally.transform.position) / 2f); 

        pcam.enabled = false;

        await Task.Yield();

        GameUILoader.Singleton.DisplayAllButtonsInBattle.SetActive(true);
        GameUILoader.Singleton.PushBattleStartEndingInterface();
        
        //Reset the player to its default Location
        pcam.canMove = true;
        pmove.canMove= true;
    }
    public async void StartRotatingCamera(PlayerCameraFollow pcam,Vector3 center)
    {
        // - 90º, serve para colocar a camara a começar na zona do jogador
        float i = - Mathf.PI/2f;

        var vInicial = new Vector3( 
            Mathf.Sin(i)*radiusRotating + center.x,
            increaseHeightRotating + center.y,
            Mathf.Cos(i)*radiusRotating + center.z);

        await pcam.MoveCamera(vInicial,center,1f);


        while (!cts.IsCancellationRequested)
        {
            float x = Mathf.Sin(i)*radiusRotating + center.x;
            float y = increaseHeightRotating + center.y;
            float z = Mathf.Cos(i)*radiusRotating + center.z;
            
            i+=Time.deltaTime * speedUpCameraRotating;

            var v = new Vector3(x,y,z);
            pcam.MoveCameraInstant(v,center);
            await Task.Yield();
        }
    }
    private void OnDisable() 
    {
        StopRotatinCamera();
    }

    public void StopRotatinCamera()
    {
        cts.Cancel();

    }
}