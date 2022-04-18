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

    private async void OnEnable() 
    {
        //Store global variables
        ally = gameStatusManager.ally;
        enemy = gameStatusManager.enemy;
        allyTrainer = gameStatusManager.allyTrainer;
        enemyTrainer = gameStatusManager.enemyTrainer;


        //Initialize the ally
        TrainerHandler.RestartStatsMon(ally.MonMain);
        ally.InitializeMeshMon(ally.MonMain);

        allyParticlesSpawning.gameObject.SetActive(true);
        allyParticlesSpawning.gameObject.transform.position = ally.gameObject.transform.position;


        //Initialize the enemy
        TrainerHandler.RestartStatsMon(enemy.MonMain);
        enemy.InitializeMeshMon(enemy.MonMain);


        //Reset the stats of all mons on the enemy trainer
        TrainerHandler.ResetMonTrainer(enemyTrainer);
        //Load buttons images
		HandleSkillButton.InitializeButtonsSkills(gameStatusManager.allSkills,ally.MonMain);
		
        await Task.Delay(500);
        //Currently now it should wait for the reference to be loaded
		await TextDialogManager.Singleton.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
		gameStatusManager.allOptionsButtons.SetActive(true);   
    }
    private void OnDisable() 
    {
        
    }
}