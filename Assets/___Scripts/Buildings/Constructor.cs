using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Constructor : MonoBehaviour
{
    public GameObject singleInputTemplatePrefab;
    public GameObject constructorInterface;//for all
    public GameObject menuInterface;//for menu
    public GameObject workInterface;//for work
    public Transform bpContent;
    public GameObject recipeButtonPrefab;

    public Transform inputsPanel;
    public Text outputSpeedText;
    public Recipe currentRecipe;

    [System.Serializable]
    public class RecipeSlots
    {
        public List<Slot> inputs;
        public List<Slider> inputSliders;
        public List<GameObject> connectedLines;
        public Slot output;
        public Slider outputSlider;
    }
    public RecipeSlots recipeSlots;
    public RecipeSlots workSlots;
    public GameObject productBoxPrefab;

    public bool allowInput;
    public bool allowOutput;
    public float spawnTime;

    void Start()
    {
        Init_();
        OnSelectedRecipe(9001);//set defalut recipe show
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRecipe == null) return;
        MakeProducts();
        if (allowOutput && workSlots.output.itemAmount > 0)
        {
            CreateOutput();
        }

    }

    void Init_()
    {
        foreach (Recipe recipe in ItemManager.Instance.recipeData.RecipeList)
        {
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, obj.GetComponent<RectTransform>().position.y + 100);
            GameObject obj = Instantiate(recipeButtonPrefab, bpContent);
            RecipeButton recipeBtn = obj.GetComponent<RecipeButton>();
            recipeBtn.recipeID = recipe.RecipeId;
        }
    }

    void CreateOutput()
    {
        if (!allowOutput) return;
        if (workSlots.output.itemAmount > 0)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= 1f)
            {
                spawnTime = 0;
                Vector3 spawnPos = GetComponentInChildren<ConveyorBelt>().transform.position;
                spawnPos.y += 0.1f;
                GameObject go = Instantiate(productBoxPrefab, spawnPos, Quaternion.identity);
                Box box = go.GetComponent<Box>();
                box.carriedItem.itemId = workSlots.output.itemId;
                box.carriedItem.itemAmount = 1;
                workSlots.output.itemAmount -= 1;
            }
        }
    }

    public void OnSelectedRecipe(int RecipeID)//btn event
    {
        Recipe recipe = ItemManager.Instance.GetRecipeById(RecipeID);
        currentRecipe = recipe;
        recipeSlots.output.itemId = recipe.Outputs.id;
        recipeSlots.output.itemAmount = recipe.Outputs.amount;
        recipeSlots.output.gameObject.SetActive(true);
        for (int i = 0; i < recipeSlots.inputs.Count; i++)
        {
            if (i < recipe.InputsArr.Length)
            {
                recipeSlots.inputs[i].itemId = recipe.InputsArr[i].id;
                recipeSlots.inputs[i].itemAmount = recipe.InputsArr[i].amount;
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

    public void OnConfirmedRecipe()//btn event
    {
        menuInterface.SetActive(false);
        workInterface.SetActive(true);

        workSlots.inputs = new List<Slot>();
        workSlots.inputSliders.Clear();
        for (int i = 0; i < currentRecipe.InputsArr.Length; i++)//create new work space
        {
            GameObject obj = Instantiate(singleInputTemplatePrefab, inputsPanel);
            SingleInputTemplate temp = obj.GetComponent<SingleInputTemplate>();
            temp.inputSlot.itemId = currentRecipe.InputsArr[i].id;
            temp.inputSlot.itemsCanStore.Add(currentRecipe.InputsArr[i].id);
            temp.consumeSpeedText.text = "ConsumeSpeed:" + 60/currentRecipe.CraftingTime * currentRecipe.InputsArr[i].amount+"/min";
            workSlots.inputs.Add(temp.inputSlot);
            workSlots.inputSliders.Add(temp.consumeBar);
        }
        outputSpeedText.text = "("+ 60 / currentRecipe.CraftingTime * currentRecipe.Outputs.amount + "/min)";
        workSlots.output.ClearSlot();
        workSlots.output.itemId = currentRecipe.Outputs.id;
        workSlots.output.itemsCanStore.Add(currentRecipe.Outputs.id);
        workSlots.outputSlider.value = 0;
        allowInput = true;
    }

    public void OnBackToRecipeMenu()
    {
        menuInterface.SetActive(true);
        workInterface.SetActive(false);
        for (int i = 0; i < workSlots.inputs.Count; i++)
        {
            if (workSlots.inputs[i].itemAmount > 0)
            {
                InventoryManager.Instance.AddItemToBag(workSlots.inputs[i].itemId, workSlots.inputs[i].itemAmount);
                //workSlots.inputs[i].ClearSlot();
            }
        }
        if(workSlots.output.itemAmount > 0)
        {
            InventoryManager.Instance.AddItemToBag(workSlots.output.itemId, workSlots.output.itemAmount);
        }
        List<GameObject> destroyList = new List<GameObject>();
        for (int i = 0; i < inputsPanel.childCount; i++)
        {
            destroyList.Add(inputsPanel.GetChild(i).gameObject);
        }
        foreach (var item in destroyList)//destory old input slots
        {
            Destroy(item);
        }
        allowInput = false;
    }

    bool InputSlotsCheck()
    {
        for (int i = 0; i < workSlots.inputs.Count; i++)
        {
            workSlots.inputs[i].itemId = currentRecipe.InputsArr[i].id;
            if(workSlots.inputSliders[i].value <= 0)
            {
                if (workSlots.inputs[i].itemAmount > 0)
                {
                    workSlots.inputs[i].itemAmount -= 1;
                    workSlots.inputSliders[i].value += 1;
                }
                else return false;//if slider=0 and amount=0
            }
        }
        return true;
    }

    void MakeProducts()
    {
        workSlots.output.itemId = currentRecipe.Outputs.id;
        if (workSlots.inputs.Count == 0) return;
        if (InputSlotsCheck())
        {
            if(workSlots.output.itemAmount==0||workSlots.output.itemAmount <= ItemManager.Instance.GetItemById(workSlots.output.itemId).StackSize)
            {
                for (int i = 0; i < workSlots.inputs.Count; i++)
                {
                    workSlots.inputSliders[i].value -= currentRecipe.InputsArr[i].amount / currentRecipe.CraftingTime * Time.deltaTime;
                }
                workSlots.outputSlider.value += currentRecipe.Outputs.amount / currentRecipe.CraftingTime * Time.deltaTime;
                if (workSlots.outputSlider.value >= 1)
                {
                    workSlots.outputSlider.value -= 1;
                    workSlots.output.itemAmount += 1;
                }
            }

        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            Box box = other.GetComponent<Box>();
            if (currentRecipe == null) return;
            for (int i = 0; i < workSlots.inputs.Count; i++)
            {
                if (workSlots.inputs[i].itemsCanStore.Contains(box.carriedItem.itemId))
                {
                    if(workSlots.inputs[i].itemAmount<100)
                    {
                        workSlots.inputs[i].itemAmount += 1;
                        box.carriedItem.itemAmount -= 1;
                    }
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Box")
        {
            allowOutput = false;
            spawnTime = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Box")
        {
            allowOutput = true;
        }
    }
}
