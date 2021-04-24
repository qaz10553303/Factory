using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class Item
//{
//    public int ID { get; set; }
//    public string Name { get; set; }
//    public string Type { get; set; }
//    public int StackSize { get; set; }
//    public string Description { get; set; }
//    public string IconPath { get; set; }

//    public Item(int id, string name, string type, int stackSize, string description, string iconPath)
//    {
//        this.ID = id;
//        this.Name = name;
//        this.Type = type;
//        this.StackSize = stackSize;
//        this.Description = description;
//        this.IconPath = iconPath;
//    }


//    public enum ItemType
//    {
//        MATERIAL,
//        CONSUMABLE,
//        NORMAL,

//    }
//}
[System.Serializable]
public class Item
{
    public int ID;
    public string Name;
    public string ItemType;
    public int StackSize;
    public string Description;
    public string IconPath;

}

