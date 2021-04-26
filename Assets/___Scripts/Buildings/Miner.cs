using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Miner : MonoBehaviour
{
    public int miningProductIndex;//1001,1002, etc
    public float miningRatePerMinute;//unit is per minute
    public float burnRatePerMinute;
    public Slot fuelSlot;
    public Slot productSlot;
    public Slider miningBar;
    public Slider fuelBar;
    public Text miningRateText;
    public Text burnRateText;

    public GameObject minerUI;
    public GameObject resourceBoxPrefab;

    private float spawnTimer;

    public bool isWorking;
    public bool allowOutput;

    //private float spawnTime;//for output items

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIText();
        FuelCheck();
        if (fuelBar.value > 0)
        {
            Mining();
        }
        CreateOutput();
    }

    void Mining()
    {
        if (productSlot.itemAmount < 100 && miningRatePerMinute>0)//if product not full
        {
            miningBar.value += miningRatePerMinute / 60 * Time.deltaTime;
            isWorking = true;
            if (miningBar.value >= 1)
            {
                miningBar.value -= 1;
                InventoryManager.Instance.AddItemToSlot(miningProductIndex, 1, productSlot);
            }
        }
        else
        {
            isWorking = false;
        }
    }

    void FuelCheck()
    {
        if (fuelBar.value <= 0 && fuelSlot.itemAmount > 0)
        {
            BurnRateCheck();
            fuelSlot.itemAmount -= 1;
            fuelBar.value += 1;
        }

        if (fuelBar.value > 0&&isWorking)
        {

            fuelBar.value -= burnRatePerMinute / 60 * Time.deltaTime;
        }
    }

    void BurnRateCheck()
    {
        switch (fuelSlot.itemId)
        {
            case 0://nothing
                burnRatePerMinute = 0;
                break;
            case 1003://coal
                burnRatePerMinute = 3;
                break;
            default:
                break;
        }
    }

    void CreateOutput()
    {
        if (!allowOutput) return;
        spawnTimer += Time.deltaTime;
        if(spawnTimer>=0.5f&& productSlot.itemAmount > 0)
        {
            spawnTimer = 0;
            Vector3 spawnPos = GetComponentInChildren<ConveyorBelt>().transform.position;
            spawnPos.y += 0.2f;
            GameObject go = Instantiate(resourceBoxPrefab, spawnPos, Quaternion.identity);
            Box box = go.GetComponent<Box>();
            box.carriedItem.itemId = productSlot.itemId;
            box.carriedItem.itemAmount = 1;
            productSlot.itemAmount -= 1;
        }
    }

    void UpdateUIText()
    {
        miningRateText.text = "(MiningSpeed:" + miningRatePerMinute + "/min)";
        burnRateText.text = "(BurnRate:" + burnRatePerMinute + "/min)";
    }


    void Init()
    {
        fuelSlot.itemsCanStore.Add(1003);//coal
        allowOutput = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (miningProductIndex == 0&&other.gameObject.layer== LayerMask.NameToLayer("Resource"))
        {
            miningProductIndex = other.GetComponent<ResourceNode>().miningProductIndex;
            miningRatePerMinute = other.GetComponent<ResourceNode>().defaultMiningRatePerMinute;
            if (miningProductIndex != 0)
            {
                productSlot.itemsCanStore.Add(miningProductIndex);
            }
            else
            {
                Debug.LogError("Mining product index Cannot be 0!");
            }
        }

    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.transform.tag == "Box")
    //    {
    //        Debug.Log("spwan");
    //        allowOutput = false;
    //        spawnTime = 0;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.transform.tag == "Box")
    //    {
    //        allowOutput = true;
    //    }
    //}
}
