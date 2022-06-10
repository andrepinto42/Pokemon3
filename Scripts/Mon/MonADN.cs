using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "MonADN", menuName = "Mon/Mon ADN", order = 1)]
public class MonADN : ScriptableObject{
    public float baseMaxHealth;
    public float baseMaxStamina;
    [Tooltip("Experience value between 50 and 150.")]
    public int experienceBase = 100;
    public string nameMon;
    [Header("Characteristics")]
    public MonTypesCombat baseAttackType = new MonTypesCombat(1,1,1,1);
    public MonTypesCombat baseDefenseType = new MonTypesCombat(1,1,1,1);
    public float baseAttack=30f;
    public float baseDefense=20f;
    public float baseSpeed = 5f;
    public float baseStance = 5f;
    public MonMeshManager monGameObjectTransform;
    public Genetic genetic;
    
    public MonTypes.Type typeMain ;
    public float attackGrow = 2f;
    public float defenseGrow = 1f;
    public float speedGrow = 1f;
    public float healthGrow = 1f;
    public Skill[] allSkills = new Skill[4];

    [Header("Extra Information")]
    public Sprite imageMon;

    public Sprite GetImage()
    {
        return imageMon;
    }
    
}
