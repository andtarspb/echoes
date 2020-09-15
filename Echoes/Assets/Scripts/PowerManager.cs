using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField]
    int bat;            // number of batteries

    int batAviable;     // number of aviable batteries

    [SerializeField]
    GameObject imgBatFull;

    [SerializeField]
    GameObject imgBatEmpty;

    [SerializeField]
    GameObject batteryPanel;

    // Start is called before the first frame update
    void Start()
    {
        batAviable = bat;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("turbo--");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("turbo++");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("shield--");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("shield++");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("stealth--");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("stealth++");
        }
    }

    void DisplayBattery(int aviableBat)
    {



        for (int i = 0; i < bat; i++)
        {

        }
    }
}
