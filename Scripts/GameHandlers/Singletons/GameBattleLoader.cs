using UnityEngine;
using System.Threading.Tasks;
[RequireComponent(typeof(GameStatusManager))]
public class GameBattleLoader : MonoBehaviour
{
    public static GameBattleLoader Singleton = null;
    public ParticleSystem allyParticlesSpawning;
    public LayerMask layerToCollide;
    public float newDegreeLookAtPlayer = 80f;
    public float increaseDistanceCam = 1f;
    public float increaseHeightCam = 2f;
    [HideInInspector] public MonManager ally;
    [HideInInspector] public MonManager enemy;
    [HideInInspector] public Trainer allyTrainer;
	[HideInInspector] public Trainer enemyTrainer;
    GameStatusManager gameStatusManager;
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
        
        Vector3 ePos = enemy.transform.position;
        Vector3 playerPos = p_object.transform.position;

        var middle = (ePos + playerPos) / 2f;
        var forwardPlayer =Vector3.Normalize(ePos - playerPos);

        await pcam.MoveCamera(middle,ePos);

        //Push dialog and at the same time move the camera around
        GameUILoader.Singleton.PushBattleBeginInterface();
        var t1= TextDialogManager.Singleton.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
        var t2 = pcam.MoveCamera(middle + Vector3.up * 2 + forwardPlayer * 2,ePos);
        
        await Task.WhenAll(t1,t2);

        //Pan the camera away from the player in the direction of the enemy
        SetCameraToMiddle(middle,playerPos,newDegreeLookAtPlayer,increaseDistanceCam,pcam);        

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

        
        await Task.Delay(1000);
        
        //Load buttons images
		HandleSkillButton.InitializeButtonsSkills(GameUILoader.Singleton.DisplayAllSkillsInBattle,ally.MonMain);
        
        // Move the camera for a better cinematic view
        SetCameraToMiddle(ally.transform.position,ePos,-60f,3f,pcam);        

        pcam.enabled = false;

        await Task.Delay(1000);
		
        //Restart the stats of the trainer mons Just for battling
        for (int i = 0; i < allyTrainer.allMons.Length; i++)
        {
            var monI =allyTrainer.allMons[i];
            if( monI == null)
                continue;
            
            monI.RestartStatsEnteringBattle();
        }

        GameUILoader.Singleton.DisplayAllButtonsInBattle.SetActive(true);
        GameUILoader.Singleton.PushBattleStartEndingInterface();
        
        //Reset the player to its default Location
        pcam.canMove = true;
        pmove.canMove= true;
    }

    private void SetCameraToMiddle(Vector3 startPoint,Vector3 endPoint,float angle,float increaseDistance,PlayerCameraFollow pcam)
    {
        var v0 = startPoint -endPoint;

        float distanceMiddle = Mathf.Sqrt(v0.x * v0.x + v0.z * v0.z);
        var angleRad=        Mathf.Acos(v0.x / distanceMiddle);

        var graus =  angleRad * Mathf.Rad2Deg;

        var newAngle = (graus - angle) * Mathf.Deg2Rad;

        var newPosition = new Vector3(
            (distanceMiddle+increaseDistance) * Mathf.Cos(newAngle),
            v0.y+increaseHeightCam,
            (distanceMiddle+increaseDistance) * Mathf.Sin(newAngle));
        
        var middle = (startPoint + endPoint) / 2f;
        
        pcam.MoveCameraInstant(endPoint+ newPosition,middle); 
    }
    private void OnDisable() 
    {
        
    }
}