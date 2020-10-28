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

    string selectedSkill;

    // Start is called before the first frame update
    void Start()
    {
        praxisPoints = 0;
        DisplayPraxisInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPraxisModule()
    {
        //Debug.Log("praxis++");
        praxisPoints++;
        //DisplayPraxisInfo();
    }

    public void DisplayPraxisInfo()
    {
        praxisText.text = "Praxis modules: " + praxisPoints;
    }

    public void DisplayTurbo1()
    {
        selectedSkill = "turbo1";
        DisplaySkillInfo(selectedSkill);
    }

    public void DisplayShield1()
    {
        selectedSkill = "shield1";
        DisplaySkillInfo(selectedSkill);
    }
    public void DisplayMagnet()
    {
        selectedSkill = "magnet";
        DisplaySkillInfo(selectedSkill);
    }
    public void DisplayStealth()
    {
        selectedSkill = "stealth";
        DisplaySkillInfo(selectedSkill);
    }

    public void DisplayInfo()
    {
        selectedSkill = "info";
        DisplaySkillInfo(selectedSkill);
    }

    public void DisplayBat1()
    {
        selectedSkill = "bat1";
        DisplaySkillInfo(selectedSkill);
    }
    public void DisplayBat2()
    {
        selectedSkill = "bat2";
        DisplaySkillInfo(selectedSkill);
    }

    void DisplaySkillInfo(string skill)
    {
        string text = "";

        switch (skill)
        {
            case "info":
                text = @"--------INFO--------
Use praxis modules to upgrade your systems. 

Select a system for the information.";
                break;

            case "turbo1":
                text = @"Name: ""Turbo I""

Press 'SPACE' to use turbo.
Gives you a speed boost for 5 seconds.

Requires 1 energy unit!";
                break;
           
            case "shield1":
                text = @"Name: ""Shield I""

Press 'K' to activate shield.
Protects you from explosions and lasers.

Requires 1 energy unit!";
                break;

            case "magnet":
                text = @"Name: ""Magnet""

Press 'M' to activate magnet.
Detects nearby objets and attracts scrap.

Requires 1 energy unit!";
                break;

            case "stealth":
                text = @"Name: ""Invisibility""

Press 'J' to activate invisibility.
Hides you and your radar emmision from enemies.

Requires 1 energy unit!";
                break;

            case "bat1":
                text = @"Name: ""Battery I""

Gives you additional enegy unit.
Use it to power up your systems.";
                break;

            case "bat2":
                text = @"Name: ""Battery II""

Gives you additional enegy unit.
Use it to power up your systems.";
                break;
        }

        skillText.text = text;
    }
}
