using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public class ItemData
    {
        public List<Item> ItemList = new List<Item>();
    }

    public ItemData itemData = new ItemData();

    public class RecipeData
    {
        public List<Recipe> RecipeList = new List<Recipe>();
    }

    public RecipeData recipeData = new RecipeData();
    // Start is called before the first frame update

    void Start()
    {
        LoadItemListFromJson();
        LoadRecipeListFromJson();
    }

    void LoadItemListFromJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Data/ItemData");//read file
        string jsonData = itemText.text;//convert to json string
        Debug.Log("Got this: " + jsonData);
        itemData = JsonUtility.FromJson<ItemData>(jsonData);//convert to json array


        Debug.Log("Imported Item Number: " + itemData.ItemList.Count);
    }

    void LoadRecipeListFromJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Data/RecipeData");//read file
        string jsonData = itemText.text;//convert to json string
        Debug.Log("Got this: " + jsonData);
        recipeData = JsonUtility.FromJson<RecipeData>(jsonData);//convert to json array


        Debug.Log("Imported Recipe Number: " + recipeData.RecipeList.Count);
    }

    public Item GetItemById(int ItemID)
    {
        foreach (Item item in itemData.ItemList)
        {
            if (item.ID == ItemID) return item;
        }
        return null;
    }

    public Recipe GetRecipeById(int RecipeID)
    {
        foreach (Recipe recipe in recipeData.RecipeList)
        {
            if (recipe.RecipeId == RecipeID) return recipe;
        }
        return null;
    }
}
