using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CameraBehavior : MonoBehaviour
{
    public float maxInteractDistance=2f;//of ray detection in the center of the screen
    public string SelectedResourceName { get; set; }

    public enum Mode
    {
        UI_ONLY,
        CAM_FREE
    }

    public Mode camMode;

    // Start is called before the first frame update
    void Start()
    {
        camMode = Mode.CAM_FREE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (camMode)
        {
            case Mode.UI_ONLY:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GetComponent<FirstPersonLook>().enabled = false;
                UICheck();
                break;
            case Mode.CAM_FREE:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (InventoryManager.Instance.selectedItemInfo.itemId != 0)
                {
                    InventoryManager.Instance.AddItemToBag(InventoryManager.Instance.selectedItemInfo.itemId, InventoryManager.Instance.selectedItemInfo.itemAmount);
                    InventoryManager.Instance.selectedItemInfo.itemId = 0;
                    InventoryManager.Instance.selectedItemInfo.itemAmount = 0;
                }
                GetComponent<FirstPersonLook>().enabled = true;
                ResourceCheck();
                break;
            default:
                break;
        }
    }

    void UICheck()
    {
        
    }

    void ResourceCheck()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position, transform.position + transform.forward * maxInteractDistance, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxInteractDistance, 1 << LayerMask.NameToLayer("Resource")))
        {
            //Interact Check
            //Debug.Log("Hit " + hitInfo.transform.name + " At " + hitInfo.point);
            SelectedResourceName = hitInfo.transform.name;
        }
        else
        {
            SelectedResourceName=null;
        }
    }
}
