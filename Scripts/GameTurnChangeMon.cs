using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameTurnChangeMon : MonoBehaviour
{
    public async Task<bool> HandleNextEnemyMon(MonManager enemy, Trainer enemyTrainer)
    {
        List<MonGame> allValidMons = GetValidMons(enemyTrainer);
        if (allValidMons.Count == 0)
            return false;
        
        //TODO
        //Find the next best option to fight the current pokemon
        MonGame nextMon = allValidMons[0];

        enemy.KillGameObject();
        enemy.MonMain = nextMon;
        TrainerHandler.RestartStatsMon(nextMon);
        enemy.InitializeMeshMon();
        GameHUDStatusManager.Singleton.ToggleDisplay(false);

        await TextDialogManager.Singleton.PushText(enemyTrainer.nameTrainer + " sent "
         + nextMon.GetNameMon() + " to the battlefield!",1000);
        enemy.gameObject.SetActive(true);
        
        //Update the HUD
        GameStatusManager.Singleton.AllyDeselectsAttack();
        return true;
    }


    //TODO
    //Generate some HUD to display the next MON
    public bool HandleNextAllyMon(MonManager ally, Trainer allyTrainer)
    {
        return true;
    }

    
    private static List<MonGame> GetValidMons(Trainer enemyTrainer)
    {
        List<MonGame> validMons = new List<MonGame>();

        for (int i = 0; i < enemyTrainer.allMons.Length; i++)
        {
            MonGame mon = enemyTrainer.allMons[i];
            if (mon == null)
                continue;
            
            if (mon.currentHealth > 0)
                validMons.Add(mon);
        }

        return validMons;
    }
}
