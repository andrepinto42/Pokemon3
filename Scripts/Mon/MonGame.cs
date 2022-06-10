using UnityEngine;
[CreateAssetMenu(fileName = "MonGame", menuName = "Mon/Mon Game", order = 1)]
public class MonGame : ScriptableObject{
    [Header("Atributes for Battle Only")]
    public float currentHealth;
    public float currentStamina;
    public float currentStance;
    public float currentSpeed;


    [Header("Atributes")]
    public MonADN monADN;
    public int experiencePoints = 0;
    public int level = 1;
    public string nickname;
    //Primeira evolucao contem sempre o tipo secundario void, apenas na segunda evolucao o pokemon recebe novos tipos
    public MonTypes.Type typeSecondary = MonTypes.Type.VOID;
    [Header("Stats")]
    public float maxHealth;
    public float maxStamina;
    public MonTypesCombat baseAttackType = new MonTypesCombat(30,1,1,1);
    public MonTypesCombat baseDefenseType = new MonTypesCombat(20,1,1,1);

    public float maxStance=5f;
    public float maxSpeed = 10f;
    public float AttackCurrent=30f;
    public float DefenseCurrent=20f;
    


    public void RestoreStatsToNormal()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentStance = maxStance;
        currentSpeed = maxSpeed;
    }

    public void RestartStatsEnteringBattle()
    {
        //Health is not set to max because the pokemon can lose health from battle to battle


        currentStamina = maxStamina;
        currentStance = maxStance;
        currentSpeed = maxSpeed;
    }
    /*
    -----------------------
        GETTERS
    -----------------------
    */

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

    public MonTypes.Type[] GetTypeMon()
    {

        return new MonTypes.Type[]{typeSecondary,monADN.typeMain};
    }
    public Genetic GetMonGenetic()
    {
        return monADN.genetic;
    }
}
