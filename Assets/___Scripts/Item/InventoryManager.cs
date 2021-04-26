using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<Slot> bagInfo;
    public List<Slot> bagExtention;

    public List<List<Slot>> containerList = new List<List<Slot>>();

    public ItemInfo selectedItemInfo= new ItemInfo();

    [System.Serializable]
    public class ItemInfo
    {
        public int itemId;
        public int itemAmount;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitBag();
        containerList.Add(bagInfo);

        AddItemToBag(1003, 100);
        AddItemToBag(2003, 100);
        AddItemToBag(2004, 100);
        AddItemToBag(2006, 100);
        AddItemToBag(2007, 100);
        AddItemToBag(2008, 200);
        AddItemToBag(2009, 100);
        AddItemToBag(2010, 100);
        //AddItemToBag(2003, 400);
    }

    // Update is called once per frame
    void Update()
    {
        //AddItemToBag(1001, 1);
        //Debug.Log(selectedSlot.itemId);
    }



    void InitBag()
    {
        //UpdateBagUI(bagInfo);
    }


    public int AddItemToBag(int itemId, int amount)
    {
        int maxSize = ItemManager.Instance.GetItemById(itemId).StackSize;
        int restSize = amount;
        foreach (Slot slot in bagInfo)//try to find if there's a slot that has the same item
        {
            if (slot.itemId == itemId&&slot.itemAmount< maxSize)//if have same item in a slot and it is not full
            {
                slot.itemAmount += amount;//fill slot amount
                if (slot.itemAmount > maxSize)//if over stack
                {
                    restSize = slot.itemAmount - maxSize;//get rest size
                    slot.itemAmount = maxSize;//set to max stack
                    AddItemToBag(itemId, restSize);//trigger again
                }
                else
                {
                    //UpdateBagUI(bagInfo);
                    return 0;
                }
            }
        }
        foreach (Slot slot in bagInfo)
        {
            if (slot.itemId == 0)
            {
                slot.itemId = itemId;
                slot.itemAmount = amount;
                if (slot.itemAmount > maxSize)
                {
                    restSize = slot.itemAmount - maxSize;
                    slot.itemAmount = maxSize;
                    AddItemToBag(itemId, restSize);
                }
                //UpdateBagUI(bagInfo);
                return 0;
            }
        }
        //UpdateBagUI(bagInfo);
        return restSize;
    }

    public int CheckItemNumInBag(int itemId)
    {
        int totalAmount = 0;
        foreach (Slot slot in bagInfo)
        {
            if (slot.itemId == itemId)
            {
                totalAmount += slot.itemAmount;
            }
        }
        return totalAmount;
    }

    public bool ReduceItemFromBag(int itemId, int reduceAmount)
    {
        if (reduceAmount > CheckItemNumInBag(itemId)) return false;//check item amount before delete
        foreach (Slot slot in bagInfo)
        {
            if (slot.itemId == itemId)
            {
                reduceAmount -= slot.itemAmount;
                if (reduceAmount >= 0)
                {
                    slot.ClearSlot();
                }
                else if (reduceAmount<0)
                {
                    slot.itemAmount = -reduceAmount;
                    return true;
                }
            }
        }
        return false;//unexpected error,should not run this line at anytime
    }


    //public void UpdateBagUI(List<Slot> container)//set slot sprite and text by itemid and amount
    //{
    //    for (int i = 0; i < container.Count; i++)
    //    {
    //        if (container[i].itemId == 0)
    //        {
    //            container[i].u_Icon.sprite = null;
    //            container[i].u_Icon.enabled = false;
    //            container[i].u_Amount.text = "";
    //        }
    //        else if (container[i].itemId != 0 && container[i].itemAmount > 0)
    //        {
    //            Sprite sp = Resources.Load<Sprite>(ItemManager.Instance.GetItemById(container[i].itemId).IconPath);
    //            container[i].u_Icon.sprite = sp;
    //            container[i].u_Icon.enabled = true;
    //            container[i].u_Amount.text = container[i].itemAmount.ToString();
    //        }
    //    }
    //}

    public int AddItemToSlot(int itemId, int amount, Slot target)
    {
        int maxSize = ItemManager.Instance.GetItemById(itemId).StackSize;
        int restSize = amount;
        if (target.itemId == 0)//if targetslot is empty
        {
            target.itemId = itemId;
            target.itemAmount = amount;
            return 0;
        }
        else if (target.itemId==itemId)//if target slot stores the same item
        {
            target.itemAmount += amount;
            if (target.itemAmount <= maxSize)
            {
                return 0;
            }
            else//if over stack
            {
                restSize = target.itemAmount - maxSize;
                target.itemAmount = maxSize;
                return restSize;
            }
        }
        return amount;
    }

    public void GetSlotDataCopy(Slot To, Slot From)
    {
        To.itemId = From.itemId;
        To.itemAmount = From.itemAmount;
        To.itemsCanStore = From.itemsCanStore;
        To.itemsCanTake = From.itemsCanTake;
    }



}
