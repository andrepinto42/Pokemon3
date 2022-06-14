using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class GameHandlerSwapMon : MonoBehaviour
{
    public SkillSwapMon swapSkillAlly;
    public SkillSwapMon swapSkillEnemy;
    [SerializeField] GameMonStatsDisplay gameMonStatsDisplay;
    GameObject[] arrMonsText;
    

    //TODO REALLY MESSING IMPLEMENTATION
    public async void OnButtonClickSwapMon(int i)
    {
        var trainer = GameStatusManager.Singleton.allyTrainer;
        var monEntering = trainer.allMons[i];
        var monExiting = GameStatusManager.Singleton.ally.MonMain;        
        
        //Do not swap if the mon is the same
        if (monEntering == monExiting)
            return;


        Debug.Log("Swapping to this mon ->" + monEntering);
        GameUILoader.Singleton.SwitchInHUD.SetActive(false);
        GameUILoader.Singleton.DisplayAllSkillsInBattle.SetActive(false);
        GameUILoader.Singleton.DisplayAllButtonsInBattle.SetActive(false);
        GameUILoader.Singleton.DisplayStats.SetActive(false);
                
        //Creates a new SwapAbility to be sent to the GameStatusManager
        swapSkillAlly.monExiting = monExiting.GetNameMon();
        swapSkillAlly.monEntering = monEntering.GetNameMon();
        
        
        //Consume the players turn and return the object that is holds the logic for the turns
        var gameTurnHandler = GameStatusManager.Singleton.SendPlayerSkill_Beginning(swapSkillAlly);
        
        
        //Swap the mon and handles the mesh and more UI coordenitaion
        await GameStatusManager.Singleton.ally.SwapMon(monEntering,GameBattleLoader.Singleton.allyParticlesSpawning);

        //Change all of the buttons to match the mon abilities
		HandleSkillButton.InitializeButtonsSkills(GameUILoader.Singleton.DisplayAllSkillsInBattle,monEntering);
        
        //Turn has ended so warn global variavel
        GameStatusManager.Singleton.ally.GetComponentInChildren<TurnMechanicMon>().IncrementTurnStage();

        //Enemy can resume their actions...
        gameTurnHandler.StartSecondMonMove();
    }

    public void LoadTextForSwitching()
    {
        arrMonsText = new GameObject[GameUILoader.Singleton.AllMonsSwitchinMenu.transform.childCount];

        for (int i = 0; i <GameUILoader.Singleton.AllMonsSwitchinMenu.transform.childCount; i++)
        {
            arrMonsText[i] = GameUILoader.Singleton.AllMonsSwitchinMenu.transform.GetChild(i).gameObject;
            Debug.Log("Set to false");
            arrMonsText[i].SetActive(false);
        } 
        
		LoadMons(GameStatusManager.Singleton.allyTrainer);

        //Active the UI
        GameUILoader.Singleton.SwitchInHUD.SetActive(true);
        GameUILoader.Singleton.DisplayStats.SetActive(true);
    }

    private void HandleHoverButton(Button button,MonGame mon){
        EventTrigger eventTrigger;

        eventTrigger = button.gameObject.GetComponent<EventTrigger>();

        // If the button doesnt have a event trigger assigned then we should add one
        if (eventTrigger == null)
        {
            eventTrigger= button.gameObject.AddComponent<EventTrigger>();
        }

        //Clear all the triggers inside because they are of a previous mon 
        if (eventTrigger.triggers.Count != 0)
            eventTrigger.triggers.Clear();

        // Initialize a new on EnterMouse behaviour
        EventTrigger.Entry pointEntry = new EventTrigger.Entry();
        pointEntry.eventID = EventTriggerType.PointerEnter;

        pointEntry.callback.AddListener( 
            (eventData) => 
        {
            gameMonStatsDisplay.UpdateVisualMon(mon);
        });

        eventTrigger.triggers.Add(pointEntry);   
    }

    //Por enquanto o tamanho maximo de uma equipa é 4
    internal void LoadMons(Trainer allyTrainer)
    {        
        var allMons = allyTrainer.allMons;
        for (int i = 0; i < allMons.Length && i<4; i++)
        {
            if (allMons[i] == null)
                break;
            
            string monName = allMons[i].GetNameMon();
            // Debug.Log(arrMonsText[i]);
            arrMonsText[i].GetComponentInChildren<TMP_Text>().SetText(monName);
            Debug.Log("Name of Mon -> " + monName);
            arrMonsText[i].SetActive(true);

            //Carefull with the static child position
            //Set the UI with the corresponding image from the mon
            var imageMon = arrMonsText[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            var spriteMon = allMons[i].monADN.GetImage();
            if (spriteMon)
                imageMon.sprite = spriteMon; 

            //Assign the event on hover to each button for each corresponding Mon 
            HandleHoverButton(arrMonsText[i].GetComponentInChildren<Button>(),allMons[i]);
        }

        //Set the stats of the first mon as default
        gameMonStatsDisplay.UpdateVisualMon(allMons[0]);

    }
}
