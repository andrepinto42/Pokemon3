using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GameMonStatsDisplay))]
public class GameHandlerSwapMon : MonoBehaviour
{
    public GameObject PopUpSwapDisplayMon;
    public GameObject allMonsTextDisplay;
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
    public void OnButtonClickSwapMon(int i)
    {
        var trainer = GameStatusManager.Singleton.allyTrainer;
        var mon = trainer.allMons[i];
        Debug.Log("Swapping to this mon ->" + mon);

        //Swap the mon
        GameStatusManager.Singleton.ally.SwapMon(mon);

        //Consume the players turn

        //Change all of the buttons to match the mon abilities
		HandleSkillButton.Initialize(GameStatusManager.Singleton.allSkills,mon);

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
