using UnityEngine;
using UnityEngine.UI;
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
    public Image _monImage;
 
    void ChangeTextDisplay(float newValue,TMP_Text texto)
    {
        texto.SetText(newValue.ToString());
    }


    void ChangeTextDisplay(float newValue,float maxValue ,TMP_Text texto)
    {
        texto.SetText( newValue.ToString()  + "/"+ maxValue.ToString() );
    }

    void ChangeTextMultiplier(float newValue,float maxValue ,TMP_Text texto)
    {
        float increase = Mathf.Round((newValue/maxValue) * 100f) / 100f;
        if (increase != 1f)
        {
            string color = (increase > 1f) ? "red" : "blue"; 
            string novoTexto = newValue +" <color=\""+ color +"\">x" + increase.ToString();
            texto.SetText(novoTexto);
        }
        else
        {
            texto.SetText(newValue.ToString());
        }
        
    }
    public void UpdateVisualMon(MonGame mon){
 
        ChangeTextMultiplier(mon.currentSpeed,mon.SpeedStarting,_textSpeed);
        ChangeTextDisplay(mon.currentStance, 100f,_textStance);
        ChangeTextDisplay(mon.currentHealth,mon.maxHealth,_textHealth);
        ChangeTextDisplay(mon.currentStamina,mon.maxStamina,_textStamina);
        
        for (int i = 0; i < SYZE_TYPES; i++)
        {
            ChangeTextDisplay(mon.currentAttackType.arrayAtributtes[i],_textAttack[i]);
        }
        for (int i = 0; i < SYZE_TYPES; i++)
        {
            ChangeTextDisplay(mon.currentDefenseType.arrayAtributtes[i],_textDefense[i]);
        }

        _monImage.sprite = mon.monADN.GetImage();
    }

}
