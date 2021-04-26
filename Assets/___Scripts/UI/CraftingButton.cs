using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CraftingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed;
    public float pressTimer;
    public Slider craftingBar;

    public Event onLongPress = new Event();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed&&Crafting.Instance.InputsCheck()==true)
        {
            int hammerTimes = Mathf.CeilToInt(Crafting.Instance.recipeSlots.currentRecipe.CraftingTime);

            pressTimer += Time.deltaTime;
            if (pressTimer >= 1)
            {
                pressTimer = 0;
                craftingBar.value += (float)1 / hammerTimes;
                AudManager.Instance.PlaySFX(2);
                if (craftingBar.value >= 1)
                {
                    InventoryManager.Instance.AddItemToBag
                        (Crafting.Instance.recipeSlots.output.itemId, Crafting.Instance.recipeSlots.output.itemAmount);
                    for (int i = 0; i < Crafting.Instance.recipeSlots.inputs.Count; i++)
                    {
                        InventoryManager.Instance.ReduceItemFromBag
                            (Crafting.Instance.recipeSlots.inputs[i].itemId, Crafting.Instance.recipeSlots.inputs[i].itemAmount);
                    }
                    craftingBar.value -= 1;
                }
            }

        }
        else
        {
            craftingBar.value = 0;
            pressTimer = 0;
        }
    }




    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        onLongPress.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
