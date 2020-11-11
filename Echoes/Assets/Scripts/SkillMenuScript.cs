using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuScript : MonoBehaviour
{
    int praxisPoints;

    [SerializeField]
    Text praxisText;

    [SerializeField]
    Text skillText;

    [SerializeField]
    Button upgradeButton;

    [SerializeField]
    GameObject checkTurbo1;
    [SerializeField]
    GameObject checkTurbo2;
    [SerializeField]
    GameObject checkStealth;
    [SerializeField]
    GameObject checkShield1;
    [SerializeField]
    GameObject checkShield2;
    [SerializeField]
    GameObject checkMagnet;
    [SerializeField]
    GameObject checkBat1;
    [SerializeField]
    GameObject checkBat2;

    [SerializeField]
    GameObject PressIText;

    string selectedSkill;

    PowerManager powerMng;
    SaveManager sm;
    AudioManager am;
    AchievementGameManager achievementsMng;

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        am = FindObjectOfType<AudioManager>();
        achievementsMng = FindObjectOfType<AchievementGameManager>();

        powerMng = FindObjectOfType<PowerManager>();
        upgradeButton.interactable = false;

        praxisPoints = sm.GetPraxisCount();
        DisplayPraxisInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePraxisModule(int quanity)
    {
        //Debug.Log("praxis++");
        praxisPoints += quanity;
        DisplayPraxisInfo();

        if (praxisPoints == 1 && powerMng.batCap == 0)
            ShowPressIText(true);
    }

    public void ShowPressIText(bool show)
    {
        PressIText.SetActive(show);
    }

    public void DisplayPraxisInfo()
    {
        praxisText.text = "Praxis modules: " + praxisPoints;
    }

    public void SavePraxis()
    {
        sm.SetPraxisCount(praxisPoints);
    }

    public void HandleCheckBoxes()
    {
        if (powerMng.turboCap >= 1)
            checkTurbo1.SetActive(true);
        if (powerMng.turboCap == 2)
            checkTurbo2.SetActive(true);
        if (powerMng.stealthCap == 1)
            checkStealth.SetActive(true);
        if (powerMng.shieldCap >= 1)
            checkShield1.SetActive(true);
        if (powerMng.shieldCap == 2)
            checkShield2.SetActive(true);
        if (powerMng.magnetCap == 1)
            checkMagnet.SetActive(true);
        if (powerMng.batCap >= 2)
            checkBat1.SetActive(true);
        if (powerMng.batCap == 3)
            checkBat2.SetActive(true);
    }

    void DisplaySkillInfo(string skill)
    {
        string text = "";

        switch (skill)
        {
            case "info":
                text = @"----------- INFO ----------

Use praxis modules to improve your systems. 

Select a system for the information.";
                break;

            case "turbo1":
                text = @"-------- Turbo I ----------

Press 'SPACE' to use turbo.
Gives you a speed boost for 3 seconds.";
                break;

            case "turbo2":
                text = @"-------- Turbo II ---------

Press 'SPACE' to use turbo.
Increases initial speed boost length by 150%.

Requires: Turbo I";
                break;

            case "shield1":
                text = @"-------- Shield I ---------

Press 'K' to activate the shield.
Protects you from explosions and lasers.

Warning: completley destroys missile with its praxis module!";
                break;

            case "shield2":
                text = @"-------- Shield II --------

Press 'K' to activate the shield.
Increases the ammount of damage taken by 150%.

Requires: Shield II";
                break;

            case "magnet":
                text = @"---------- Magnet ---------

Press 'M' to activate magnet.
Detects nearby objets and attracts scrap.

Requires: Turbo I";
                break;

            case "stealth":
                text = @"------- Invisibility ------

Press 'J' to activate invisibility.
Hides you and your radar emmision from enemies for 5 seconds.

Requires: Shield I";
                break;

            case "bat1":
                text = @"-------- Battery I --------

Gives you additional energy unit.
2 in total.

Use it to power up your systems.

Requires: Turbo I or Shield I";
                break;

            case "bat2":
                text = @"-------- Battery II -------

Gives you additional energy unit.
3 in total.

Use it to power up your systems.

Requires: Battery I";
                break;
        }

        skillText.text = text;
    }

    void HandleUpgradeButton(string skill)
    {
        switch (skill)
        {
            case "info":
                upgradeButton.interactable = false;
                break;
            case "turbo1":
                if (powerMng.turboCap == 0 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "turbo2":
                if (powerMng.turboCap == 1 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "stealth":
                if (powerMng.shieldCap >= 1 && powerMng.stealthCap == 0 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "shield1":
                if (powerMng.shieldCap == 0 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "shield2":
                if (powerMng.shieldCap == 1 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "magnet":
                if (powerMng.turboCap >= 1 && powerMng.magnetCap == 0 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "bat1":
                if (powerMng.batCap == 1 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
            case "bat2":
                if (powerMng.batCap == 2 && praxisPoints > 0)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
                break;
        }
    }

    public void UpgradeSkill()
    {
        switch (selectedSkill)
        {
            case "turbo1":
                powerMng.SetTurboCap(1);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;

                if (powerMng.batCap == 0)                
                    powerMng.SetBatCap(1);                

                break;
            case "turbo2":
                powerMng.SetTurboCap(2);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;                
                break;
            case "stealth":
                powerMng.SetStealthCap(1);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;
                break;
            case "shield1":
                powerMng.SetShieldCap(1);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;

                if (powerMng.batCap == 0)
                    powerMng.SetBatCap(1);

                break;
            case "shield2":
                powerMng.SetShieldCap(2);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;
                break;
            case "magnet":
                powerMng.SetMagnetCap(1);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;
                break;
            case "bat1":
                powerMng.SetBatCap(2);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;
                break;
            case "bat2":
                powerMng.SetBatCap(3);
                HandlePraxisModule(-1);
                upgradeButton.interactable = false;
                break;
        }

        achievementsMng.CheckSkillfulAchievement();

        HandleCheckBoxes();

        am.Play("skill_upgrade");
    }

    public void DisplayInfo()
    {
        selectedSkill = "info";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);
    }
    #region Buttons
    public void DisplayTurbo1()
    {
        selectedSkill = "turbo1";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);
    }
    public void DisplayTurbo2()
    {
        selectedSkill = "turbo2";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);
    }

    public void DisplayShield1()
    {
        selectedSkill = "shield1";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }
    public void DisplayShield2()
    {
        selectedSkill = "shield2";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }
    public void DisplayMagnet()
    {
        selectedSkill = "magnet";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }
    public void DisplayStealth()
    {
        selectedSkill = "stealth";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }

    public void DisplayBat1()
    {
        selectedSkill = "bat1";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }
    public void DisplayBat2()
    {
        selectedSkill = "bat2";
        DisplaySkillInfo(selectedSkill);
        HandleUpgradeButton(selectedSkill);

    }
    #endregion
}
