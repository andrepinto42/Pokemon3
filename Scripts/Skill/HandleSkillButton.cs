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

    public static void InitializeButtonsSkills(GameObject allSkills,MonGame monMain)
    {
        Dictionary<MonTypes.Type,Color32> mapColor = new Dictionary<MonTypes.Type, Color32>(){
        
        {MonTypes.Type.EARTH,new Color32(205, 127, 50,255)},
        {MonTypes.Type.LIGHTNING,new Color32(255,252,68,255)},
        {MonTypes.Type.FIRE,new Color32(255,0,0,255)},
        {MonTypes.Type.ICE,new Color32(161, 249, 255,255)},
        {MonTypes.Type.WATER,new Color32(96,96,255,255)},
        {MonTypes.Type.COSMIC,new Color32(138,43,226,255)},//Purple
        {MonTypes.Type.VOID,new Color32(255,255,255,255)},//White
        {MonTypes.Type.DEMON,new Color32(191,72,21,255)}//Orange

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
            //Skill doesnt Exist then set the button to invisible
            if (skills[i] == null)
            {
                allSkillsTransform[i].gameObject.SetActive(false);
                continue;
            }
            
            //Set name Skill Text
            var text =allSkillsTransform[i].GetChild(positionChildHabilityText).GetComponent<TMP_Text>();
            text.SetText(skills[i].nameSkill);
            
            var image = allSkillsTransform[i].GetChild(positionChildHabilityImage).GetComponent<Image>();
            
            image.color = mapColor[skills[i].type];
        }

        //Set stamina Text
        for (int i = 0; i < numChilds; i++)
        {
            //Skill doesnt Exist
            if (skills[i] == null)
                continue;
            
            var text = allSkillsTransform[i].GetChild(positionChildStaminaText).GetComponent<TMP_Text>();
            int value = (int) skills[i].stamina;

            //Red if consumes Stamina, blue if it charges stamina
            text.color = value >0 ? Color.red :  Color.blue;

            text.SetText(Math.Abs(value).ToString());
        }
        
    }
}