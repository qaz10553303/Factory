using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CameraBehavior : MonoBehaviour
{
    public float maxInteractDistance=2f;//of ray detection in the center of the screen
    public string selectedResourceName;
    public GameObject selectedInteractObj;

    public GameObject testFactoryPrefab;
    public GameObject currentFactory;
    public Material ghostMat;
    public enum Mode
    {
        UI_ONLY,
        CAM_FREE,
    }

    public Mode camMode;



    // Start is called before the first frame update
    void Start()
    {
        //camMode = Mode.CAM_FREE;

    }

    // Update is called once per frame
    void Update()
    {

        CamModeCheck();
    }




    void CamModeCheck()
    {
        switch (camMode)
        {
            case Mode.UI_ONLY:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GetComponentInParent<FirstPersonMovement>().enabled = false;
                GetComponent<FirstPersonLook>().enabled = false;

                selectedResourceName = null;//for resource check

                PlayerInterface.Instance.interactHintStr = "";//for interact check
                selectedInteractObj = null;
                break;
            case Mode.CAM_FREE:
                //PlayerInterface.Instance.CloseAllTabs();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (InventoryManager.Instance.selectedItemInfo.itemId != 0)
                {
                    InventoryManager.Instance.AddItemToBag(InventoryManager.Instance.selectedItemInfo.itemId, InventoryManager.Instance.selectedItemInfo.itemAmount);
                    InventoryManager.Instance.selectedItemInfo.itemId = 0;
                    InventoryManager.Instance.selectedItemInfo.itemAmount = 0;
                }
                GetComponentInParent<FirstPersonMovement>().enabled = true;
                GetComponent<FirstPersonLook>().enabled = true;
                Debug.DrawLine(transform.position, transform.position + transform.forward * maxInteractDistance, Color.red);
                RaycastCheck();
                break;
            default:
                break;
        }
    }

    void RaycastCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxInteractDistance))//interact check
        {
            //Interact Check
            //Debug.Log("Hit " + hitInfo.transform.name + " At " + hitInfo.point);
            InteractCheck(hitInfo);
            ResourceCheck(hitInfo);
        }
        else
        {
            selectedResourceName = null;//for resource check

            PlayerInterface.Instance.interactHintStr = "";//for interact check
            selectedInteractObj = null;
        }
    }



    void ResourceCheck(RaycastHit hitInfo)
    {
        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            selectedResourceName = hitInfo.transform.name;
        }
    }

    void InteractCheck(RaycastHit hitInfo)//check interactive items, factories, items, etc
    {
        if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            selectedInteractObj = hitInfo.transform.gameObject;
            Box box = hitInfo.transform.GetComponent<Box>();
            string ItemName = ItemManager.Instance.GetItemById(box.carriedItem.itemId).Name.Replace("_", " ");
            PlayerInterface.Instance.interactHintStr = ItemName + "("+ box.carriedItem.itemAmount+")";
        }
        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Factory"))
        {
            selectedInteractObj = hitInfo.transform.gameObject;
            if (hitInfo.transform.tag == "Conveyor") 
            {
                PlayerInterface.Instance.interactHintStr ="";
                return;
            }
            PlayerInterface.Instance.interactHintStr = "F -- Open";
        }
    }
}
