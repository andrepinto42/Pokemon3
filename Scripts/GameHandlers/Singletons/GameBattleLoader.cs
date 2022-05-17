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

    public void StartBattleLoadEnemyMon()
    {
        
    }
    public async void StartBattleLoader(MonManager enemy,Trainer enemyTrainer) 
    {
        //Store global variables
        ally = gameStatusManager.ally;
        // enemy = gameStatusManager.enemy;
        allyTrainer = gameStatusManager.allyTrainer;
        // enemyTrainer = gameStatusManager.enemyTrainer;


        //Initialize the ally
        TrainerHandler.RestartStatsMon(ally.MonMain);

        //Debug
        await Task.Delay(3000);

     
        //Initialize the enemy
        TrainerHandler.RestartStatsMon(enemy.MonMain);
        enemy.InitializeMeshMon(enemy.MonMain);


        //Reset the stats of all mons on the enemy trainer
        TrainerHandler.ResetMonTrainer(enemyTrainer);

    
        await Task.Delay(1000);
    
    //TODO
    //add animations to look better the spawning
        ally.InitializeMeshMon(ally.MonMain);

        allyParticlesSpawning.gameObject.SetActive(true);
        allyParticlesSpawning.gameObject.transform.position = ally.gameObject.transform.position;


        await Task.Delay(1000);

        GameUILoader.Singleton.PushBattleBeginInterface();
        
        //Load buttons images
		HandleSkillButton.InitializeButtonsSkills(gameStatusManager.allSkills,ally.MonMain);
        
        allyParticlesSpawning.gameObject.SetActive(false);
		
        await Task.Delay(500);
        //Currently now it should wait for the reference to be loaded
		await TextDialogManager.Singleton.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
		gameStatusManager.allOptionsButtons.SetActive(true);   
    }
    private void OnDisable() 
    {
        
    }
}