using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    [System.Serializable]
    public class ItemInfo
    {
        public int id;
        public int amount;
    }
    public int RecipeId;
    public float CraftingTime;
    public ItemInfo[] InputsArr;
    public ItemInfo Outputs;



}
