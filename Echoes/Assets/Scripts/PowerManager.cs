﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    [SerializeField]
    int batCap;                     // number of batteries
    int batCharged;                 // number of charged batteries

    int turboCap;                   // number of batteries for turbo
    int turboCharged;               // number of charged batteries for turbo

    int shieldCap;                  // number of batteries for shield
    int shieldCharged;              // number of charged batteries for shield

    int stealthCap;                 // number of batteries for stealth
    int stealthCharged;             // number of charged batteries for stealth

    [SerializeField]
    Sprite fullCharge;              // image of a full charge
    [SerializeField]
    Sprite emptyCharge;             // image of an empty charge 

    [SerializeField]
    GameObject batteryPanel;        // whole battery panel
    [SerializeField]
    Image[] batteryChargesImg;      // images of battery charges
    
    [SerializeField]
    GameObject turboPanel;          // whole turbo panel
    [SerializeField]
    Image[] turboChargesImg;        // images of turbo battery charges

    [SerializeField]
    GameObject shieldPanel;         // whole shield panel
    [SerializeField]
    Image[] shieldChargesImg;       // images of shield battery charges

    [SerializeField]
    GameObject stealthPanel;        // whole stealth panel
    [SerializeField]
    Image[] stealthChargesImg;      // images of stealth battery charges


    BoosterTurbov2 turboBooster;    // reference to turbo booster

    // Start is called before the first frame update
    void Start()
    {
        // setting batery parameters
        batCharged = batCap;

        // setting turbo parameters
        turboCap = 2;
        turboCharged = 0;

        // setting shield parameters
        shieldCap = 2;
        shieldCharged = 0;

        // setting stealth parameters
        stealthCap = 2;
        stealthCharged = 0;

        // display charges
        DisplayTurboCharges();
        DisplayShieldCharges();
        DisplayStealthCharges();
    }

    // Update is called once per frame
    void Update()
    {
        // DISPLAY TURBO CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift) && turboCharged > 0)
        {
            //Debug.Log("turbo--");
            batCharged++;
            turboCharged--;
            DisplayTurboCharges();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && batCharged > 0 && turboCharged < turboCap)
        {
            //Debug.Log("turbo++");
            batCharged--;
            turboCharged++;
            DisplayTurboCharges();
        }

        // DISPLAY SHIELD CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftShift) && shieldCharged > 0)
        {
            //Debug.Log("shield--");
            batCharged++;
            shieldCharged--;
            DisplayShieldCharges();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && batCharged > 0 && shieldCharged < shieldCap)
        {
            //Debug.Log("shield++");
            batCharged--;
            shieldCharged++;
            DisplayShieldCharges();
        }

        // DISPLAY STEALTH CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftShift) && stealthCharged > 0)
        {
            batCharged++;
            stealthCharged--;
            DisplayStealthCharges();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && batCharged > 0 && stealthCharged < stealthCap)
        {
            //Debug.Log("stealth++");
            batCharged--;
            stealthCharged++;
            DisplayStealthCharges();
        }


        
    }





    void DisplayBatteryPanel(bool show)
    {
        batteryPanel.SetActive(show);
    }

    void DisplayBatteryCharges()
    {
        for (int i = 0; i < batteryChargesImg.Length; i++)
        {
            if (i < batCharged)
            {
                batteryChargesImg[i].sprite = fullCharge;
            }
            else
            {
                batteryChargesImg[i].sprite = emptyCharge;
            }

            if (i < batCap)            
                batteryChargesImg[i].enabled = true;
            else
                batteryChargesImg[i].enabled = false;
        }
    }

    void DisplayTurboCharges()
    {
        DisplayBatteryCharges();

        for (int i = 0; i < turboChargesImg.Length; i++)
        {
            if (i < turboCharged)
            {
                turboChargesImg[i].sprite = fullCharge;
            }
            else
            {
                turboChargesImg[i].sprite = emptyCharge;
            }

            if (i < turboCap)
                turboChargesImg[i].enabled = true;
            else
                turboChargesImg[i].enabled = false;
        }
    }

    void DisplayShieldCharges()
    {
        DisplayBatteryCharges();

        for (int i = 0; i < shieldChargesImg.Length; i++)
        {
            if (i < shieldCharged)
            {
                shieldChargesImg[i].sprite = fullCharge;
            }
            else
            {
                shieldChargesImg[i].sprite = emptyCharge;
            }

            if (i < shieldCap)
                shieldChargesImg[i].enabled = true;
            else
                shieldChargesImg[i].enabled = false;
        }
    }

    void DisplayStealthCharges()
    {
        DisplayBatteryCharges();

        for (int i = 0; i < stealthChargesImg.Length; i++)
        {
            if (i < stealthCharged)
            {
                stealthChargesImg[i].sprite = fullCharge;
            }
            else
            {
                stealthChargesImg[i].sprite = emptyCharge;
            }

            if (i < stealthCap)
                stealthChargesImg[i].enabled = true;
            else
                stealthChargesImg[i].enabled = false;
        }
    }
}