using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInterface : Singleton<BagInterface>
{
    // Start is called before the first frame update
    public GameObject bagUI;
    public GameObject smallBagUI;

    public List<Slot> smallBagSlots;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < smallBagSlots.Count; i++)
        //{
        //    //smallBagSlots[i] = InventoryManager.Instance.bagInfo[i];
        //    smallBagSlots[i].itemId = InventoryManager.Instance.bagInfo[i].itemId;
        //    smallBagSlots[i].itemAmount = InventoryManager.Instance.bagInfo[i].itemAmount;
        //}
    }
}
