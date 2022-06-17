using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleSkillButton
{
    private const  int positionChildHabilityText = 5;
    private const int positionChildStaminaText = 4;

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
            //Force the ativation of the gameObject
            allSkillsTransform[i].gameObject.SetActive(true);
            
            //Set name Skill Text
            var text =allSkillsTransform[i].GetChild(positionChildHabilityText).GetComponent<TMP_Text>();
            text.SetText(skills[i].nameSkill);
            
            var imageBackground = allSkillsTransform[i].GetChild(1).GetComponent<Image>();
            var imageBolinha = allSkillsTransform[i].GetChild(3).GetComponent<Image>();
            var imageBackgroundPrincipal = allSkillsTransform[i].GetChild(0).GetComponent<Image>();

            
            imageBackground.color = mapColor[skills[i].type];
            imageBolinha.color = mapColor[skills[i].type];

            imageBackgroundPrincipal.color = mapColor[skills[i].type].Multiply(new Color32(150,150,150,255));
        }

        //Set stamina Text
        for (int i = 0; i < numChilds; i++)
        {
            //Skill doesnt Exist
            if (skills[i] == null)
                continue;
            
            var imageBackground = allSkillsTransform[i].GetChild(positionChildStaminaText).GetComponent<Image>();
            var text = allSkillsTransform[i].GetChild(positionChildStaminaText).GetComponentInChildren<TMP_Text>();

            int value = (int) skills[i].stamina;

            //Red if consumes Stamina, blue if it charges stamina, grey if it neither consumes nor gains stamina
            if (value == 0)
                imageBackground.color = Color.grey;
            else
                imageBackground.color = value >0 ? Color.red :  Color.blue;


            text.SetText(Math.Abs(value).ToString());
        }
        
    }
}