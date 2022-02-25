using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trainer", menuName = "Person/New Trainer", order = 1)]
public class Trainer : ScriptableObject{
    public String nameTrainer;
    public MonGame[] allMons = new MonGame[4];

}