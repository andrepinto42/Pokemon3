public class MonCalculateDamage{

    static float lowerBoundRandom = 0.9f;
    static float upperBoundRandom = 1f;
    /*
    Damage calculated from here
    https://bulbapedia.bulbagarden.net/wiki/Damage
    */
    public static int CalculateDamage(SkillDamage skillDamage,MonGame ally,MonGame enemy,
                                      MonManager allyManager,MonManager enemyManager)
    {
        float level = (2*ally.level / 5f) + 2;
        
        float powerDamage = CalculatePowerDamage(skillDamage,ally,enemy,allyManager,enemyManager);
        
        float damageBase = ((level * powerDamage)/25f) +2;
        
        //Numero random [lowerboundRandom,upperboundRandom]
        float random  = (float) (new System.Random().NextDouble() * (upperBoundRandom - lowerBoundRandom) + lowerBoundRandom);
        
        float advantage = MonTypes.GetAdvantage(skillDamage.type,enemy.GetTypeMon());
        
        return (int) ( damageBase * random * advantage);
    }

    public static float CalculatePowerDamage(SkillDamage skillDamage,MonGame ally,MonGame enemy,
                                      MonManager allyManager,MonManager enemyManager)
    {
        //Find the type of the damage
        //Ex: PIERCE OR WARCRY
        int typeDamage = (int)skillDamage.typeOfDamage;

        //buffDefenseType varies between ]0,2] normally
        float defenseEnemy = 
        enemy.currentDefenseType.arrayAtributtes[typeDamage] *  enemy.defenseTypeBuff.arrayAtributtes[typeDamage];

        //attackBuff varies between ]0,2] normally
        float attackAlly = 
        skillDamage.damage * 
        ally.currentAttackType.arrayAtributtes[typeDamage] * ally.attackTypeBuff.arrayAtributtes[typeDamage];

        return  attackAlly / defenseEnemy;

    }
}