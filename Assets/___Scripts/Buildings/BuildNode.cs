using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour
{
    public bool hasBuildingOnNode;
    public string buildingTypeOnNode;
    public GameObject buildingGoOnNode;
    float checkTimer;
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer += Time.deltaTime;
        if(checkTimer > 1)
        {
            hasBuildingOnNode = false;
            buildingTypeOnNode = null;
            buildingGoOnNode = null;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Factory"))
        {
            checkTimer = 0;
            hasBuildingOnNode = true;
            buildingTypeOnNode = other.transform.tag;
            buildingGoOnNode = other.gameObject;
        }
    }

       
}
