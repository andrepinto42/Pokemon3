using System.Threading.Tasks;
using System;

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
                changingMon.ChangeBaseDamage(buffStatus.increase,buffStatus.typeOfDamage);
                return MessageToDisplay(changingMon,buffStatus.typeOfDamage,"attack",buffStatus.increase);
    
            case SkillBuff.Stat.DEFENSE:
                changingMon.ChangeBaseDefense(buffStatus.increase,buffStatus.typeOfDamage);
                return MessageToDisplay(changingMon,buffStatus.typeOfDamage,"defense",buffStatus.increase);
    
            
            case SkillBuff.Stat.SPEED:
                changingMon.ChangeBaseSpeed(buffStatus.increase);
                return MessageToDisplay(changingMon,"speed",buffStatus.increase);

            default:
                return null;
        }
    }

    private static DataHandlerStatusBuff MessageToDisplay(MonManager monManager,TypeOfDamage typeOfDamage ,string status,float increase)
    {
        string increaseText = increase > 1f ? "risen" : "decreased";
        
        //Convert the type from PIERCE to Pierce
        char[] type = typeOfDamage.ToString().ToCharArray();
        for (int i = 1; i < type.Length; i++)
        {
            type[i] = Char.ToLower( type[i]);
        }

        string s1 = monManager.MonMain.GetNameMon() + " ";
        string typeString = new string(type);
        int startIndex = s1.Length;
        int endIndex = startIndex + typeString.Length;

        string s2 =" " + status + " has " + increaseText + " !";
        
        //Store the data in this variable so it can be later read by the textDialogManager
        var dmwc = new DataMessageWithColor(status.Equals("attack") ? "red" : "blue",startIndex,endIndex);
        
        return new DataHandlerStatusBuff(s1+typeString+s2,dmwc);
    }
     private static DataHandlerStatusBuff MessageToDisplay(MonManager monManager ,string status,float increase)
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