using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrainerHandler
{
    internal static void ResetMonTrainer(Trainer trainer)
    {
        for (int i = 0; i < trainer.allMons.Length; i++)
        {
            MonGame mon = trainer.allMons[i];
            
            if (mon == null) return;

            RestartStatsMon(mon);
        }
    }

    public static void RestartStatsMon(MonGame mon)
    {
        mon.currentHealth = mon.maxHealth;
        mon.currentStamina = mon.maxStamina;
        mon.currentStance = mon.maxStance;
        mon.attackBuff = 1f;
        mon.defenseBuff = 1f;
        mon.speedBuff = 1f;
    }
}