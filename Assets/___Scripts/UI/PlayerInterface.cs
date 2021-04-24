using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInterface : Singleton<PlayerInterface>
{
    GameObject ghostItemUI;
    public GameObject ghostItemPrefab;

    void Start()
    {
        //StartCoroutine("AutoUpdateUI");
    }

    void Update()
    {
        InventoryManager.Instance.UpdateBagUI(InventoryManager.Instance.bagInfo);
        UpdateIconOnSelected();
    }

    public void SetInterfaceActive(GameObject obj , bool isActive)
    {
        obj.SetActive(isActive);
    }

    IEnumerator AutoUpdateUI()
    {
        while (true)
        {
            InventoryManager.Instance.UpdateBagUI(InventoryManager.Instance.bagInfo);
            UpdateIconOnSelected();
            yield return new WaitForSeconds(1f);
        }
    }

    void UpdateIconOnSelected()
    {
        if (InventoryManager.Instance.selectedItemInfo.itemId != 0&& ghostItemUI==null)
        {
            Debug.Log("On Ghost Create!");
            ghostItemUI = Instantiate(ghostItemPrefab,Input.mousePosition,Quaternion.identity, this.transform);
            string iconPath = ItemManager.Instance.GetItemById(InventoryManager.Instance.selectedItemInfo.itemId).IconPath;
            ghostItemUI.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(iconPath);
            ghostItemUI.GetComponentInChildren<Text>().text = InventoryManager.Instance.selectedItemInfo.itemAmount.ToString();
        }
        else if (InventoryManager.Instance.selectedItemInfo.itemId != 0 && ghostItemUI)
        {
            Debug.Log("On Ghost Move!");
            ghostItemUI.transform.position = Input.mousePosition;
            ghostItemUI.GetComponentInChildren<Text>().text = InventoryManager.Instance.selectedItemInfo.itemAmount.ToString();
        }
        else if (ghostItemUI)
        {
            Debug.Log("On Ghost Destroy!");
            Destroy(ghostItemUI);
        }
    }
}

