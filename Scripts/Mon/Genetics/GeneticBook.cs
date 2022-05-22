public class GeneticBook
{
    public enum GeneticTypes
    {
        NoAttackLost,//FierceNoDefenseLost
        NoDefenseLost,//cant lose defense HardenScale
        NoSpeedLost,//cant lose speed Runner
        Glorius // Glorius
    }
    public static SkillHandlerStatusBuff.DataHandlerStatusBuff GeneticDecreaseStat(BuffStatus buffStatus, MonManager changingMon)
    {
        //If the mon is getting its status raised do nothing;
        if (buffStatus.increase >= 1f)
            return null;
        Genetic genetic = changingMon.MonMain.GetMonGenetic();
        
        //Mon doesnt have a genetic
        if (genetic == null)
            return null;
        
        //If the genetic of the mon doesnt allow for the ATK to be lowered
        if (buffStatus.effect == SkillBuff.Stat.ATTACK &&  (genetic.nameGenetic == GeneticTypes.NoAttackLost))
            return BuildMessage("Attack",genetic);
        
        else if(buffStatus.effect == SkillBuff.Stat.DEFENSE && (genetic.nameGenetic == GeneticTypes.NoDefenseLost) )
            return BuildMessage("Defense",genetic);
        
        else if(buffStatus.effect == SkillBuff.Stat.SPEED && (genetic.nameGenetic == GeneticTypes.NoSpeedLost) )
            return BuildMessage("Speed",genetic);
        
        else
            return null;

    }

    private static SkillHandlerStatusBuff.DataHandlerStatusBuff BuildMessage(string type,Genetic gen)
    {
        string message1 = type +" can't be lowered because of ";
        string message2 =ConvertGeneticToString(gen);
        string message3 =" Genetic !";
        int startIndex = message1.Length;
        int endIndex = startIndex + message2.Length;
        var dwc = new SkillHandlerStatusBuff.DataMessageWithColor("red",startIndex,endIndex);
        return new SkillHandlerStatusBuff.DataHandlerStatusBuff(message1 + message2 + message3,dwc);
    }

    public static string ConvertGeneticToString(Genetic gen)
    {
        return gen.geneticName;
    }
}