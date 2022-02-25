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
        ICE
    }
    //Time of search O(1)
    static Dictionary<Type,Dictionary<Type,float>> mapTypeAdvantage = 
    new Dictionary<Type, Dictionary<Type,float>>(){

        {Type.LIGHTNING,
        new Dictionary<Type, float>(){
                {Type.WATER,2f},
                {Type.EARTH,0.5f}
            }
        },
   
        {Type.WATER,
        new Dictionary<Type, float>(){
                {Type.EARTH,2f},
                {Type.LIGHTNING,0.5f}
            }
        },

        {Type.EARTH,
        new Dictionary<Type, float>(){
                {Type.LIGHTNING,2f},
                {Type.WATER,0.5f}
            }
        },

        {Type.ICE,
        new Dictionary<Type, float>(){
                {Type.FIRE,2f},
                {Type.ICE,0.5f}
            }
        },

        {Type.FIRE,
        new Dictionary<Type, float>(){
                {Type.ICE,2f},
                {Type.FIRE,0.5f}
            }
        }   
    };

    public static float GetAdvantage(Type t1,Type t2){
        if (!mapTypeAdvantage[t1].ContainsKey(t2))
            return 1f;
        
        return mapTypeAdvantage[t1][t2];
    }
}