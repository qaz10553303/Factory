using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeButton : MonoBehaviour
{
    public Text nameText;
    public Image iconImg;
    public int recipeID;
    public bool isCraftBtn;

    void Start()
    {
        if (GetComponentInParent<Crafting>())
        {
            isCraftBtn = true;
            GetComponent<Button>().onClick.AddListener(delegate () {
                Crafting.Instance.OnSelectedRecipe(recipeID);
            });
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(delegate () {
                GetComponentInParent<Constructor>().OnSelectedRecipe(recipeID);
            });
        }



        int outputItemId = ItemManager.Instance.GetRecipeById(recipeID).Outputs.id;
        Sprite sp = Resources.Load<Sprite>("Icons/Material/" + ItemManager.Instance.GetItemById(outputItemId).Name);
        iconImg.sprite = sp;
        string nameStr = ItemManager.Instance.GetItemById(outputItemId).Name.Replace("_", " ");
        nameText.text = nameStr;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
