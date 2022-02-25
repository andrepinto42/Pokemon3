using UnityEngine;

[CreateAssetMenu(fileName = "MonGame", menuName = "Mon/Mon Game", order = 1)]
public class MonGame : ScriptableObject{
    public float currentHealth;
    [HideInInspector]public float currentStamina;
    [HideInInspector]public float attackBuff = 1f;
    [HideInInspector]public float defenseBuff = 1f;
    [HideInInspector]public float speedBuff =1f;

    [Header("Atributes")]
    public MonADN monADN;
    public int experiencePoints = 0;
    public int level = 1;
    public string nickname;
    public MonTypes.Type type;
    [Header("Stats")]
    public float maxHealth;
    public float maxStamina;
    public float SpeedCurrent;
    
    public float AttackCurrent=30f;
    public float DefenseCurrent=20f;

    [SerializeField]private Skill[] allSkills = new Skill[4];
     public string GetNameMon(){
        return monADN.nameMon;
    }
     public Skill[] GetSkills(){
        return allSkills;
    }
    public MonMeshManager GetMonMeshManager(){
        return monADN.monGameObjectTransform;
    }
    public int GetBaseExperience(){
        return monADN.experienceBase;
    }
}
