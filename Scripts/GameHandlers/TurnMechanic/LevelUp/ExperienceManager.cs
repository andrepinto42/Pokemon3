using System;
using System.Threading.Tasks;
public class ExperienceManager
{
    public static Task taskDiscoverExperienceGained;
    static int xpGained = 0;
    //if the battle takes 3 turn to end, then the mon receives the most xp,
    //more turns or less reduce the XP Gain;
    const int MAX_TURN_COUNTER_YIELD = 3;
    //TODO
    internal async static void DiscoverExperienceGained(MonManager killer, MonManager victim,int counter)
    {
        int baseKiller = killer.MonMain.GetBaseExperience();
        int baseVictim = victim.MonMain.GetBaseExperience();
        int levelKiller = killer.MonMain.level;
        int levelVictim = victim.MonMain.level;

        //https://bulbapedia.bulbagarden.net/wiki/Experience
        //BaseXP 110 yields the same base XP as Charizard
        double xpGainedFromVictim =  ConvertBaseExperience(baseVictim) * levelVictim / ( 5f * 1);
        double xpGainedFromKiller = Math.Pow((2 * levelVictim + 10) / (levelVictim + levelKiller + 10f),2.5f);
        
      
        //Describes a curve that rapidly increases and then it falls when counter = 4
        double curveExperienceGainByTurn = (-counter * counter + MAX_TURN_COUNTER_YIELD*2*counter) * 0.5f;
        double xpCounter =  Math.Max(curveExperienceGainByTurn,0.5f);
        xpGained = (int) (xpGainedFromKiller * xpGainedFromVictim * xpCounter);        

        string message = "By defeating " + victim.MonMain.GetNameMon() + ", " 
                        + killer.MonMain.GetNameMon() + " gained " + xpGained +" XP";
        taskDiscoverExperienceGained= TextDialogManager.Singleton.PushText(message);
        await taskDiscoverExperienceGained;
    }

    public static int GetExperienceGained()
    {
        int xp = xpGained;
        xpGained =0;
        return xp;
    }

    public static int GetTotalExperience(int baseExperience,int currentLevel)
    {
        //https://bulbapedia.bulbagarden.net/wiki/Experience
        //Currently its the fast speed method
        double xp = Math.Pow(currentLevel,3) * (baseExperience / 100f) ;
        return (int) xp;
        
        //Retornava valores demasiado grandes...
        // double totalXP = baseExperience * Math.Pow(1.5f,currentLevel -1);
        // return (int) totalXP;
    }
    public static int GetTotalExperience(MonGame mon,int currentLevel)
    {
        return GetTotalExperience(mon.GetBaseExperience(),currentLevel);
    }

    // Para baseExperience = 50 -> 39
    // Para baseExperience = 150 -> 315
    // Descreve uma curva exponencial
    public static float ConvertBaseExperience(float baseExperience)
    {
        return   0.0184f * (float) Math.Pow(baseExperience,2) - 0.92f * baseExperience + 39f;
    }
}