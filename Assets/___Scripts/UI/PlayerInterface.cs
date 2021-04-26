using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInterface : Singleton<PlayerInterface>
{
    GameObject ghostItemUI;
    public GameObject ghostItemPrefab;
    public Text interactHintText;
    public string interactHintStr;
    public GameObject currentUI;
    public Text msgText;
    public Text camFreeText;

    void Start()
    {
        //StartCoroutine("AutoUpdateUI");
    }

    void Update()
    {
        //InventoryManager.Instance.UpdateBagUI(InventoryManager.Instance.bagInfo);
        UpdateIconOnSelected();
        ShowInteractHintWhenClose();

        if(Camera.main.GetComponent<CameraBehavior>().camMode== CameraBehavior.Mode.CAM_FREE)
        {
            camFreeText.gameObject.SetActive(true);
        }
        else
        {
            camFreeText.gameObject.SetActive(false);
        }
    }

    public void SetInterfaceActive(GameObject obj , bool isActive)
    {
        obj.SetActive(isActive);
    }

    public void CloseAllTabs()
    {
        if (currentUI)
        {
            currentUI.SetActive(false);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform interfaceTab = transform.GetChild(i);//get all interfaces under playerInterface
            for (int j = 0; j < interfaceTab.childCount; j++)
            {
                interfaceTab.GetChild(j).gameObject.SetActive(false);//and close everything under each interfaces
            }
        }
    }



    void UpdateIconOnSelected()
    {
        if (InventoryManager.Instance.selectedItemInfo.itemId != 0&& ghostItemUI==null)
        {
            //Debug.Log("On Ghost Create!");
            ghostItemUI = Instantiate(ghostItemPrefab,Input.mousePosition,Quaternion.identity, this.transform);
            string iconPath = "Icons/Material/"+ ItemManager.Instance.GetItemById(InventoryManager.Instance.selectedItemInfo.itemId).Name;
            ghostItemUI.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(iconPath);
            ghostItemUI.GetComponentInChildren<Text>().text = InventoryManager.Instance.selectedItemInfo.itemAmount.ToString();
        }
        else if (InventoryManager.Instance.selectedItemInfo.itemId != 0 && ghostItemUI)
        {
            //Debug.Log("On Ghost Move!");
            ghostItemUI.transform.position = Input.mousePosition;
            ghostItemUI.GetComponentInChildren<Text>().text = InventoryManager.Instance.selectedItemInfo.itemAmount.ToString();
        }
        else if (ghostItemUI)
        {
            //Debug.Log("On Ghost Destroy!");
            Destroy(ghostItemUI);
        }
    }

    public void ShowInteractHintWhenClose()
    {
        interactHintText.text = interactHintStr;
        if (interactHintStr != "")
        {
            interactHintText.gameObject.SetActive(true);
        }
        else
        {
            interactHintText.gameObject.SetActive(false);
        }

    }

    public void PrintMsg(string msg, Color msgColor)
    {
        StopAllCoroutines();
        msgText.color = msgColor;
        StartCoroutine("ShowMsg", (msg));
    }

    IEnumerator ShowMsg(string msg)
    {
        msgText.text = msg;
        yield return new WaitForSeconds(2);
        msgText.color = new Color(0, 0, 0, 0);
    }
}

