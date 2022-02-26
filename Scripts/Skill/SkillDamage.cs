using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Mon", menuName = "Mon/Skill/Damage", order = 1)]
public class SkillDamage : Skill{
    public float damage;

    public BuffStatus[] buffStatus;
  
}