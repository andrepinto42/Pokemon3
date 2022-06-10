using UnityEngine;
using TMPro;

public class GameMonStatsDisplay : MonoBehaviour
{
    const int SYZE_TYPES = 4;
    public TMP_Text _textHealth;
    public TMP_Text _textStamina;
    public TMP_Text _textStance;
    public TMP_Text _textSpeed;
    public TMP_Text[] _textDefense ;
    public TMP_Text[] _textAttack ;
 
    void ChangeTextDisplay(string newValue,TMP_Text texto)
    {
        texto.SetText(newValue);
    }

    public void UpdateVisualMon(MonGame mon){
 
        ChangeTextDisplay(mon.maxSpeed.ToString(),_textSpeed);
        ChangeTextDisplay(mon.maxStance.ToString(),_textStance);
        
        //BUG might be here
        ChangeTextDisplay(mon.currentHealth.ToString(),_textHealth);
        ChangeTextDisplay(mon.currentStamina.ToString(),_textStamina);
        
        for (int i = 0; i < SYZE_TYPES; i++)
        {
            ChangeTextDisplay(mon.baseAttackType.arrayAtributtes[i].ToString(),_textAttack[i]);
        }
        for (int i = 0; i < SYZE_TYPES; i++)
        {
            ChangeTextDisplay(mon.baseDefenseType.arrayAtributtes[i].ToString(),_textDefense[i]);
        }
        
    }

}
