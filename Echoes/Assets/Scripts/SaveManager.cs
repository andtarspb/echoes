using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool startFromBeginning;         // true - start new game

    public Vector3 startPos;                // start position - at the beginning od the game

    public Vector3 lastChackpointPos;       // position of the last checkpoint

    public bool leftPuzzle;                 // if the player solved puzzles before the boss
    public bool rightPuzzle;

    public int msgID;                       // id of the last message

    public bool gamePlayed;                 // играли ли в игру

    [SerializeField]
    GameObject startPosition;
    [SerializeField]
    GameObject escapePosition;                 // позиция игрока при рестарте после победы над боссом

    void Awake()
    {
        startFromBeginning = CheckStartFromBegining();

        if (startFromBeginning) //  new game
        {
            if (startPosition != null)
            {
                startPos = startPosition.transform.position;
            }            

            // to place the player in the beginning
            SetCheckpointPos(startPos);
            //PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
            //PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
            //PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);

            // didn't solve boss's puzzles
            SetLeftPuzzleSolved(0);
            SetRightPuzzleSolved(0);

            // didn't won the boss battle
            SetBossBattleWon(0);

            // didn't complete the game
            SetStartFromBegining(0);

            // didn't show any messages
            SetMessageID(0);

            // set initial capacities - no batteries, turbo, ...
            SetInitialCapacities();

            // set that player have not used any skills
            SetSkillUsed(false);

            // set playtime = 0
            SetPlayTime(0);
        }

        // place the player at he last checkpoint
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);

        // check boss battle won 
        if (CheckBossBattleWon())
        {
            // position the player above boss to start escaping

            //lastChackpointPos.x = 255.75f;
            //lastChackpointPos.y = -207;
            //lastChackpointPos.z = 0;

            if (escapePosition != null)
            {
                lastChackpointPos = escapePosition.transform.position;
            }
            
        }
            

        // check puzzles 
        if (CheckLeftPuzzleSolved())
            leftPuzzle = true;
        if (CheckRightPuzzleSolved())
            rightPuzzle = true;
    }

    #region achievements

    #region ace
    public void SetSkillUsed(bool wasUsed)
    {
        if (wasUsed)
        {
            PlayerPrefs.SetInt("wasSkillUsed", 1);
        }
        else
        {
            PlayerPrefs.SetInt("wasSkillUsed", 0);
        }
    }

    public bool GetSkillUsed()
    {
        int binarBool = PlayerPrefs.GetInt("wasSkillUsed");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    
    public void AchieveAce()
    {
        PlayerPrefs.SetInt("achAce", 1);
    }

    public bool GetAceAchievement()
    {
        int binarBool = PlayerPrefs.GetInt("achAce");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    #endregion

    #region skillful
    public void AchieveSkillful()
    {
        PlayerPrefs.SetInt("achSkill", 1);
    }

    public bool GetSkillfulAchievement()
    {
        int binarBool = PlayerPrefs.GetInt("achSkill");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    #endregion

    #region speedrun
   
    public void SetPlayTime(float time)
    {
        PlayerPrefs.SetFloat("playTime", time);
    }
    public float GetPlayTime()
    {
        return PlayerPrefs.GetFloat("playTime");
    }

    public void AchieveSpeedrun()
    {
        PlayerPrefs.SetInt("achSpeed", 1);
    }

    public bool GetSpeedrunAchievement()
    {
        int binarBool = PlayerPrefs.GetInt("achSpeed");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    #endregion

    public void ResetAchievements()
    {
        PlayerPrefs.SetInt("achAce", 0);
        PlayerPrefs.SetInt("achSkill", 0);
        PlayerPrefs.SetInt("achSpeed", 0); 
    }

    #endregion

    #region skills
    public void SetInitialCapacities()
    {
        PlayerPrefs.SetInt("batCap", 0);
        PlayerPrefs.SetInt("turboCap", 0);
        PlayerPrefs.SetInt("shieldCap", 0);
        PlayerPrefs.SetInt("magnetCap", 0);
        PlayerPrefs.SetInt("stealthCap", 0);
        PlayerPrefs.SetInt("praxisCount", 0);
    }

    public void SetBatCap(int cap)
    {
        PlayerPrefs.SetInt("batCap", cap);
    }
    public int GetBatCap()
    {
        return PlayerPrefs.GetInt("batCap");
    }
    public void SetTurboCap(int cap)
    {
        PlayerPrefs.SetInt("turboCap", cap);
    }
    public int GetTurboCap()
    {
        return PlayerPrefs.GetInt("turboCap");
    }
    public void SetShieldCap(int cap)
    {
        PlayerPrefs.SetInt("shieldCap", cap);
    }
    public int GetShieldCap()
    {
        return PlayerPrefs.GetInt("shieldCap");
    }
    public void SetMagnetCap(int cap)
    {
        PlayerPrefs.SetInt("magnetCap", cap);
    }
    public int GetMagnetCap()
    {
        return PlayerPrefs.GetInt("magnetCap");
    }
    public void SetStealthCap(int cap)
    {
        PlayerPrefs.SetInt("stealthCap", cap);
    }
    public int GetStealthCap()
    {
        return PlayerPrefs.GetInt("stealthCap");
    }
    public void SetPraxisCount(int praxisModules)
    {
        PlayerPrefs.SetInt("praxisCount", praxisModules);
    }
    public int GetPraxisCount()
    {
        return PlayerPrefs.GetInt("praxisCount");
    }
    #endregion

    #region Checkpoints
    public void SetCheckpointPos(Vector3 pos)
    {
        PlayerPrefs.SetFloat("lastCheckpointPosX", pos.x);
        PlayerPrefs.SetFloat("lastCheckpointPosY", pos.y);
        PlayerPrefs.SetFloat("lastCheckpointPosZ", pos.z);
    }

    public Vector3 GetCheckpointPos()
    {
        Vector3 pos;

        pos.x = PlayerPrefs.GetFloat("lastCheckpointPosX");
        pos.y = PlayerPrefs.GetFloat("lastCheckpointPosY");
        pos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ");

        return pos;
    }
    #endregion

    #region Messages
    public int GetLastMessageID()
    {
        int id = PlayerPrefs.GetInt("messagesShowed");
        return id;
    }

    public void SetMessageID(int lastMessageID)
    {
        PlayerPrefs.SetInt("messagesShowed", lastMessageID);
    }
    #endregion

    #region where to start
    public void SetStartFromBegining(int fromBegining)
    {
        PlayerPrefs.SetInt("startFromBegining", fromBegining);
    }

    public bool CheckStartFromBegining()
    {
        int binarBool = PlayerPrefs.GetInt("startFromBegining");
        if (binarBool == 1)
            return true;
        else
            return false;
    }   

    public void SetGamePlayed(int isPlayed)
    {
        PlayerPrefs.SetInt("gamePlayed", isPlayed);
    }

    public bool CheckGamePlayed()
    {
        int binarBool = PlayerPrefs.GetInt("gamePlayed");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    #endregion

    #region Boss and puzzles
    public void SetLeftPuzzleSolved(int isSolved)
    {
        PlayerPrefs.SetInt("bossLPuzzleSolved", isSolved);
    }

    public bool CheckLeftPuzzleSolved()
    {
        int binarBool = PlayerPrefs.GetInt("bossLPuzzleSolved");
        if (binarBool == 1)        
            return true;        
        else
            return false;        
    }

    public void SetRightPuzzleSolved(int isSolved)
    {
        PlayerPrefs.SetInt("bossRPuzzleSolved", isSolved);
    }

    public bool CheckRightPuzzleSolved()
    {
        int binarBool = PlayerPrefs.GetInt("bossRPuzzleSolved");
        if (binarBool == 1)
            return true;
        else
            return false;
    }

    public void SetBossBattleWon(int isDefited)
    {
        PlayerPrefs.SetInt("bossDone", isDefited);
    }

    public bool CheckBossBattleWon()
    {
        int binarBool = PlayerPrefs.GetInt("bossDone");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
    #endregion
}
