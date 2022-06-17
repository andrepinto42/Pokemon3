using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "MonADN", menuName = "Mon/Mon ADN", order = 1)]
public class MonADN : ScriptableObject{
   
    [Tooltip("Experience value between 50 and 150.")]
    public int experienceBase = 100;
    public string nameMon;
    [Header("Characteristics")]
    //https://bulbapedia.bulbagarden.net/wiki/List_of_Pok%C3%A9mon_by_base_stats_(Generation_VIII-present)
    public int baseSpeed = 45/2;
    public int baseStance = 20;
    public int baseMaxHealth= 45/2;
    public int baseMaxStamina = 10;
    public MonTypesCombat baseAttackType = new MonTypesCombat(25,49/2,65/2,65/2);
    public MonTypesCombat baseDefenseType = new MonTypesCombat(49/2,49/2,80/2,80/2);
    public MonMeshManager monGameObjectTransform;
    public Genetic genetic;
    
    public MonTypes.Type typeMain ;
    public Skill[] allSkills = new Skill[4];

    [Header("Extra Information")]
    public Sprite imageMon;

    public Sprite GetImage()
    {
        return imageMon;
    }
    
}
