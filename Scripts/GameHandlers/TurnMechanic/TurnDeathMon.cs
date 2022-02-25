using UnityEngine;
using TMPro;
public class TurnDeathMon
{
    private MonManager killerMonManager;
    private MonManager victimMonManager;
    private TurnMechanicMon killerTurnMechanic;
    private int counter;

    public TurnDeathMon(MonManager firstMonManager, MonManager secondMonManager, TurnMechanicMon firstTurnMechanic,int counter)
    {
        this.killerMonManager = firstMonManager;
        this.victimMonManager = secondMonManager;
        this.killerTurnMechanic = firstTurnMechanic;
        this.counter = counter;
    }

    //Handle when the first Mon Kills the second Mon
    internal async void HandleDeath()
    {
        
        killerTurnMechanic.onAttackOver -= HandleDeath;

        //LEANTWEEN SCALES DOWNS
        LeanTween.scale(victimMonManager.gameObject.GetComponentInChildren<MonMeshManager>().gameObject,Vector3.zero,1f);
        await TextDialogManager.Singleton.PushTextAwaitKey(victimMonManager.MonMain.GetNameMon() + " has been defeated !");
        var popUpObject = GameStatusManager.Singleton.PopUpMonButton;

        //Subscribe the event to be handled in the Class TurnAddLevel
        GameHUDStatusManager.Singleton.eventOnLevelGained += TurnAddLevel.AddLevelPokemon;

        ExperienceManager.DiscoverExperienceGained(killerMonManager,victimMonManager,counter);
        
        //Activate the popUpText after the battle has ended
        GameHUDStatusManager.Singleton.SetText(killerMonManager.MonMain);
        
        victimMonManager.gameObject.SetActive(false);
        
    }
}