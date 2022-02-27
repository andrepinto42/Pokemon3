using System;
using System.Collections.Generic;
public class GeneticBook
{
    public enum GeneticTypes
    {
        Fierce,
        HardenScale,
        Runner,
        Glorius
    }
    public static string GeneticDecreaseStat(BuffStatus buffStatus, MonManager changingMon)
    {
        //If the mon is getting its status raised do nothing;
        if (buffStatus.increase >= 1f)
            return null;
        Genetic genetic = changingMon.MonMain.GetMonGenetic();
        switch(buffStatus.effect){
            case SkillBuff.Stat.ATTACK:
                //If the genetic of the mon doesnt allow for the ATK to be lowered
                if (genetic.nameGenetic == GeneticTypes.Fierce)
                    return "Attack can't be lowered because of "+ ConvertGeneticToString(genetic) + " Genetic !";
                break;
            case SkillBuff.Stat.DEFENSE:
                if (genetic.nameGenetic == GeneticTypes.HardenScale)
                    return "Defense can't be lowered because of "+ ConvertGeneticToString(genetic) + " Genetic !";
                break;
            case SkillBuff.Stat.SPEED:
                if (genetic.nameGenetic == GeneticTypes.Runner)
                    return "Speed can't be lowered because of "+ ConvertGeneticToString(genetic) + " Genetic !";
                break;
            default:
                return "Status not found to display";
        }

        return null;
    }


    public static string ConvertGeneticToString(Genetic gen)
    {
        string s = gen.nameGenetic.ToString();
        
        for (int i = 1; i < s.Length; i++)
        {
            if (System.Char.IsUpper(s[i]) )
            {
                s = s.Substring(0,i) + " " + s.Substring(i,s.Length - i);
                i++;
            }
        }

        return s;

    }
}