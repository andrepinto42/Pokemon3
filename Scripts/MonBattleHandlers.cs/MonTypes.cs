using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonTypes {
    public enum Type
    {
        LIGHTNING,
        WATER,
        EARTH,
        FIRE,
        ICE,
        COSMIC,
        VOID,
        DEMON
    }
    //Time of search O(1)
    static Dictionary<Type,Dictionary<Type,float>> mapTypeAdvantage = 
    new Dictionary<Type, Dictionary<Type,float>>(){

        {Type.LIGHTNING,
        new Dictionary<Type, float>(){
                {Type.COSMIC,2f},
                {Type.WATER,2f},
                {Type.EARTH,0.5f}
            }
        },
   
        {Type.WATER,
        new Dictionary<Type, float>(){
                {Type.COSMIC,2f},
                {Type.EARTH,2f},
                {Type.LIGHTNING,0.5f}
            }
        },

        {Type.EARTH,
        new Dictionary<Type, float>(){
                {Type.COSMIC,2f},
                {Type.LIGHTNING,2f},
                {Type.WATER,0.5f}
            }
        },

        {Type.ICE,
        new Dictionary<Type, float>(){
                {Type.COSMIC,2f},
                {Type.FIRE,2f},
                {Type.ICE,0.5f}
            }
        },

        {Type.FIRE,
        new Dictionary<Type, float>(){
                {Type.COSMIC,2f},
                {Type.ICE,2f},
                {Type.FIRE,0.5f}
            }
        },

        {Type.COSMIC,
        new Dictionary<Type, float>(){
            {Type.DEMON,2f},
        }},

        //Void is just here as a control type
        {Type.VOID,
        new Dictionary<Type, float>(){

        }},

        //Demon Type is very OP but can be easily taken down by a Cosmic
        {Type.DEMON,
        new Dictionary<Type, float>(){
            {Type.COSMIC,0f}, //is Immune to Demon Attacks
            {Type.ICE,2f},
            {Type.FIRE,2f},
            {Type.LIGHTNING,2f},
            {Type.EARTH,2f},
            {Type.WATER,2f},

        }},
    };

    public static float GetAdvantage(Type t1,Type[] arr_t2){
        float advantage = 1f;
        
        //Moves can be 0.25x to 4x effective
        for (int i = 0; i < arr_t2.Length; i++)
        {
            if (mapTypeAdvantage[t1].ContainsKey(arr_t2[i]))
                advantage *= mapTypeAdvantage[t1][arr_t2[i]];
        }
        
        return advantage;
    }
}