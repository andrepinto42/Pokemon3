using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffStatus{
    public float increase = 1f;
    public bool ally;
   
    public SkillBuff.Stat effect;
}

[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Buff", order = 1)]
public class SkillBuff : Skill{
    //TODO change this to an array so it can suport more than 1 buff
    public BuffStatus buffStatus;

    public enum Stat
    {
        ATTACK,
        DEFENSE,
        SPEED
    }
}