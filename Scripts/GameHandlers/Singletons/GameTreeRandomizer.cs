using UnityEngine;
using System.Collections.Generic;

public class GameTreeRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class MaterialRandomizer
    { 
        public Material mat;public  int chance = 1;
        public MaterialRandomizer(Material m,int c)
        {
            mat = m;
            chance = c;
        }
    }

    public MaterialRandomizer[] treeBarkMaterials;
    public MaterialRandomizer[] leafsColorMaterials;
    public GameObject[] TreeParent;
    System.Random random;
    public static GameTreeRandomizer Singleton;
    void Start()
    {
        if (Singleton == null)
            Singleton = this;
 
        RandomizeTrees();
    }

    public void RandomizeTrees()
    {
        random = new System.Random();

        SortMats(leafsColorMaterials);
        SortMats(treeBarkMaterials);

        for (int i = 0; i < TreeParent.Length; i++)
        {
            var treeP = TreeParent[i];

            for (int j = 0; j < treeP.transform.childCount; j++)
            {
                var tree = treeP.transform.GetChild(j).gameObject;
                RandomizeTree(tree);
            } 
        }
    }
    public void RandomizeTree(GameObject tree)
    {
        var meshR =tree.GetComponent<MeshRenderer>();

        Material[] newMats = new Material[2];
        newMats[0] = GetMaterial(treeBarkMaterials);
        newMats[1] = GetMaterial(leafsColorMaterials);        

        
        meshR.materials = newMats;       
    }

    public Material GetMaterial(MaterialRandomizer[] mats)
    {
        int totalChange = 0;
        for (int i = 0; i < mats.Length; i++)
        {
            totalChange += mats[i].chance;
        }

        //Generate random Roll
        int roll = random.Next(0,totalChange);

        Debug.Log("Rolled a" + roll+ "and total Change"  +totalChange);
        int currentRoll = 0;
        for (int i = 0; i < mats.Length; i++)
        {
            currentRoll += mats[i].chance;
            //If it rolls a 10 and chance is 5 then its not going to choose that material
            //On the second iterations joins 5 chance with the other materiasl chance
            // 5 + 10 = 15 chance
            //maximum roll goes to 15, so this way is ensured that the roll will always be smaller than the last roll
            if (roll <= currentRoll)
            {
                Debug.Log("Choosing " + mats[i].mat);
                return mats[i].mat;
            }
        }
        

        //Should never execute this part of the code
        return mats[0].mat;
    }

    public void SortMats(MaterialRandomizer[] mats)
    {
        for (int i = 0; i < mats.Length; i++)
        {
            for (int j = i+1; j < mats.Length; j++)
            {
                if (mats[i].chance >mats[j].chance )
                {
                    SwapMats(mats,j,i);
                }
            }
            
        }
    }

    public void SwapMats(MaterialRandomizer[] m,int i1,int i2)
    {
        MaterialRandomizer mNew = m[i1];
        m[i1] = m[i2];
        m[i2] = mNew;
    }

}