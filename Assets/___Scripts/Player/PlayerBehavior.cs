using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    CameraBehavior cam;

    float actionTime;
    float miningTime = 3;//time for each mining action to complete

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        ActionDetection();
        MiningDetection();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBagInterface();
        }
    }

    void ToggleBagInterface()
    {
        if (BagInterface.Instance.bagUI.activeSelf)
        {
            BagInterface.Instance.bagUI.SetActive(false);
            cam.camMode = CameraBehavior.Mode.CAM_FREE;
        }
        else
        {
            BagInterface.Instance.bagUI.SetActive(true);
            cam.camMode = CameraBehavior.Mode.UI_ONLY;
        }
    }

    void ActionDetection()
    {
        if (!Input.GetKey(KeyCode.F))
        {
            ResetActionTime();
        }

    }

    void ResetActionTime()
    {
        actionTime = 0;
    }


    void MiningDetection()
    {
        if (cam.SelectedResourceName != null)
        {
            MiningInterface.Instance.ShowHint();//in range to show hint
            if (Input.GetKey(KeyCode.F))//start mining when hold "F"
            {
                MiningInterface.Instance.ShowSlider();//show progress bar
                StartMining();
            }
            else
            {
                MiningInterface.Instance.HideSlider();//stop mining and hide progress bar when release "F"
            }
        }
        else
        {
            //if not in range, hide everything and reset action time to 0
            ResetActionTime();
            MiningInterface.Instance.HideHint();
            MiningInterface.Instance.HideSlider();
        }
    }


    void StartMining()
    {
        actionTime += Time.deltaTime;
        MiningInterface.Instance.progressBar.value = actionTime/miningTime;
        if (actionTime >= miningTime)
        {
            switch (cam.SelectedResourceName)
            {
                case "Iron":
                    InventoryManager.Instance.AddItemToBag(1001, 1);
                    break;
                case "Copper":
                    InventoryManager.Instance.AddItemToBag(1002, 1);
                    break;
                case "Coal":
                    InventoryManager.Instance.AddItemToBag(1003, 1);
                    break;
                case "Limestone":
                    InventoryManager.Instance.AddItemToBag(1004, 1);
                    break;

                default:
                    Debug.Log("Resource Not Recognized: " + cam.SelectedResourceName);
                    break;
            }
            ResetActionTime();
        }
    }
}
