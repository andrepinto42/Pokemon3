using UnityEngine;
public class GameUILoader : MonoBehaviour
{
    public GameObject DisplayStats;
    public GameObject PopUpMonName;
    public GameObject BordaButoes;
    public GameObject TextoDialogoCombate;
    public GameObject SwitchInHUD;
    public GameObject AllMonsSwitchinMenu;
    public GameObject DisplayAllSkillsInBattle;
    public GameObject DisplayAllButtonsInBattle;
    public GameObject DisplayStatsAfterLevelUpMon;
    public static GameUILoader Singleton;
    void Start()
    {
        if (Singleton == null)
            Singleton = this;

        //For debug purpose lets dissapear all the UI
        PopUserInterface();
    }

    public void PopUserInterface()
    {
        DisplayStats.SetActive(false);
        PopUpMonName.SetActive(false);
        BordaButoes.SetActive(false);
        TextoDialogoCombate.SetActive(false);
        SwitchInHUD.SetActive(false);
        DisplayStatsAfterLevelUpMon.SetActive(false);
    }

    //TODO
    //Add animations for the push event
    public void PushBattleBeginInterface()
    {
        TextoDialogoCombate.SetActive(true);
    }

    public void PushBattleStartEndingInterface()
    {
        BordaButoes.SetActive(true);
        DisplayAllSkillsInBattle.SetActive(false);
    }

    //If you want to hide then bool is false
    public void EnableDisplayStatsAfterMonLevelUp(bool value)
    {
        DisplayStatsAfterLevelUpMon.SetActive(value);
        TextoDialogoCombate.SetActive(value);
        
        BordaButoes.SetActive(!value);
    }
}