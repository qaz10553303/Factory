using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float textureOffset;
    public List<Box> currentBoxesOnThis= new List<Box>();
    public List<Box> incomingBoxes = new List<Box>();

    Material mat;
    bool hasMat;

    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
        if (transform.Find("Belt"))
        {
            hasMat = true;
            mat = transform.Find("Belt").GetComponent<MeshRenderer>().material;
        }

    }

    // Update is called once per frame
    void Update()
    {
        SetTexture();
        if (GetComponentInParent<Miner>())
        {
            GetComponentInParent<Miner>().allowOutput = currentBoxesOnThis.Count > 0 ? false : true;
        }
        if (GetComponentInParent<Constructor>())
        {
            GetComponentInParent<Constructor>().allowOutput = currentBoxesOnThis.Count > 0 ? false : true;
        }

    }

    void SetTexture()
    {
        if (!hasMat) return;
        textureOffset = GameManager.Instance.globalConveyorTexOffset;

        mat.SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
    }

    

    private void OnCollisionStay(Collision collision)
    {
        BuildNode node = GetComponentInChildren<BuildNode>();
        if (collision.transform.tag == "Box")
        {
            Debug.Log("have box!");
            Box box = collision.transform.GetComponent<Box>();
            if(!currentBoxesOnThis.Contains(box))
            {
                currentBoxesOnThis.Add(box);
                box.listsContainThis.Add(currentBoxesOnThis);
            }
            if (incomingBoxes.Contains(box)&&!box.hasTarget)
            {
                incomingBoxes.Remove(box);
                box.listsContainThis.Remove(incomingBoxes);
            }
            if (node.hasBuildingOnNode && node.buildingTypeOnNode == "Conveyor")
            {
                if (!box.hasTarget)
                {
                    if (node.buildingGoOnNode) 
                    {
                        if (node.buildingGoOnNode.GetComponent<ConveyorBelt>().currentBoxesOnThis != null)
                        {
                            List<Box> boxesInFront = node.buildingGoOnNode.GetComponent<ConveyorBelt>().currentBoxesOnThis;

                            if (boxesInFront.Count < 1 || (boxesInFront.Count == 1 && boxesInFront[0].hasTarget))
                            {
                                if (node.buildingGoOnNode.GetComponent<ConveyorBelt>().incomingBoxes.Count < 1)
                                {
                                    box.MoveToTarget(GetComponentInChildren<BuildNode>().transform.position);
                                    node.buildingGoOnNode.GetComponent<ConveyorBelt>().incomingBoxes.Add(box);
                                    box.listsContainThis.Add(node.buildingGoOnNode.GetComponent<ConveyorBelt>().incomingBoxes);
                                }
                            }
                        }

                    }


                }
            }
            else if (node.hasBuildingOnNode && node.buildingTypeOnNode == "Constructor")
            {
                if (!box.hasTarget)
                {
                    if (node.buildingGoOnNode)
                    {
                        Constructor con = node.buildingGoOnNode.GetComponent<Constructor>();
                        if (!con.allowInput) return;
                        for (int i = 0; i < con.workSlots.inputs.Count; i++)
                        {
                            if (con.workSlots.inputs[i].itemsCanStore.Contains(box.carriedItem.itemId))
                            {
                                if (con.workSlots.inputs[i].itemAmount < 100)
                                {
                                    box.MoveToTarget(GetComponentInChildren<BuildNode>().transform.position);
                                }
                            }
                        }
                    }
                    
                }
            }
        }
    }

        
    

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Box")
        {
            Box box = collision.transform.GetComponent<Box>();
            currentBoxesOnThis.Remove(box);
            box.listsContainThis.Remove(currentBoxesOnThis);
        }
    }
}
