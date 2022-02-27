using UnityEngine;
[CreateAssetMenu(fileName = "MonADN", menuName = "Mon/Mon ADN", order = 1)]
public class MonADN : ScriptableObject{
    public float baseMaxHealth;
    public float baseMaxStamina;
    public int experienceBase = 100;
    public string nameMon;
    public float baseAttack=30f;
    public float baseDefense=20f;
    public float baseSpeed = 5f;
    public float baseStance = 5f;
    public MonMeshManager monGameObjectTransform;
    public Genetic genetic;
    
    public MonTypes.Type typeMain ;
    public Skill[] allSkills = new Skill[4];
    
}
