using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
	[Header("Battle Options")]
	public Trainer allyTrainer;
	public Trainer enemyTrainer;
	public MonManager ally;
	public MonManager enemy;
	
	[Header("Buttons")]
	public GameObject allOptionsButtons;
	public GameObject allSkills;
	public GameObject PopUpMonButton;
	public GameObject PopUpSwitchInMenu;
	[SerializeField] TextDialogManager textDialogManager;
	GameTurnChangeMon gameTurnChangeMon;
	public static GameStatusManager Singleton;
	void Awake()
	{
		gameTurnChangeMon = GetComponent<GameTurnChangeMon>();
	}
	void Start()
	{
		if(Singleton == null)
			Singleton = this;
	}

   

    //WHEN AN ALLY ATTACKS USING THE BUTTON
    public void AllySelectsAttack()
   {
		allOptionsButtons.SetActive(false);
		allSkills.SetActive(true);
   }

	public void AllyDeselectsAttack()
   {
		allOptionsButtons.SetActive(true);
		allSkills.SetActive(false);
   }

	//MOST IMPORTANT FUNCTION
   public async void OnSkillButtonClicked(int i)
	{
		allSkills.SetActive(false);
		
		//Get the skill and check if is valid
		Skill skillAlly = ally.MonMain.GetSkills()[i];
		bool valid = await CheckValidSkill(skillAlly);
		if (!valid)
		{
			allSkills.SetActive(true);
			return;
		}

		//Transfer the controll here because this function will be reused
		SendPlayerSkill_Beginning(skillAlly);
	}

	private async Task<bool> CheckValidSkill(Skill skill)
	{
		bool enoughStamina = ally.HandleStaminaManagement(skill);
        if (!enoughStamina)
        {
            await TextDialogManager.Singleton.PushText("Not enought stamina to perform " + skill.nameSkill+ " !");
			return false;
        }
		return true;
	}

    internal async void SendNextMon(MonGame monVictorius)
    {
		//Check to see if we are fighting a wild pokemon
		if (enemyTrainer == null)
		{
			EndBattle();
			return;
		}

		bool hasNextMon;
        if(ally.MonMain == monVictorius)
		{
			hasNextMon = await gameTurnChangeMon.HandleNextEnemyMon(enemy,enemyTrainer);
		}
		else
			hasNextMon = gameTurnChangeMon.HandleNextAllyMon(ally,allyTrainer);
    }

   

    public GameTurnHandler SendPlayerSkill_Beginning(Skill skillAlly){
		
		//TODO Add the possibilty that the enemy can swap out pokemon
		Skill skillEnemy = SkillBotManager.FindBestSkill(enemy,ally,
		enemy.MonMain.GetSkills(),SkillBotManager.DIFFICULTY.AWARE);
		
		Debug.Log("Name of skill enemy" + skillEnemy.nameSkill);
		GameTurnHandler turnHandler;
		
		//Checks for Swaps
		if (skillAlly is SkillSwapMon)
			turnHandler = new GameTurnHandler(ally,enemy,skillAlly,skillEnemy);
		else if(skillEnemy is SkillSwapMon)
			turnHandler = new GameTurnHandler(enemy,ally,skillEnemy,skillAlly);
		
		//Normal behaviour, faster pokemon starts the combat fight
		else if (ally.GetCurrentSpeed() >= enemy.GetCurrentSpeed())
			turnHandler = new GameTurnHandler(ally,enemy,skillAlly,skillEnemy);
		else
			turnHandler = new GameTurnHandler(enemy,ally,skillEnemy,skillAlly);
		  

		turnHandler.StartFirstMonMove();

		//Register the event turnOver so it can be called from turnHandler
		TurnMechanicMon.onTurnOver += turnHandler.HandleOnTurnOver;
		return turnHandler;
	}
	//TODO
	 private async void EndBattle()
    {
		TurnOffUI();
		
		//The MonManager doenst need to be disabled for now
		enemy.gameObject.SetActive(true);
		enemy.KillGameObject();
		
		var pcam =  GamePlayerStatusHandler.Singleton.playerGameObject.GetComponent<PlayerCameraFollow>();		
		pcam.enabled = true;
		await Task.Delay(3000);
		ally.KillGameObject();
	}
	private void TurnOffUI()
    {
		//Remove the text of the level up Stats
		GameHUDStatusManager.Singleton.ToggleDisplay(false);

		GameUILoader.Singleton.PopUserInterface();
    }

}
