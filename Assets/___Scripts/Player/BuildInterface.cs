using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildSystem;
public class BuildInterface : Singleton<BuildInterface>
{
    public GameObject buildHint;//build hint
    public GameObject removeHint;//remove hint
    public Transform buildReqContent;
    //public List<Slot> buildReqSlots=new List<Slot>();
    public GameObject slotPrefab;
    public int currentBuildIndex;
    string currentBuildingName;

    ObjectPlacer objPlacer;
    public enum BuildMode
    {
        NONE,
        BUILD,
        REMOVE,
    }

    public BuildMode buildMode;

    public class Building
    {
        public string buildingName;
        public List<InventoryManager.ItemInfo> buildReqInfos= new List<InventoryManager.ItemInfo>();
    }

    public List<Building> buildingList= new List<Building>();

    void Start()
    {
        objPlacer = Camera.main.GetComponentInParent<ObjectPlacer>();
        InitBuildingList();
    }

    // Update is called once per frame
    void Update()
    {
        BuildReqCheck();
        //UpdateBuildSlots();
        BuildModeCheck();
    }

    void BuildModeCheck()
    {
        switch (buildMode)
        {
            case BuildMode.NONE:
                buildHint.SetActive(false);
                removeHint.SetActive(false);
                buildReqContent.gameObject.SetActive(false);
                break;
            case BuildMode.BUILD:
                buildHint.SetActive(true);
                removeHint.SetActive(false);
                buildReqContent.gameObject.SetActive(true);
                break;
            case BuildMode.REMOVE:
                buildHint.SetActive(false);
                removeHint.SetActive(true);
                buildReqContent.gameObject.SetActive(false);
                break;
        }
    }

    //public void OnSelectedBuildingToBuild()
    //{
    //    buildingList[currentBuildIndex].buildReqInfos.
    //}

    public void UpdateBuildSlots()
    {
        if (objPlacer.objectToPlace)
        {
            int count = buildReqContent.childCount;
            for (int i = 0; i < count; i++)
            {
                DestroyImmediate(buildReqContent.GetChild(0).gameObject);
            }
            //Debug.Log(buildingList[currentBuildIndex].buildReqInfos.Count);
            for (int i = 0; i < buildingList[currentBuildIndex].buildReqInfos.Count; i++)
            {
                Slot slot = Instantiate(slotPrefab, buildReqContent).GetComponent<Slot>();
                slot.itemId = buildingList[currentBuildIndex].buildReqInfos[i].itemId;
                slot.itemAmount = buildingList[currentBuildIndex].buildReqInfos[i].itemAmount;
                slot.isUIOnly = true;
                Destroy(slot.GetComponentInChildren<Button>());
            }
        }
    }

    public bool BuildReqCheck()
    {
        bool canUnlock = true;
        for (int i = 0; i < buildReqContent.childCount; i++)
        {
            Slot slot = buildReqContent.GetChild(i).GetComponent<Slot>();
            slot.u_Icon.color = new Color(1, 1, 1, 1);//set to white

            if (slot.itemAmount > InventoryManager.Instance.CheckItemNumInBag(slot.itemId))
            {
                slot.isUIOnly = true;
                slot.u_Icon.color = new Color(1, 0, 0, 1);//set to red
                canUnlock = false;
            }
        }
        return canUnlock;
    }
    public void RemoveReqItemsFromBag()
    {
        for (int i = 0; i < buildReqContent.childCount; i++)
        {
            Slot slot = buildReqContent.GetChild(i).GetComponent<Slot>();
            InventoryManager.Instance.ReduceItemFromBag
                (slot.itemId, slot.itemAmount);
        }
    }


    void InitBuildingList()
    {
        InventoryManager.ItemInfo req = new InventoryManager.ItemInfo();//set a template
        //-------------Miner------------------------
        Building miner = new Building();
        miner.buildingName = "Miner_B";
        req = new InventoryManager.ItemInfo();
        req.itemId = 2003;
        req.itemAmount = 12;
        miner.buildReqInfos.Add(req);
        req = new InventoryManager.ItemInfo();
        req.itemId = 2004;
        req.itemAmount = 4;
        miner.buildReqInfos.Add(req);
        req = new InventoryManager.ItemInfo();
        req.itemId = 2008;
        req.itemAmount = 3;
        miner.buildReqInfos.Add(req);
        buildingList.Add(miner);

        //-------------Constructor--------------------
        Building constructor = new Building();
        constructor.buildingName = "Constructor_B";
        req = new InventoryManager.ItemInfo();
        req.itemId = 2009;
        req.itemAmount = 2;
        constructor.buildReqInfos.Add(req);
        req = new InventoryManager.ItemInfo();
        req.itemId = 2007;
        req.itemAmount = 3;
        constructor.buildReqInfos.Add(req);
        buildingList.Add(constructor);

        //-------------ConveyorBelt--------------------
        Building conveyorBelt = new Building();
        conveyorBelt.buildingName = "ConveyorBelt_B";
        req = new InventoryManager.ItemInfo();
        req.itemId = 2003;
        req.itemAmount = 2;
        conveyorBelt.buildReqInfos.Add(req);
        buildingList.Add(conveyorBelt);
    }
}
