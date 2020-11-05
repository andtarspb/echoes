using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    public int batCap;                     // number of batteries
    int batCharged;                 // number of charged batteries

    public int turboCap;                   // number of batteries for turbo
    int turboCharged;               // number of charged batteries for turbo

    public int shieldCap;                  // number of batteries for shield
    int shieldCharged;              // number of charged batteries for shield

    public int stealthCap;                 // number of batteries for stealth
    int stealthCharged;             // number of charged batteries for stealth

    public int magnetCap;                  // number of batteries for magnet
    int magnetCharged;              // number of charged batteries for magnet

    [SerializeField]
    GameObject batteryText;
    [SerializeField]
    GameObject systemsText;

    [SerializeField]
    Sprite fullCharge;              // image of a full charge
    [SerializeField]
    Sprite emptyCharge;             // image of an empty charge 

    [SerializeField]
    GameObject batteryPanel;        // whole battery panel
    [SerializeField]
    Image[] batteryChargesImg;      // images of battery charges
    
    [SerializeField]
    GameObject turboPanel;          // whole turbo panel [1]
    [SerializeField]
    Image[] turboChargesImg;        // images of turbo battery charges

    [SerializeField]
    GameObject shieldPanel;         // whole shield panel [2]
    [SerializeField]
    Image[] shieldChargesImg;       // images of shield battery charges

    [SerializeField]
    GameObject stealthPanel;        // whole stealth panel [3]
    [SerializeField]
    Image[] stealthChargesImg;      // images of stealth battery charges

    [SerializeField]
    GameObject magnetPanel;        // whole magnet panel [4]
    [SerializeField]
    Image[] magnetChargesImg;      // images of magnet battery charges

    BoosterTurbov2 turboBooster;    // reference to turbo booster
    BoosterShieldV2 shieldBooster;
    BoosterInvis invisBooster;
    MagnetZone magnetBooster;

    SaveManager sm;
    AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        am = FindObjectOfType<AudioManager>();

        int cap;

        // setting batery parameters
        //batCharged = 0;
        cap = sm.GetBatCap();
        SetBatCap(cap);

        // setting turbo parameters
        //turboCap = 0;
        //turboCharged = 0;
        cap = sm.GetTurboCap();
        SetTurboCap(cap);

        // setting shield parameters
        //shieldCap = 0;
        //shieldCharged = 0;
        cap = sm.GetShieldCap();
        SetShieldCap(cap);

        // setting stealth parameters
        //stealthCap = 0;
        //stealthCharged = 0;
        cap = sm.GetStealthCap();
        SetStealthCap(cap);

        // setting stealth parameters
        //magnetCap = 0;
        //magnetCharged = 0;
        cap = sm.GetMagnetCap();
        SetMagnetCap(cap);

        // display charges
        //DisplayTurboCharges();
        //DisplayShieldCharges();
        //DisplayStealthCharges();
        //DisplayMagnetCharges();

        turboBooster = FindObjectOfType<BoosterTurbov2>();
        shieldBooster = FindObjectOfType<BoosterShieldV2>();
        invisBooster = FindObjectOfType<BoosterInvis>();
        magnetBooster = FindObjectOfType<MagnetZone>();
    }

    void PowerOn(bool on)
    {
        if (on)
        {
            batCharged--;
            am.Play("power_on");
        }
        else
        {
            batCharged++;
            am.Play("power_off");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // DISPLAY TURBO CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift) && turboCharged > 0)
        {
            //Debug.Log("turbo--");
            PowerOn(false);
            turboCharged--;
            //turboBooster.powerLevel = turboCharged;
            turboBooster.SetPowerLevel(turboCharged);
            DisplayTurboCharges();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && batCharged > 0 && turboCharged < turboCap)
        {
            //Debug.Log("turbo++");
            PowerOn(true);
            turboCharged++;
            //turboBooster.powerLevel = turboCharged;
            turboBooster.SetPowerLevel(turboCharged);
            DisplayTurboCharges();
        }

        // DISPLAY SHIELD CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftShift) && shieldCharged > 0)
        {
            //Debug.Log("shield--");
            PowerOn(false);
            shieldCharged--;
            //shieldBooster.powerLevel = shieldCharged;
            shieldBooster.SetPowerLevel(shieldCharged);
            DisplayShieldCharges();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && batCharged > 0 && shieldCharged < shieldCap)
        {
            //Debug.Log("shield++");
            PowerOn(true);
            shieldCharged++;            
            //shieldBooster.powerLevel = shieldCharged;
            shieldBooster.SetPowerLevel(shieldCharged);
            DisplayShieldCharges();
        }

        // DISPLAY STEALTH CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftShift) && stealthCharged > 0)
        {
            PowerOn(false);
            stealthCharged--;
            invisBooster.powerLevel = stealthCharged;
            DisplayStealthCharges();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) && batCharged > 0 && stealthCharged < stealthCap)
        {
            //Debug.Log("stealth++");
            PowerOn(true);
            stealthCharged++;
            invisBooster.powerLevel = stealthCharged;
            DisplayStealthCharges();
        }

        // DISPLAY MAGNET CHARGES
        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftShift) && magnetCharged > 0)
        {
            PowerOn(false);
            magnetCharged--;
            magnetBooster.powerLevel = magnetCharged;
            DisplayMagnetCharges();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && batCharged > 0 && magnetCharged < magnetCap)
        {
            //Debug.Log("stealth++");
            PowerOn(true);
            magnetCharged++;
            magnetBooster.powerLevel = magnetCharged;
            DisplayMagnetCharges();
        }

    }

    public void SetBatCap(int capacity)
    {
        if (capacity == 0)
        {
            batteryPanel.SetActive(false);
            batteryText.SetActive(false);
            systemsText.SetActive(false);
        }
        else
        {
            batteryPanel.SetActive(true);
            batteryText.SetActive(true);
            systemsText.SetActive(true);
        }

        batCap = capacity;
        batCharged = batCap - (turboCharged + shieldCharged + stealthCharged + magnetCharged);
        DisplayBatteryCharges();
    }    

    public void SetTurboCap(int capacity)
    {
        if (capacity == 0)
            turboPanel.SetActive(false);
        else
            turboPanel.SetActive(true);

        turboCap = capacity;
        //turboCharged = 0;
        //SetBatCap(batCap);
        DisplayTurboCharges();
    }

    public void SetShieldCap(int capacity)
    {
        if (capacity == 0)        
            shieldPanel.SetActive(false);
        else
            shieldPanel.SetActive(true);

        shieldCap = capacity;
        //shieldCharged = 0;
        //SetBatCap(batCap);
        DisplayShieldCharges();
    }

    public void SetStealthCap(int capacity)
    {
        if (capacity == 0)
            stealthPanel.SetActive(false);
        else
            stealthPanel.SetActive(true);

        stealthCap = capacity;
        //stealthCharged = 0;
        //SetBatCap(batCap);
        DisplayStealthCharges();
    }

    public void SetMagnetCap(int capacity)
    {
        if (capacity == 0)
            magnetPanel.SetActive(false);
        else
            magnetPanel.SetActive(true);

        magnetCap = capacity;
        //magnetCharged = 0;
        //SetBatCap(batCap);
        DisplayMagnetCharges();       
    }


    void DisplayBatteryPanel(bool show)
    {
        batteryPanel.SetActive(show);
    }

    public void SaveSkills()
    {
        
        sm.SetBatCap(batCap);
        sm.SetTurboCap(turboCap);
        sm.SetShieldCap(shieldCap);
        sm.SetStealthCap(stealthCap);
        sm.SetMagnetCap(magnetCap);
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

    void DisplayMagnetCharges()
    {
        DisplayBatteryCharges();

        for (int i = 0; i < magnetChargesImg.Length; i++)
        {
            if (i < magnetCharged)
            {
                magnetChargesImg[i].sprite = fullCharge;
            }
            else
            {
                magnetChargesImg[i].sprite = emptyCharge;
            }

            if (i < magnetCap)
                magnetChargesImg[i].enabled = true;
            else
                magnetChargesImg[i].enabled = false;
        }
    }


    #region Buttons
    public void SetBatCap0()
    {
        SetBatCap(0);
    }

    public void SetBatCap1()
    {
        SetBatCap(1);
    }

    public void SetBatCap2()
    {
        SetBatCap(2);
    }

    public void SetBatCap3()
    {
        SetBatCap(3);
    }

    public void SetTurboCap0()
    {
        SetTurboCap(0);
    }
    public void SetTurboCap1()
    {
        SetTurboCap(1);
    }
    public void SetTurboCap2()
    {
        SetTurboCap(2);
    }

    public void SetShieldCap0()
    {
        SetShieldCap(0);
    }

    public void SetShieldCap1()
    {
        SetShieldCap(1);
    }

    public void SetShieldCap2()
    {
        SetShieldCap(2);
    }

    public void SetStealthCap0()
    {
        SetStealthCap(0);
    }
    public void SetStealthCap1()
    {
        SetStealthCap(1);
    }

    public void SetMagnetCap0()
    {
        SetMagnetCap(0);
    }
    public void SetMagnetCap1()
    {
        SetMagnetCap(1);
    }
    #endregion
}
