using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

[RequireComponent(typeof(HandleAnimations),typeof(TurnMechanicMon))]
public class MonMeshManager : MonoBehaviour
{
    [HideInInspector] public MonManager monManager;
    public Transform pivotHUD;
    [HideInInspector]public Image healthImage;
    [HideInInspector]public Image staminaImage;
    [HideInInspector]public TMP_Text textNameMon;
    [HideInInspector]public TMP_Text damageHealthText;
    [HideInInspector]public TMP_Text levelText;

    //Carefull with the child positions in this object
    void Awake()
    {
        healthImage = pivotHUD.GetChild(1).GetChild(1).GetComponent<Image>();
        staminaImage = pivotHUD.GetChild(2).GetChild(1).GetComponent<Image>();
        textNameMon = pivotHUD.GetChild(3).GetComponent<TMP_Text>();
        damageHealthText = pivotHUD.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        levelText = pivotHUD.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }
    void Start()
    {
        damageHealthText.transform.gameObject.SetActive(false);
    }

    void Update()
    {
        pivotHUD.LookAt(Camera.main.transform,Vector3.up);
    }

    public void UpdateHealth(float currentHealth,float maxHealth)
    {
        healthImage.fillAmount = currentHealth/maxHealth;
    }

    public void UpdateStamina(float currentstamina,float maxstamina)
    {
        staminaImage.fillAmount = currentstamina/maxstamina;
    }

    public async void StartDamagePopUpText(float damage)
    {
        //Initialize
        var obj = damageHealthText.transform.gameObject;
        float delaySeconds = 0.5f;

        obj.SetActive(true);
        damageHealthText.SetText(damage.ToString());

        LeanTween.scale(obj,Vector3.one * 2,delaySeconds);
        await Task.Delay( (int) (delaySeconds * 1000));

        LeanTween.scale(obj,Vector3.zero,delaySeconds);
        await Task.Delay((int) (delaySeconds * 1000));

        obj.SetActive(false);
    }
}
