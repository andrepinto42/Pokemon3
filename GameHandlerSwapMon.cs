using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

[RequireComponent(typeof(GameMonStatsDisplay))]
public class GameHandlerSwapMon : MonoBehaviour
{
    public GameObject PopUpSwapDisplayMon;
    public GameObject allMonsTextDisplay;
    public SkillSwapMon swapSkillAlly;
    public SkillSwapMon swapSkillEnemy;
    GameMonStatsDisplay gameMonStatsDisplay;
    GameObject[] arrMonsText;
    void Awake() 
    {
        gameMonStatsDisplay = GetComponent<GameMonStatsDisplay>();    
    }

    void Start()
    {
        arrMonsText = new GameObject[allMonsTextDisplay.transform.childCount];

        for (int i = 0; i <allMonsTextDisplay.transform.childCount; i++)
        {
            arrMonsText[i] = allMonsTextDisplay.transform.GetChild(i).gameObject;
            arrMonsText[i].SetActive(false);
        } 

		LoadMons(GameStatusManager.Singleton.allyTrainer);
    }

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
        PopUpSwapDisplayMon.SetActive(false);
        
        //Creates a new SwapAbility to be sent to the GameStatusManager
        swapSkillAlly.monExiting = monExiting.GetNameMon();
        swapSkillAlly.monEntering = monEntering.GetNameMon();
        
        //During 100 frames the pokemon will decrease in size
        var objectAllyMon = GameStatusManager.Singleton.ally.gameObject.transform;
        for (float j = 1f; j > 0f; j-=0.01f)
        {
            objectAllyMon.localScale = new Vector3(j,j,j);
            await Task.Yield();
        }

        //Swap the mon and handles the mesh and more UI coordenitaion
        GameStatusManager.Singleton.ally.SwapMon(monEntering);

        //Consume the players turn and return the object that is holds the logic for the turns
        var gameTurnHandler = GameStatusManager.Singleton.SendPlayerSkill_Beginning(swapSkillAlly);
        
        //During 100 frames the pokemon will increase in size
        for (float j = 0f; j < 1f; j+=0.01f)
        {
            objectAllyMon.localScale = new Vector3(j,j,j);
            await Task.Yield();
        }

        Debug.Log("Should Sleep now for a 3s");
        await Task.Delay(3000);
        Debug.Log("Awake now");
        
        //Change all of the buttons to match the mon abilities
		HandleSkillButton.InitializeButtonsSkills(GameStatusManager.Singleton.allSkills,monEntering);
        
        //Turn has ended so warn global variavel
        GameStatusManager.Singleton.ally.GetComponentInChildren<TurnMechanicMon>().IncrementTurnStage();

        //Enemy can resume their actions...
        gameTurnHandler.StartSecondMonMove();
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
            arrMonsText[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText(monName);
            arrMonsText[i].SetActive(true);

            //Assign the event on hover to each button for each corresponding Mon 
            HandleHoverButton(arrMonsText[i].GetComponent<Button>(),allMons[i]);
        }
    }
}
