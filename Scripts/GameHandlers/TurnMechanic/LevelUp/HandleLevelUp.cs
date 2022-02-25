using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HandleLevelUp
{
    private const int MAX_SIZE=4;
    private const double BASIC_MEAN=1;

    public const double HP_MEAN =3;

    internal static async Task Handle(MonGame mon, TMP_Text levelText, TMP_Text attackText, TMP_Text defenseText,
    TMP_Text speedText, TMP_Text healthText, TMP_Text staminaText,TMP_Text[] allGains)
    {
        System.Random rand = new System.Random(); //reuse this if you are generating many
        int[] randoArr = new int[MAX_SIZE];
        for (int i = 0; i < MAX_SIZE; i++)
        {
            //TODO
            //Find a better strategy to implement on adding stats upon level up
            //Like mon should have prefered type to gain more stats
            randoArr[i] = (int) GetRandomNormalDistribution(BASIC_MEAN,BASIC_MEAN,rand);
        }
        
        UpdateText(attackText,randoArr[0],allGains[0]);
        mon.AttackCurrent += randoArr[0];
        
        UpdateText(defenseText,randoArr[1],allGains[1]);
        mon.DefenseCurrent += randoArr[1];

        UpdateText(speedText,randoArr[2],allGains[2]);
        mon.SpeedCurrent += randoArr[2];

        //Stamina text por enquanto pode ser descartado
        // UpdateText(staminaText,randoArr[3]);
        // mon.AttackCurrent += randoArr[3];
        
        int hpIncrease =(int) GetRandomNormalDistribution(HP_MEAN,HP_MEAN,rand);
        UpdateText(healthText,hpIncrease,allGains[3]);
        mon.currentHealth += hpIncrease;
        
        UpdateText(levelText,1,null);
        mon.level += 1;
        mon.GetMonMeshManager().levelText.SetText(mon.level.ToString());
        
        await TextDialogManager.Singleton.PushText("Your " + mon.GetNameMon() + " reached level " + mon.level + "!");

    }

    private static void UpdateText(TMP_Text levelText,int increase,TMP_Text gainText)
    {
        if (increase <= 0)
            return;
        levelText.SetText((Int32.Parse(levelText.text) + increase).ToString());
        
        if (gainText != null)
        {
            gainText.transform.gameObject.SetActive(true);
            gainText.SetText("+"+increase.ToString());
        }
        
        levelText.color = Color.green;
    }

    public static double GetRandomNormalDistribution(double mean,double stdDev,System.Random rand)
    {
        double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0-rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =   mean + stdDev * randStdNormal;
        return randNormal;
    }
}