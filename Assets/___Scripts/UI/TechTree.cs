using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BuildSystem;
public class TechTree : Singleton<TechTree>
{
    [System.Serializable]
    public class TechUIElements
    {
        public Description description;
    }

    public int SelectedTechID;
    public TechUIElements techUI;
    public GameObject slotPrefab;

    public BuildItemContainer[] buildingContainerArr;
    public ObjectSelector objSelector;

    [System.Serializable]
    public struct Description
    {
        public Image img;
        public Text name;
        public Text description;
        public Transform slotsPanel;
        public List<Slot> requiredSlots;
    }

    [System.Serializable]
    public class Tech
    {
        public int preReqTech;
        public bool isUnlocked;
        public Sprite img;
        public string name;
        public string description;
        public int[] reqItemIDs;
        public int[] reqItemAmounts;
    }

    public List<Tech> techs;

    void Start()
    {
        techUI.description.requiredSlots = new List<Slot>();
        objSelector = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        UnlockCheck();

    }

    void CreateNewSlots()
    {
        int count = techUI.description.slotsPanel.childCount;

        for (int i = 0; i < count; i++)
        {
            //Debug.Log("destroy!"+ techUI.description.slotsPanel.GetChild(0).gameObject.name);
            DestroyImmediate(techUI.description.slotsPanel.GetChild(0).gameObject);
            
        }
        techUI.description.requiredSlots.Clear();

        for (int i = 0; i < techs[SelectedTechID].reqItemIDs.Length; i++)
        {
            //Debug.Log(techs[SelectedTechID].reqItemIDs.Length);
            GameObject slotGo = Instantiate(slotPrefab, techUI.description.slotsPanel);
            techUI.description.requiredSlots.Add(slotGo.GetComponent<Slot>());
            techUI.description.requiredSlots[i].itemId = techs[SelectedTechID].reqItemIDs[i];
            techUI.description.requiredSlots[i].itemAmount = techs[SelectedTechID].reqItemAmounts[i];
            Destroy(techUI.description.requiredSlots[i].GetComponentInChildren<Button>());
        }
    }

    public bool UnlockCheck()
    {
        bool canUnlock = true;
        for (int i = 0; i < techUI.description.requiredSlots.Count; i++)
        {
            techUI.description.requiredSlots[i].u_Icon.color = new Color(1, 1, 1, 1);//set to white
   
            if (techUI.description.requiredSlots[i].itemAmount > InventoryManager.Instance.CheckItemNumInBag(techUI.description.requiredSlots[i].itemId))
            {
                techUI.description.requiredSlots[i].isUIOnly = true;
                techUI.description.requiredSlots[i].u_Icon.color = new Color(1, 0, 0, 1);//set to red
                canUnlock = false;
            }
        }
        return canUnlock;
    }

    void RemoveReqItemsFromBag()
    {
        for (int i = 0; i < techUI.description.requiredSlots.Count; i++)
        {
            InventoryManager.Instance.ReduceItemFromBag
                (techUI.description.requiredSlots[i].itemId, techUI.description.requiredSlots[i].itemAmount);
        }
    }

    public void OnComfirmedUnlock()
    {
        if (UnlockCheck())
        {
            if (!techs[SelectedTechID].isUnlocked)
            {
                if ((techs[SelectedTechID].preReqTech == -1 || techs[techs[SelectedTechID].preReqTech].isUnlocked))
                {
                    RemoveReqItemsFromBag();
                    techs[SelectedTechID].isUnlocked = true;
                    switch (SelectedTechID)
                    {
                        case 0:
                            UnlockBagSlots();
                            OnSelectedButton(1);
                            PlayerInterface.Instance.PrintMsg("You have unlocked a bigger bag!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        case 1:
                            UnlockBagSlots();
                            OnSelectedButton(2);
                            PlayerInterface.Instance.PrintMsg("You have unlocked a bigger bag!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        case 2:
                            UnlockBagSlots();
                            OnSelectedButton(3);
                            PlayerInterface.Instance.PrintMsg("You have unlocked a bigger bag!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        case 3:
                            objSelector.buildObjectList = buildingContainerArr[0];
                            objSelector.RetargetList(buildingContainerArr[0]);
                            OnSelectedButton(4);
                            PlayerInterface.Instance.PrintMsg("You can now build miners(Q)!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        case 4:
                            objSelector.buildObjectList = buildingContainerArr[1];
                            objSelector.RetargetList(buildingContainerArr[1]);
                            OnSelectedButton(5);
                            PlayerInterface.Instance.PrintMsg("You can now build constructors(Q)!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        case 5:
                            objSelector.buildObjectList = buildingContainerArr[2];
                            objSelector.RetargetList(buildingContainerArr[2]);
                            PlayerInterface.Instance.PrintMsg("You can now build conveyor belts(Q)!", Color.green);
                            AudManager.Instance.PlaySFX(1);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    PlayerInterface.Instance.PrintMsg("You need to unlock the previous first!", Color.red);
                    AudManager.Instance.PlaySFX(0);
                }
            }
            else
            {
                PlayerInterface.Instance.PrintMsg("You have already unlocked this!", Color.red);
                AudManager.Instance.PlaySFX(0);
            }
        }
            
        else
        {
            PlayerInterface.Instance.PrintMsg("You don't have enough material!", Color.red);
            AudManager.Instance.PlaySFX(0);
        }

    }

    void UnlockBagSlots()
    {
        InventoryManager ivty = InventoryManager.Instance;
        if (ivty.bagExtention.Count >= 6)
        {
            for (int i = 0; i < 6; i++)
            {
                Slot newSlot = ivty.bagExtention[0];
                ivty.bagInfo.Add(newSlot);
                ivty.bagExtention.Remove(newSlot);
                newSlot.gameObject.SetActive(true);
            }

        }

    }

    public void OnSelectedButton(int btnId)
    {
        SelectedTechID = btnId;
        CreateNewSlots();
        techUI.description.img.sprite = techs[SelectedTechID].img;
        techUI.description.name.text = techs[SelectedTechID].name;
        techUI.description.description.text = techs[SelectedTechID].description;
    }

}
