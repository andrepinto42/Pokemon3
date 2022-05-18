using UnityEngine;
using System.Threading.Tasks;
[RequireComponent(typeof(GameStatusManager))]
public class GameBattleLoader : MonoBehaviour
{
    public static GameBattleLoader Singleton = null;
    public ParticleSystem allyParticlesSpawning;
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

        StartBattleAlly();

    }

    public async void StartBattleTrainerEnemy(MonManager enemy1,Trainer enemyTrainer1)
    {
        this.enemy = enemy1;
        this.enemyTrainer = enemyTrainer1;
        
        TrainerHandler.RestartStatsMon(ally.MonMain);

        //Debug
        await Task.Delay(3000);

     
        //Initialize the enemy
        TrainerHandler.RestartStatsMon(enemy.MonMain);
        enemy.InitializeMeshMon(enemy.MonMain);


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
        
        Debug.Log("Enemy position" +enemy.transform.position);
        Debug.Log("Ally position" +p_object.transform.position);
         
        pmove.StopMoving();
        pcam.canMove = false;
        
        var middle = (enemy.transform.position + p_object.transform.position) / 2f;
        var forwardPlayer =Vector3.Normalize(enemy.transform.position - p_object.transform.position);

        pcam.MoveCamera(middle);
        pcam.LookCamera(enemy.transform.position);
        pcam.MoveCamera(middle + Vector3.up * 2 + forwardPlayer * 2);
        
        await Task.Delay(1000);

        GameUILoader.Singleton.PushBattleBeginInterface();
        await TextDialogManager.Singleton.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
    
    
        //TODO
        //add animations to look better the spawning
        ally.InitializeMeshMon(ally.MonMain);

        allyParticlesSpawning.gameObject.SetActive(true);
        allyParticlesSpawning.gameObject.transform.position = ally.gameObject.transform.position;
        

        await Task.Delay(1000);

        
        //Load buttons images
		HandleSkillButton.InitializeButtonsSkills(gameStatusManager.allSkills,ally.MonMain);
        
        allyParticlesSpawning.gameObject.SetActive(false);
		
        await Task.Delay(1000);
		gameStatusManager.allOptionsButtons.SetActive(true);
        GameUILoader.Singleton.PushBattleStartEndingInterface();
        
        //Reset the player to its default Location
        pcam.canMove = true;
        pmove.canMove= true;
    }
    private void OnDisable() 
    {
        
    }
}