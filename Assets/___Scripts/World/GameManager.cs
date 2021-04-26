using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float globalConveyorTexOffset;//to make conveyors texture look the same
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        globalConveyorTexOffset -= 0.5f * Time.deltaTime;
    }
}
