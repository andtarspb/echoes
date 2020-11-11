using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementGameManager : MonoBehaviour
{
    int skillsCount;    // how many skills player have
    bool isAce;         // if the player is still an ace

    // reference to managers
    SaveManager sm;
    PowerManager pm;

    [SerializeField]
    Text playTimeText;      // text to display playtime
    float playTime;         // actual playtime
    [SerializeField]
    int speedrunTime;       // max time for a speedrun in minutes

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        pm = FindObjectOfType<PowerManager>();

        CheckIfSkillWasUsed();

        playTime = sm.GetPlayTime();
    }

    void Update()
    {
        // calculate playtime
        playTime += Time.deltaTime;
        //playTimeText.text = String.Format("{0:0.00}", playTime);
    }

    #region Speedrun
    public void SetPlayTime()
    {
        sm.SetPlayTime(playTime);
    }   

    public float GetPlayTime()
    {
        return playTime;
    }

    public bool CheckSpeedrunOnGameFinish()
    {
        //print("playtime: " + playTime);
        //print("speedrunTime: " + speedrunTime*60);

        if (playTime <= speedrunTime * 60)
        {
            sm.AchieveSpeedrun();
            return true;
        }

        return false;
    }

    #endregion

    #region Ace
    public bool CheckAceOnGameFinish()
    {
        //print("ACE: " + isAce);

        // if didn't use skills - unlock achievement
        if (isAce)
        {
            sm.AchieveAce();
            return true;
        }

        return false;
    }

    public void SaveIfSkillWasUsed()
    {
        sm.SetSkillUsed(!isAce);
    }

    void CheckIfSkillWasUsed()
    {
        if (sm.GetSkillUsed())
        {
            isAce = false;
        }
        else
        {
            isAce = true;
        }
    }

    public void NotAceAnymore()
    {
        isAce = false;
    }
    #endregion

    #region Skillful
    public bool CheckSkillfulAchievement()
    {
        //skillsCount = sm.GetBatCap() + sm.GetTurboCap() + sm.GetShieldCap() + sm.GetMagnetCap() + sm.GetStealthCap();
        skillsCount = pm.batCap + pm.turboCap + pm.shieldCap + pm.magnetCap + pm.stealthCap;

        //print("skillsCount = " + skillsCount);

        if (skillsCount == 9)
        {
            // skillful achievement unlock
            sm.AchieveSkillful();
            return true;
        }

        return false;
    }

    public int GetSkillsCount()
    {
        if (skillsCount == 0)
        {
            return 0;
        }
        return skillsCount-1;
    }
    #endregion
}
