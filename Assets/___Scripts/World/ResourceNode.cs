using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public int miningProductIndex;
    public float defaultMiningRatePerMinute;
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
