using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    int lastMassageID;

    SaveManager sm;
    AudioManager am;

    SkillMenuScript skillMng;
    PowerManager powerMng;
    AchievementGameManager achievementsMng;

    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        am = FindObjectOfType<AudioManager>();
        achievementsMng = FindObjectOfType<AchievementGameManager>();

        skillMng = FindObjectOfType<SkillMenuScript>();
        powerMng = FindObjectOfType<PowerManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if we have not saved here - indication about saving
            if (transform.position != sm.GetCheckpointPos())
            {
                am.Play("save");
                FindObjectOfType<MenuManager>().DisplaySaving(true);

                GameObject child = gameObject.transform.GetChild(0).gameObject;
            }            

            // set checkpoint position to load from
            sm.SetCheckpointPos(transform.position);

            skillMng.SavePraxis();                  // save the amount of praxis modules
            powerMng.SaveSkills();                  // save skills info
            achievementsMng.SaveIfSkillWasUsed();   // save if any of the skills was used 
            achievementsMng.SetPlayTime();          // set play time

            // desplay last messages
            if (lastMassageID != 0) 
            {
                sm.SetMessageID(lastMassageID);
            }

            
            
        }


    }
}
