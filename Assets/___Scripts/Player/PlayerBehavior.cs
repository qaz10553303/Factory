using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using BuildSystem;
public class PlayerBehavior : MonoBehaviour
{
    CameraBehavior cam;

    float actionTime;
    float miningTime = 3;//time for each mining action to complete


    //[DllImport("user32.dll", EntryPoint = "keybd_event")]

    //public static extern void Keybd_event(

    //byte bvk,//虚拟键值 

    //byte bScan,//0

    //int dwFlags,//0为按下，1按住，2释放

    //int dwExtraInfo//0

    //);


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        MiningDetection();

        InputDetection();
    }

    void InputDetection()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cam.selectedInteractObj != null)//if currently selecting something in range
            {
                switch (cam.selectedInteractObj.layer)//check what type of gameobject is selected
                {
                    case 6://Layer6=Factory
                        if (cam.selectedInteractObj.GetComponent<Miner>())//if target is miner factory
                        {
                            cam.camMode = CameraBehavior.Mode.UI_ONLY;
                            PlayerInterface.Instance.currentUI = cam.selectedInteractObj.GetComponent<Miner>().minerUI;
                            cam.selectedInteractObj.GetComponent<Miner>().minerUI.SetActive(true);
                            BagInterface.Instance.smallBagUI.SetActive(true);

                        }
                        else if (cam.selectedInteractObj.GetComponent<Constructor>())//if target is constructor factory
                        {
                            cam.camMode = CameraBehavior.Mode.UI_ONLY;
                            PlayerInterface.Instance.currentUI = cam.selectedInteractObj.GetComponent<Constructor>().constructorInterface;
                            cam.selectedInteractObj.GetComponent<Constructor>().constructorInterface.SetActive(true);
                            BagInterface.Instance.smallBagUI.SetActive(true);

                        }

                        break;
                    case 8://Layer8=Item
                        int itemId = cam.selectedInteractObj.GetComponent<Box>().carriedItem.itemId;
                        int itemAmount = cam.selectedInteractObj.GetComponent<Box>().carriedItem.itemAmount;
                        InventoryManager.Instance.AddItemToBag(itemId, itemAmount);
                        cam.selectedInteractObj.GetComponent<Box>().DestroyBox();


                        break;
                    default:
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (cam.camMode == CameraBehavior.Mode.CAM_FREE)
            {
                TechTree.Instance.transform.GetChild(0).gameObject.SetActive(true);
                cam.camMode = CameraBehavior.Mode.UI_ONLY;
            }
            else if(cam.camMode == CameraBehavior.Mode.UI_ONLY)
            {
                TechTree.Instance.transform.GetChild(0).gameObject.SetActive(false);
                cam.camMode = CameraBehavior.Mode.CAM_FREE;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(cam.camMode== CameraBehavior.Mode.UI_ONLY)
            {
                if (GetComponentInParent<ObjectPlacer>().canPlace == true)
                {
                    //Keybd_event(81, 0, 1, 0);//simulate Q key
                    GetComponentInParent<ObjectPlacer>().Toggle();
                    GetComponentInParent<ObjectSelector>().ToggleUI();
                    PlayerInterface.Instance.CloseAllTabs();
                }
                else
                {
                    PlayerInterface.Instance.CloseAllTabs();
                    cam.camMode = CameraBehavior.Mode.CAM_FREE;
                }
            }
            else if(cam.camMode == CameraBehavior.Mode.CAM_FREE)
            {
                if(BuildInterface.Instance.buildMode == BuildInterface.BuildMode.BUILD)
                {
                    GetComponentInParent<ObjectPlacer>().Toggle();
                    GetComponentInParent<ObjectSelector>().ToggleUI();
                    PlayerInterface.Instance.CloseAllTabs();
                    return;
                }
                BagInterface.Instance.smallBagUI.SetActive(true);
                Crafting.Instance.craftingInterface.SetActive(true);
                cam.camMode = CameraBehavior.Mode.UI_ONLY;
            }
        }

    }



    //void ToggleBagInterface()
    //{
    //    if (BagInterface.Instance.bagUI.activeSelf)
    //    {
    //        BagInterface.Instance.bagUI.SetActive(false);
    //        cam.camMode = CameraBehavior.Mode.CAM_FREE;
    //    }
    //    else
    //    {
    //        BagInterface.Instance.bagUI.SetActive(true);
    //        cam.camMode = CameraBehavior.Mode.UI_ONLY;
    //    }
    //}

    //void ActionDetection()
    //{
    //    if (!Input.GetKey(KeyCode.F))
    //    {
    //        ResetActionTime();
    //    }

    //}

    //void ResetActionTime()
    //{
    //    actionTime = 0;
    //}


    void MiningDetection()
    {
        if (cam.selectedResourceName != null&&cam.camMode== CameraBehavior.Mode.CAM_FREE)
        {
            MiningInterface.Instance.ShowHint();//in range to show hint
            if (Input.GetKey(KeyCode.F))//start mining when hold "F"
            {
                AudioSource aud = GetComponent<AudioSource>();
                if (!aud.isPlaying)
                {
                    aud.Play();
                }

                MiningInterface.Instance.ShowSlider();//show progress bar
                StartMining();
            }
            else
            {
                actionTime = 0;
                MiningInterface.Instance.HideSlider();//stop mining and hide progress bar when release "F"
            }
        }
        else
        {
            //if not in range, hide everything and reset action time to 0
            actionTime = 0;
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
            switch (cam.selectedResourceName)
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
                    Debug.Log("Resource Not Recognized: " + cam.selectedResourceName);
                    break;
            }
            actionTime = 0;
        }
    }
}
