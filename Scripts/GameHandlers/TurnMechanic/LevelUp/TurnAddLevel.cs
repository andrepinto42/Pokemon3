using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine;
using System;
using TMPro;
public class TurnAddLevel
{
    private const int TICKS = 50;
    public async static void AddLevelPokemon(Image xpBar,TMP_Text xpNeededText,MonGame mon)
    {
        //Need to await this one because the text might not be ended its display :(
        await ExperienceManager.taskDiscoverExperienceGained;

        int XP_Gained = ExperienceManager.GetExperienceGained();

        int XP_Needed = GetNextLevelNeededExperience(mon);
        int XP_Current = mon.experiencePoints;
        float TICKSFLOAT = (float) TICKS;
        Debug.Log("XP CURRENT -> " + XP_Current + " needed " + XP_Needed + " gained " + XP_Gained);
    
        //Delay is about 1s to load an entire xp Bar
    
        while (XP_Gained > 0)
        {
            XP_Needed = GetNextLevelNeededExperience(mon);
            //The more xp is exceed from the needed the faster the animation will be
            double speedUp =   (XP_Gained /  (float) XP_Needed) + 1f;
            int delay =  (int)  (1000 / (speedUp * TICKS) ); // 300fps = 0,003 s

            var leftoverXp = XP_Needed - XP_Current;
            
            //If the xpgained doesnt level up the mon then it should be the value to use to fill the bar
            if (XP_Gained<leftoverXp )
            {
                leftoverXp = XP_Gained;
            }
            // Debug.Log("Leftover XP = " +leftoverXp);
            for (int i = 0; i <= TICKS; i++)
            {
                var xpI = (leftoverXp * (i/ TICKSFLOAT) ) + XP_Current;
                xpBar.fillAmount =  xpI / XP_Needed;

                // Debug.Log(xpI + " / " + xpNeeded + " | i =" + i + " / " + TICKSFLOAT);
                xpNeededText.SetText( ( (int) (XP_Needed- xpI) ).ToString() );
                
                await Task.Delay(delay);
                // await Task.Delay((int)  (Time.deltaTime * delay) );
            }
            XP_Gained -= leftoverXp;
            
            if (XP_Gained > 0)
            {
                xpBar.fillAmount = 0;
                XP_Current = 0;

                await GameHUDStatusManager.Singleton.UpdateDisplayLevelUpMon(mon);
            }
            else
            {
                //Add the leftoverXP
                mon.experiencePoints = XP_Gained + leftoverXp+ XP_Current;
            }
        }
        XP_Gained =0;
        GameHUDStatusManager.Singleton.eventOnLevelGained -= AddLevelPokemon;
        
        //Waiting just one second so the information can be processed by the user
        await Task.Delay(1000);

        //After the Mon gains XP send the next Mon or end the battle
        GameStatusManager.Singleton.SendNextMon(mon);
    }   

    public static int GetNextLevelNeededExperience(MonGame mon)
    {
        return ExperienceManager.GetTotalExperience(mon,mon.level+1) - ExperienceManager.GetTotalExperience(mon,mon.level);
    }
  
}