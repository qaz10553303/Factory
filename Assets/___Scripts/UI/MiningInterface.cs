using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiningInterface : Singleton<MiningInterface>
{
    public Text interactHint;
    public Slider progressBar;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHint()
    {
        PlayerInterface.Instance.SetInterfaceActive(interactHint.gameObject, true);
    }

    public void ShowSlider()
    {
        PlayerInterface.Instance.SetInterfaceActive(progressBar.gameObject, true);
    }

    public void HideHint()
    {
        PlayerInterface.Instance.SetInterfaceActive(interactHint.gameObject, false);
    }

    public void HideSlider()
    {
        progressBar.value = 0;
        PlayerInterface.Instance.SetInterfaceActive(progressBar.gameObject, false);
    }

}
