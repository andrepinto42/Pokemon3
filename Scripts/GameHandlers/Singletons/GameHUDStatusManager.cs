using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class GameHUDStatusManager : MonoBehaviour
{
    public TMP_Text nameMon;
    public TMP_Text[] attackText;
    public TMP_Text[] defenseText;
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text staminaText;
    //Stance doenst grow for now
    public TMP_Text stanceText;
    public TMP_Text levelText;
    public TMP_Text neededExperience;
    public Image xpBar;

    public static GameHUDStatusManager Singleton;
    TMP_Text[] allAtributes = new TMP_Text[15];
    private MonGame currentMon = null;
    public delegate void OnLevelGained(Image xpBar,TMP_Text xpNeededText,MonGame mon);
    public event OnLevelGained eventOnLevelGained;

    void Start()
    {
        if (Singleton == null)
            Singleton = this;
        

        allAtributes[0] = nameMon;
        allAtributes[1] = levelText;
        for (int i = 0; i < 4; i++)
        {
            allAtributes[i+2] = attackText[i];
        }
        for (int i = 0; i < 4; i++)
        {
            allAtributes[i+6] = defenseText[i];
        }

        allAtributes[10] = speedText;
        allAtributes[11] = healthText;
        allAtributes[12] = staminaText;
        allAtributes[13] = stanceText;
        allAtributes[14] = neededExperience;

        ScaleDown();
    }
    public void SetText(MonGame mon)
    {
        //Store it locally so it can be used outside this function
        currentMon = mon;

        //Start displaying the elements one by one
        ToggleDisplay(true);


        for (int i = 0; i < 4; i++)
        {
            attackText[i].SetText( mon.currentAttackType.arrayAtributtes[i].ToString());
        }
        
        for (int i = 0; i < 4; i++)
        {
            defenseText[i].SetText( mon.currentDefenseType.arrayAtributtes[i].ToString());
        }
        nameMon.SetText(mon.GetNameMon());
        speedText.SetText(mon.SpeedStarting.ToString());
        healthText.SetText(mon.maxHealth.ToString());
        staminaText.SetText(mon.maxStamina.ToString());
        stanceText.SetText(mon.StanceStarting.ToString());
        levelText.SetText(mon.level.ToString());

        var xpNeeded =TurnAddLevel.GetNextLevelNeededExperience(mon);
        neededExperience.SetText( ( xpNeeded - mon.experiencePoints ).ToString());
        xpBar.fillAmount = mon.experiencePoints / xpNeeded ;
    }

    public void ToggleDisplay(bool isDisplaying)
    {
        GameUILoader.Singleton.EnableDisplayStatsAfterMonLevelUp(isDisplaying);

        if (!isDisplaying)
        {
            ScaleDown();
            return;
        }
        
        NextPopUp();
    }

    private void ScaleDown()
    {
        foreach (var item in allAtributes)
            {
                LeanTween.scale(item.gameObject,Vector3.zero,1);
            }
    }


    private int nextPopUpNum = 0;
    public const float delay = .2f;
    private void NextPopUp()
    {
        if (nextPopUpNum == allAtributes.Length)
        {
            nextPopUpNum = 0;

            //Raise Event to be handled in class TurnAddLevel
            if (eventOnLevelGained != null)
                eventOnLevelGained(xpBar,neededExperience,currentMon);
            
            currentMon = null;
            return;
        }

        LeanTween.scale(allAtributes[nextPopUpNum].gameObject,Vector3.one,delay).setOnComplete(NextPopUp);
        nextPopUpNum++;
    }

    public async Task  UpdateDisplayLevelUpMon(MonGame mon)
    {   
        var monADN = mon.monADN;

        for (int i = 0; i < 4; i++)
        {
            mon.currentAttackType.arrayAtributtes[i] = GetStat(mon.level,(int) monADN.baseAttackType.arrayAtributtes[i]);
            UpdateText(attackText[i],(int) mon.currentAttackType.arrayAtributtes[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            mon.currentDefenseType.arrayAtributtes[i] = GetStat(mon.level,(int) monADN.baseDefenseType.arrayAtributtes[i]);
            UpdateText(defenseText[i],(int) mon.currentDefenseType.arrayAtributtes[i]);
        }
        
        mon.SpeedStarting = GetStat(mon.level,monADN.baseSpeed);
        UpdateText(speedText,(int) mon.SpeedStarting);
        
        mon.maxHealth = GetStatHealth(mon.level,monADN.baseMaxHealth);
        UpdateText(healthText,(int) mon.maxHealth);
        
        mon.level += 1;
        UpdateText(levelText,mon.level);


        mon.GetMonMeshManager().levelText.SetText(mon.level.ToString());
        
        await TextDialogManager.Singleton.PushTextAwaitKey("Your " + mon.GetNameMon() + " reached level " + mon.level + "!");
    }


    private static void UpdateText(TMP_Text text,int currentStat)
    {
        if (currentStat <= 0)
            return;
    
        text.SetText(currentStat.ToString());        
        text.color = Color.green;
    }


    private static int GetStat(int level,int baseStat)
    {
        //Current level cap is 100
        float percent = level/100f;

        return (int) ( (baseStat * 4) * percent ) + 5;
    }

    private static int GetStatHealth(int level,int baseStat)
    {
        //Current level cap is 100
        float percent = level/100f;

        return (int) ( (baseStat * 4) * percent ) + 10 + level;
    }

}
