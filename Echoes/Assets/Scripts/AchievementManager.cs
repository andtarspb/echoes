using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    //  THIS SCRIPT IS FOR THE MENU
    //  FOR THE GAME IS AchievementGameManager

    bool aceAchieved;           // wheather achievement is achieved
    bool skillAchieved;
    bool speedAchieved;

    // references to the images on the canvas
    [SerializeField]
    Image aceImg;
    [SerializeField]
    Image skillImg;
    [SerializeField]
    Image speedImg;

    // referemces to the checkboxes
    [SerializeField]
    GameObject aceCheck;
    [SerializeField]
    GameObject skillCheck;
    [SerializeField]
    GameObject speedCheck;

    // references to the sprites
    [SerializeField]
    Sprite aceSprite;
    [SerializeField]
    Sprite skillSprite;
    [SerializeField]
    Sprite speedSprite;
    [SerializeField]
    Sprite blankSprite;

    SaveManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SaveManager>();

        SetAchievements();
    }

    public void SetAceAchieved()
    {
        sm.AchieveAce();

        SetAchievements();
    }

    public void SetSkillfulAchieved()
    {
        sm.AchieveSkillful();

        SetAchievements();
    }

    public void SetSpeedrunAchieved()
    {
        sm.AchieveSpeedrun();

        SetAchievements();
    }

    public void ResetAchievements()
    {
        // set achievements values
        sm.ResetAchievements();

        SetAchievements();
    }

    public void SetAchievements()
    {
        // get achievements values
        aceAchieved = sm.GetAceAchievement();
        skillAchieved = sm.GetSkillfulAchievement();
        speedAchieved = sm.GetSpeedrunAchievement();

        SetupImages();
    }

    void SetupImages()
    {
        // setup sprites
        aceImg.sprite = (aceAchieved) ? aceSprite : blankSprite;
        skillImg.sprite = (skillAchieved) ? skillSprite : blankSprite;
        speedImg.sprite = (speedAchieved) ? speedSprite : blankSprite;

        // setup checkboxes
        if (aceAchieved)
            aceCheck.SetActive(true);
        else 
            aceCheck.SetActive(false);
        if (skillAchieved)
            skillCheck.SetActive(true);
        else
            skillCheck.SetActive(false);
        if (speedAchieved)
            speedCheck.SetActive(true);
        else
            speedCheck.SetActive(false);

    }
}
