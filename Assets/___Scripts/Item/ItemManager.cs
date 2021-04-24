using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public class ItemData
    {
        public List<Item> ItemList = new List<Item>();
    }

    public ItemData data = new ItemData();

    // Start is called before the first frame update

    void Start()
    {
        LoadItemListFromJson();
    }

    void LoadItemListFromJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Data/ItemData");//read file
        string jsonData = itemText.text;//convert to json string
        Debug.Log("Got this: " + jsonData);
        data = JsonUtility.FromJson<ItemData>(jsonData);//convert to json array


        Debug.Log("Imported Item Number: " + data.ItemList.Count);
    }

    public Item GetItemById(int ItemID)
    {
        foreach (Item item in data.ItemList)
        {
            if (item.ID == ItemID) return item;
        }
        return null;
    }
}
