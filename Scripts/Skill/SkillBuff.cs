using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Buff", order = 1)]
public class SkillBuff : Skill{
    public float increase;
    public bool ally;
   
    public Stat effect;

    public enum Stat
    {
        ATTACK,
        DEFENSE,
        SPEED
    }
}