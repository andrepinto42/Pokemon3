using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class GameHUDStatusManager : MonoBehaviour
{
    public TMP_Text nameMon;
    public TMP_Text attackText;
    public TMP_Text defenseText;
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text staminaText;
    public TMP_Text luckText;
    public TMP_Text levelText;
    public TMP_Text neededExperience;
    public Image xpBar;

    public static GameHUDStatusManager Singleton;
    TMP_Text[] allAtributes = new TMP_Text[9];
    TMP_Text[] allGains = new TMP_Text[6];//SO existem 6 atributos para serem modificados
    private MonGame currentMon = null;
    public delegate void OnLevelGained(Image xpBar,TMP_Text xpNeededText,MonGame mon);
    public event OnLevelGained eventOnLevelGained;

    void Start()
    {
        if (Singleton == null)
            Singleton = this;
        

        allAtributes[0] = nameMon;
        allAtributes[1] = levelText;
        allAtributes[2] = attackText;
        allAtributes[3] = defenseText;
        allAtributes[4] = speedText;
        allAtributes[5] = healthText;
        allAtributes[6] = staminaText;
        allAtributes[7] = luckText;
        allAtributes[8] = neededExperience;
        
        for (int i = 0; i < allAtributes.Length -3; i++)
        {
            allGains[i] = allAtributes[i+2].transform.GetChild(0).GetComponent<TMP_Text>();
            allGains[i].transform.gameObject.SetActive(false);   
        }
        
        ToggleDisplay(false);
    }
    public void SetText(MonGame mon)
    {
        //Store it locally so it can be used outside this function
        currentMon = mon;

        //Start displaying the elements one by one
        ToggleDisplay(true);

        nameMon.SetText(mon.GetNameMon());
        attackText.SetText(mon.AttackCurrent.ToString());
        defenseText.SetText(mon.DefenseCurrent.ToString());
        speedText.SetText(mon.maxSpeed.ToString());
        healthText.SetText(mon.maxHealth.ToString());
        staminaText.SetText(mon.maxStamina.ToString());
        luckText.SetText("420");
        levelText.SetText(mon.level.ToString());
        var xpNeeded =TurnAddLevel.GetNextLevelNeededExperience(mon);
        neededExperience.SetText( ( xpNeeded - mon.experiencePoints ).ToString());
        xpBar.fillAmount = mon.experiencePoints / xpNeeded ;
    }

    public void ToggleDisplay(bool isDisplaying)
    {
        nameMon.gameObject.transform.parent.gameObject.SetActive(isDisplaying);
        attackText.gameObject.transform.parent.gameObject.SetActive(isDisplaying);
        
        if (!isDisplaying)
        {
            foreach (var item in allAtributes)
            {
                LeanTween.scale(item.gameObject,Vector3.zero,1);
            }
            return;
        }
        
        NextPopUp();
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

        await HandleLevelUp.Handle(mon,levelText,attackText,defenseText,speedText,healthText,staminaText,allGains);
    }

   
}
