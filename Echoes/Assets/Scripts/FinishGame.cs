using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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

    AudioManager am;
    CameraController theCamera;
    PlayerController thePlayer;
    TimerManager theTimer;


    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        theCamera = FindObjectOfType<CameraController>();
        thePlayer = FindObjectOfType<PlayerController>();
        theTimer = FindObjectOfType<TimerManager>();

        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
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
    }
}
