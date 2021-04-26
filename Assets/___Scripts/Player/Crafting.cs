using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crafting : Singleton<Crafting>
{
    public GameObject craftingInterface;
    public Transform content;
    public GameObject recipeButtonPrefab;

    public Text estimateCraftingTimeText;

    [System.Serializable]
    public class RecipeSlots
    {
        public Recipe currentRecipe;
        public List<Slot> inputs;
        public List<GameObject> connectedLines;
        public Slot output;
    }
    public RecipeSlots recipeSlots;
    

    void Start()
    {
        Init_();
        OnSelectedRecipe(9001);//set defalut recipe show
    }

    // Update is called once per frame
    void Update()
    {
        InputsCheck();
    }

    void Init_()
    {
        foreach (Recipe recipe in ItemManager.Instance.recipeData.RecipeList)
        {
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, obj.GetComponent<RectTransform>().position.y + 100);
            GameObject obj = Instantiate(recipeButtonPrefab, content);
            RecipeButton recipeBtn = obj.GetComponent<RecipeButton>();
            recipeBtn.recipeID = recipe.RecipeId;
        }
    }

    public bool InputsCheck()
    {
        bool canCraft = true;
        Recipe.ItemInfo[] inputs = recipeSlots.currentRecipe.InputsArr;

        for (int i = 0; i < inputs.Length; i++)
        {
            recipeSlots.inputs[i].u_Icon.color = new Color(1, 1, 1, 1);//set to white
            if (inputs[i].amount > InventoryManager.Instance.CheckItemNumInBag(inputs[i].id))
            {
                recipeSlots.inputs[i].u_Icon.color = new Color(1, 0, 0, 1);//set to red
                canCraft = false;
            }
        }
        return canCraft;
    }

    public void OnSelectedRecipe(int RecipeID)
    {
        Recipe recipe = ItemManager.Instance.GetRecipeById(RecipeID);
        recipeSlots.currentRecipe = recipe;
        recipeSlots.output.itemId = recipe.Outputs.id;
        recipeSlots.output.itemAmount = recipe.Outputs.amount;
        recipeSlots.output.gameObject.SetActive(true);
        for (int i = 0; i < recipeSlots.inputs.Count; i++)
        {
            if (i < recipe.InputsArr.Length)
            {
                recipeSlots.inputs[i].itemId = recipe.InputsArr[i].id;
                recipeSlots.inputs[i].itemAmount = recipe.InputsArr[i].amount;
                if(estimateCraftingTimeText) estimateCraftingTimeText.text = "CraftingTime:" + recipe.CraftingTime + "s";
                recipeSlots.inputs[i].gameObject.SetActive(true);
                recipeSlots.connectedLines[i].SetActive(true);
            }
            else
            {
                recipeSlots.inputs[i].gameObject.SetActive(false);
                recipeSlots.connectedLines[i].SetActive(false);
            }

        }
    }
}
