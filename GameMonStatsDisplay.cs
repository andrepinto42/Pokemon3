using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameStatusManager))]
public class GameMonStatsDisplay : MonoBehaviour
{
    public TMP_Text _textHealth;
    public TMP_Text _textStamina;
    public TMP_Text _textStance;
    public TMP_Text _textDefense;
    public TMP_Text _textAttack;
    public TMP_Text _textSpeed;
    public TMP_Text _textIncreaseAttack;
    public TMP_Text _textIncreaseDefense;
    public TMP_Text _textIncreaseSpeed;
    GameStatusManager gameStatusManager;
    MonManager ally;
    float storedAttack;
    float storedDefense;
    float storedSpeed;

    void Awake()
    {
        gameStatusManager = GetComponent<GameStatusManager>();
        ally = gameStatusManager.ally;
    }
    void Start()
    {
        
        // Initialize the value of the text of display of the Mon status
        var mon = ally.MonMain;
        HandleChange(mon.AttackCurrent.ToString(),_textAttack);
        HandleChange(mon.DefenseCurrent.ToString(),_textDefense);
        HandleChange(mon.SpeedCurrent.ToString(),_textSpeed);
        HandleChange(mon.maxStance.ToString(),_textStance);
        HandleChange(mon.currentHealth.ToString(),_textHealth);
        HandleChange(mon.currentStamina.ToString(),_textStamina);

        storedAttack = mon.AttackCurrent;
        storedDefense = mon.DefenseCurrent;
        storedSpeed = mon.SpeedCurrent;

        // Setup the delegates Functions
        ally.delegateDamageChange = (string value) =>HandleChange(storedAttack,value,_textAttack,_textIncreaseAttack);
        ally.delegateDefenseChange = (string value) =>HandleChange(storedDefense,value,_textDefense,_textIncreaseDefense);
        ally.delegateSpeedChange = (string value) =>HandleChange(storedSpeed,value,_textSpeed,_textIncreaseSpeed);
    }

    void HandleChange(float previousValue,string newValue,TMP_Text textoMain,TMP_Text increaseText)
    {
        // Round the number up to 2 decimal places 
        float value = Mathf.Round(float.Parse(newValue) * previousValue * 100f) / 100f;
        
        //x0.125
        increaseText.SetText("x"+newValue);
        textoMain.SetText( value.ToString());
    }
    void HandleChange(string newValue,TMP_Text texto)
    {
        texto.SetText(newValue);
    }

}
