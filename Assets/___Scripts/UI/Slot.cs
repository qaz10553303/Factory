using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int itemId;
    public int itemAmount;
    public Image u_Icon;
    public Text u_Amount;


    private void Awake()
    {
        //ClearSlot();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSlotSelected()
    {
        if (InventoryManager.Instance.selectedItemInfo.itemId == 0)//if currently selects nothing
        {
            InventoryManager.Instance.selectedItemInfo.itemId = this.itemId;
            InventoryManager.Instance.selectedItemInfo.itemAmount = this.itemAmount;
            itemId = 0;
            itemAmount = 0;
        }
        else
        {
            InventoryManager.Instance.selectedItemInfo.itemAmount = //save the rest amount
                InventoryManager.Instance.AddItemToSlot(InventoryManager.Instance.selectedItemInfo.itemId, InventoryManager.Instance.selectedItemInfo.itemAmount, this);
        }
    }

    public void ClearSlot()
    {
        itemId = 0;
        itemAmount = 0;
    }
}
