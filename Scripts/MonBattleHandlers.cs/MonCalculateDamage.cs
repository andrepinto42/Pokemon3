using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonCalculateDamage{

    static float lowerBoundRandom = 0.9f;
    static float upperBoundRandom = 1f;
    /*
    Damage calculated from here
    https://bulbapedia.bulbagarden.net/wiki/Damage
    */
    public static int CalculateDamage(SkillDamage skillDamage,MonGame ally,MonGame enemy){
        float level = (2*ally.level / 5f) + 2;
        //defenseBuff and attackBuff varies between ]0,2] normally
        float powerDamage = skillDamage.damage * ally.attackBuff  * ally.AttackCurrent / (enemy.DefenseCurrent *  enemy.defenseBuff)  ;
        float damageBase = ((level * powerDamage)/25f) +2;
        
        //Numero random [lowerboundRandom,upperboundRandom]
        float random  = (float) (new System.Random().NextDouble() * (upperBoundRandom - lowerBoundRandom) + lowerBoundRandom);
        
        float advantage = MonTypes.GetAdvantage(skillDamage.type,enemy.type);
        
        return (int) ( damageBase * random * advantage);
    }
}