using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleSkillButton
{
    private const int positionChildHabilityImage = 0;
    private const  int positionChildHabilityText = 2;
    private const int positionChildStaminaText = 3;

    public static void Initialize(GameObject allSkills,MonGame monMain)
    {
        Dictionary<MonTypes.Type,Color32> mapColor = new Dictionary<MonTypes.Type, Color32>(){
        
        {MonTypes.Type.EARTH,new Color32(205, 127, 50,255)},
        {MonTypes.Type.LIGHTNING,new Color32(255,252,68,255)},
        {MonTypes.Type.FIRE,new Color32(255,0,0,255)},
        {MonTypes.Type.ICE,new Color32(161, 249, 255,255)},
        {MonTypes.Type.WATER,new Color32(96,96,255,255)}

        };

        Skill[] skills = monMain.GetSkills();
        Transform parentTransform = allSkills.transform;
        
        //Get 4 buttons associated to a skill
        int numChilds = 4;
        Transform[] allSkillsTransform = new Transform[numChilds];
        for (int i = 0; i < numChilds; i++)
        {
            allSkillsTransform[i] = parentTransform.GetChild(i);
        }
        
        for (int i = 0; i < numChilds; i++)
        {
            //Set name Skill Text
            var text =allSkillsTransform[i].GetChild(positionChildHabilityText).GetComponent<TMP_Text>();
            text.SetText(skills[i].nameSkill);
            
            var image = allSkillsTransform[i].GetChild(positionChildHabilityImage).GetComponent<Image>();
            
            image.color = mapColor[skills[i].type];
        }

        //Set stamina Text
        for (int i = 0; i < numChilds; i++)
        {
            var text = allSkillsTransform[i].GetChild(positionChildStaminaText).GetComponent<TMP_Text>();
            int value = (int) skills[i].stamina;

            //Red if consumes Stamina, blue if it charges stamina
            text.color = value >0 ? Color.red :  Color.blue;

            text.SetText(Math.Abs(value).ToString());
        }
        
    }
}