using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrainerHandler
{
    internal static void ResetMonTrainer(Trainer enemyTrainer)
    {
        for (int i = 0; i < enemyTrainer.allMons.Length; i++)
        {
            MonGame mon = enemyTrainer.allMons[i];
            
            if (mon == null) return;

            RestartStatsMon(mon);
        }
    }

    public static void RestartStatsMon(MonGame mon)
    {
        mon.currentHealth = mon.maxHealth;
        mon.currentStamina = mon.maxStamina;
        mon.attackBuff = 1f;
        mon.defenseBuff = 1f;
        mon.speedBuff = 1f;
    }
}