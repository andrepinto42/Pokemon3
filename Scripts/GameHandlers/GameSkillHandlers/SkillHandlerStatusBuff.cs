using System.Threading.Tasks;

public class SkillHandlerStatusBuff  
{
    public class DataMessageWithColor
    {
        public string color;
        public int indexStart;
        public int indexEnd;
        public DataMessageWithColor(string c,int i1,int i2)
        {
            color = c;
            indexStart = i1;
            indexEnd = i2;
        }
    }
    public class DataHandlerStatusBuff{
        public string message;
        public DataMessageWithColor dataColor;
        
        internal DataHandlerStatusBuff(string m,DataMessageWithColor data1 = null)
        {
            message = m;
            dataColor = data1;
        }
        
    }
    public static DataHandlerStatusBuff HandleBuff(MonManager ally,MonManager enemy,BuffStatus buffStatus)
    {
        //A little un-optimized but code looks better this way
        MonManager changingMon = (buffStatus.ally) ? ally : enemy;

        //Check if the mon has a genetic that can override the default behaviour.
        var dataGenetic = GeneticBook.GeneticDecreaseStat(buffStatus,changingMon);
        if (dataGenetic != null)
            return dataGenetic;

        //Trigger the particle system
        if (buffStatus.ally) 
            GameVisualEffectsHandler.Singleton.AllyStartSpinning(buffStatus,changingMon);
        else
            GameVisualEffectsHandler.Singleton.EnemyStartSpinning(buffStatus,changingMon);

        switch(buffStatus.effect)
        {
            case SkillBuff.Stat.ATTACK:
                changingMon.ChangeBaseDamage(buffStatus.increase);
                return MessageToDisplay(changingMon,"attack",buffStatus.increase);
    
            case SkillBuff.Stat.DEFENSE:
                changingMon.ChangeBaseDefense(buffStatus.increase);
                return MessageToDisplay(changingMon,"defense",buffStatus.increase);
    
            
            case SkillBuff.Stat.SPEED:
                changingMon.ChangeBaseSpeed(buffStatus.increase);
                return MessageToDisplay(changingMon,"speed",buffStatus.increase);

            default:
                return null;
        }
    }

    private static DataHandlerStatusBuff MessageToDisplay(MonManager monManager,string status,float increase)
    {
        string increaseText = increase > 1f ? "risen" : "decreased";
        string s =  monManager.MonMain.GetNameMon() + " " + status + " has " + increaseText + " !";
        return new DataHandlerStatusBuff(s);
    }

    public static async Task PushMessage(DataHandlerStatusBuff data)
    {
        if (data.dataColor != null)
        {
            await TextDialogManager.Singleton.PushText(data);        
            return;
        }
        await TextDialogManager.Singleton.PushText(data);        
    }
}