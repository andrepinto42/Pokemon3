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
	async void Start()
	{
		if (Singleton == null)
			Singleton = this;

		gameTurnChangeMon = GetComponent<GameTurnChangeMon>();
		//Load the mons to the display
		
		allOptionsButtons.SetActive(false);
		allSkills.SetActive(false);
		PopUpMonButton.SetActive(false);
		PopUpSwitchInMenu.SetActive(false);

		TrainerHandler.ResetMonTrainer(enemyTrainer);
		HandleSkillButton.Initialize(allSkills,ally.MonMain);
		
		await textDialogManager.PushText("A wild "+ enemy.MonMain.GetNameMon()+ " has appeared!");
		allOptionsButtons.SetActive(true);

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

		//TODO
		Skill skillEnemy = SkillBotManager.FindBestSkill(enemy.MonMain,ally.MonMain,
		enemy.MonMain.GetSkills(),SkillBotManager.DIFFICULTY.AWARE);
		
		
		GameTurnHandler turnHandler;
		if (ally.GetCurrentSpeed() >= enemy.GetCurrentSpeed())
		   turnHandler = new GameTurnHandler(ally,enemy,skillAlly,skillEnemy);
		else
		   turnHandler = new GameTurnHandler(enemy,ally,skillEnemy,skillAlly);
		  

		turnHandler.StartFirstMonMove();

		//Register the event turnOver so it can be called from turnHandler
		TurnMechanicMon.onTurnOver += turnHandler.HandleOnTurnOver;
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
		bool hasNextMon;
		//This function behaves very weird
		//For some reason you cant instantiate a object and assign it to the MonManager transform,
		//For that we need to do a little cheating, 
		// fixed-> LEANTWEEN.SCALE was making the mesh not render...
        if(ally.MonMain == monVictorius)
		{
			hasNextMon = await gameTurnChangeMon.HandleNextEnemyMon(enemy,enemyTrainer);
		}
		else
			hasNextMon = gameTurnChangeMon.HandleNextAllyMon(ally,allyTrainer);
    }

}
