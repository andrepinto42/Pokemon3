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
        UpdateVisualMon(mon);

        // Setup the delegates Functions
        ally.delegateDamageChange = (string value) =>ChangeTextIncreaseDisplay(storedAttack,value,_textAttack,_textIncreaseAttack);
        ally.delegateDefenseChange = (string value) =>ChangeTextIncreaseDisplay(storedDefense,value,_textDefense,_textIncreaseDefense);
        ally.delegateSpeedChange = (string value) =>ChangeTextIncreaseDisplay(storedSpeed,value,_textSpeed,_textIncreaseSpeed);
    }

    void ChangeTextIncreaseDisplay(float previousValue,string newValue,TMP_Text textoMain,TMP_Text increaseText)
    {
        float increase = float.Parse(newValue);

        // Round the number 
        float value = Mathf.Round(increase * previousValue );
        
        // Round the number up to 2 decimal places
        increase = Mathf.Round(increase * 100f) / 100f;
        
        //x0.125
        increaseText.gameObject.SetActive(true);
        increaseText.SetText("x"+increase);

        textoMain.SetText( value.ToString());
    }
    void ChangeTextDisplay(string newValue,TMP_Text texto)
    {
        texto.SetText(newValue);
    }

    public void UpdateVisualMon(MonGame mon){
        ChangeTextDisplay(mon.AttackCurrent.ToString(),_textAttack);
        ChangeTextDisplay(mon.DefenseCurrent.ToString(),_textDefense);
        ChangeTextDisplay(mon.SpeedCurrent.ToString(),_textSpeed);
        ChangeTextDisplay(mon.maxStance.ToString(),_textStance);
        
        //BUG might be here
        ChangeTextDisplay(mon.currentHealth.ToString(),_textHealth);
        ChangeTextDisplay(mon.currentStamina.ToString(),_textStamina);
        
        //Disable the text of increasing Status
        _textIncreaseAttack.gameObject.SetActive(false);
        _textIncreaseDefense.gameObject.SetActive(false);
        _textIncreaseSpeed.gameObject.SetActive(false);

        //Store it locally the initial values of the mon Stats
        storedAttack = mon.AttackCurrent;
        storedDefense = mon.DefenseCurrent;
        storedSpeed = mon.SpeedCurrent;
    }

}
