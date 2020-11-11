using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    [SerializeField]
    float timeToGo;

    [SerializeField]
    GameObject finishPoint;         // the point to move player towards after game is finished

    [SerializeField]
    GameObject finishText;

    [SerializeField]
    GameObject lastCheckpoint;

    // achievements panel
    [SerializeField]
    GameObject achAcePanel;
    [SerializeField]
    GameObject achSkillPanel;
    [SerializeField]
    GameObject achSpeedPanel;
    [SerializeField]
    GameObject achBlankPanel;

    // statistics panel
    [SerializeField]
    Text timeText;
    [SerializeField]
    Text skillsText;

    AudioManager am;
    CameraController theCamera;
    PlayerController thePlayer;
    TimerManager theTimer;
    AchievementGameManager achievementsMng;


    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        theCamera = FindObjectOfType<CameraController>();
        thePlayer = FindObjectOfType<PlayerController>();
        theTimer = FindObjectOfType<TimerManager>();
        achievementsMng = FindObjectOfType<AchievementGameManager>();

        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);

        achAcePanel.SetActive(false);
        achSpeedPanel.SetActive(false);
        achSkillPanel.SetActive(false);
        achBlankPanel.SetActive(false);
    }


    public void StartFinishSequence()
    {
        // save the progress
        FindObjectOfType<SaveManager>().SetStartFromBegining(1);
        FindObjectOfType<SaveManager>().SetGamePlayed(0);

        thePlayer.canControl = false;   // disable player control
        theTimer.StopTimer(false);      // stop the timer       

        // move the player up
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(thePlayer.transform.DOMove(finishPoint.transform.position, timeToGo));

        // camera stop follows the player, but radar does
        theCamera.isFollowing = false;

        StartCoroutine(ShowFinishText(timeToGo/1.5f));
    }

    IEnumerator ShowFinishText(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        am.Play("finish_screen");
        lastCheckpoint.SetActive(false);
        finishText.SetActive(true);
        
        // check achievements
        CheckAllAchivements();

        // display statistics
        DisplayStatistics();
    }

    void DisplayStatistics()
    {
        float timer = achievementsMng.GetPlayTime();
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);
        timeText.text = minutes + " min " + seconds + " s";

        int skillsCount = achievementsMng.GetSkillsCount();
        skillsText.text = skillsCount + "/8";
    }

    void CheckAllAchivements()
    {
        if (achievementsMng.CheckAceOnGameFinish())
        {
            achAcePanel.SetActive(true);
            //achBlankPanel.SetActive(false);
        }
        if (achievementsMng.CheckSpeedrunOnGameFinish())
        {
            achSpeedPanel.SetActive(true);
            //achBlankPanel.SetActive(false);
        }
        if (achievementsMng.CheckSkillfulAchievement())
        {
            achSkillPanel.SetActive(true);
            //achBlankPanel.SetActive(false);
        }
        if (!achievementsMng.CheckAceOnGameFinish() && !achievementsMng.CheckSpeedrunOnGameFinish() && !achievementsMng.CheckSkillfulAchievement())
        {
            //achAcePanel.SetActive(false);
            //achSpeedPanel.SetActive(false);
            //achSkillPanel.SetActive(false);
            achBlankPanel.SetActive(true);
        }
    }
}
