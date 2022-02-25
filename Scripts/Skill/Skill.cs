using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject{
    public String nameSkill;
    public float stamina;
    public String description;
    public MonTypes.Type type;
    public AudioClip soundEffect;
}
