using System;
using System.Collections.Generic;
using UnityEngine;
public class GameTurnHandler
{
    private List<string> messagesEndTurn = new List<string>(){
        "What should you do now ?",
        "This battle just started trainer, keep up !",
        "C'mon let's show a big fight",
        "Maybe you should flee ?",
        "What is your next action ?"
    };
    private HandleAnimations firstAnimations;
    private HandleAnimations secondAnimations;
    private TurnMechanicMon firstTurnMechanic;
    private TurnMechanicMon secondTurnMechanic;
    private Skill firstSkill;
    private Skill secondSkill;
    private MonManager firstMonManager;
    private MonManager secondMonManager;
    private int moveCounter = 0;
    private bool battleEnded = false;

    public GameTurnHandler(MonManager firstManager, MonManager secondManager,
    Skill firstSkill, Skill secondSkill)
    {
        this.firstMonManager = firstManager;
        this.secondMonManager = secondManager;
        this.firstSkill = firstSkill;
        this.secondSkill = secondSkill;

        /*
        GetComponent inChildren are a little bit dangerous
        */
        firstAnimations = firstManager.gameObject.GetComponentInChildren<HandleAnimations>();
        secondAnimations = secondManager.gameObject.GetComponentInChildren<HandleAnimations>();
        
        firstTurnMechanic = firstManager.gameObject.GetComponentInChildren<TurnMechanicMon>();
        secondTurnMechanic = secondManager.gameObject.GetComponentInChildren<TurnMechanicMon>();
        
    }

    internal async void StartFirstMonMove()
    {
        //Increment the counter for total moves done
        moveCounter++;

        //Deal with the skill (damage,buff,special effects)
        await TextDialogManager.Singleton.PushText(firstSkill,firstMonManager,secondMonManager);
        bool killed = GameSkillManager.HandleSkill(firstSkill,firstMonManager,secondMonManager,firstAnimations);

        if (killed)
        {
            HandleDeathMon(firstMonManager,secondMonManager,firstTurnMechanic);
            return;
        }
        
        firstTurnMechanic.onAttackOver += StartSecondMonMove;        
    }

 
    //This method gets called when the AttackAnimation is done
    public async void StartSecondMonMove()
    {
        //Unsubscribe the method for no memory leak
        firstTurnMechanic.onAttackOver -= StartSecondMonMove;

        await TextDialogManager.Singleton.PushText(secondSkill,secondMonManager,firstMonManager);
        
        Debug.Log("Here is fine" + secondAnimations.name);
        
        bool killed = GameSkillManager.HandleSkill(secondSkill,secondMonManager,firstMonManager,secondAnimations);
        Debug.Log("Here is fineee");
        
        if (killed)
        {
            HandleDeathMon(secondMonManager,firstMonManager,secondTurnMechanic);
            return;
        }
        
    }

    public void HandleDeathMon(MonManager m1,MonManager m2,TurnMechanicMon t1)
    {
            //Transfer the controll to the TurnDeathMon so it can be easilly manipulated
            TurnDeathMon thm = new TurnDeathMon(m1,m2,t1,moveCounter);
            t1.onAttackOver += thm.HandleDeath;

            battleEnded = true;
    }

    public async void HandleOnTurnOver()
    {
		TurnMechanicMon.onTurnOver -= HandleOnTurnOver;

        if (battleEnded)
            return;
        string message = RandomMessageFinalTurn();
        await TextDialogManager.Singleton.PushText(message);

        GameUILoader.Singleton.DisplayAllButtonsInBattle.SetActive(true);
    }

   
    private string RandomMessageFinalTurn()
    {
        AddEndTurnMessages(GameStatusManager.Singleton.ally.MonMain.GetNameMon());

        System.Random r = new System.Random();
        int pos = r.Next(0,messagesEndTurn.Count);
        return messagesEndTurn[pos];
    }
    private void AddEndTurnMessages(string nameFirstMon)
    {
        messagesEndTurn.Add("Daaamn your " + nameFirstMon + " seems tired ");
        messagesEndTurn.Add("I think " + nameFirstMon + " wants more blood");
        messagesEndTurn.Add("I have a feeling that " + nameFirstMon + " wants to flee ");
    }
}