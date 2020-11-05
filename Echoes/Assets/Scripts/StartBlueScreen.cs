using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlueScreen : MonoBehaviour
{
    [SerializeField]
    GameObject blueScreen;

    [SerializeField]
    float timeFreezing;
    [SerializeField]
    float timeToShow;

    float timeToHide;

    float initThrust;
    float initRotation;

    AudioManager am;
    PlayerController thePlayer;
    Rotate radar;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        radar = FindObjectOfType<Rotate>();
    }

    IEnumerator RemoveBlueScreenForSeconds(float time)
    {
        //Debug.Log("corutine");
        yield return new WaitForSecondsRealtime(time);

        //Debug.Log("hide blue screen");
        //thePlayer.thrust = initThrust;
        Time.timeScale = 1f;
        blueScreen.SetActive(false);
    }

    public void ShowBlueScreen2(){
        // show blue screen for x seconds        
        am.Play("blue_screen");
        blueScreen.SetActive(true);

        //initThrust = thePlayer.thrust;
        //thePlayer.thrust = 0;
        //Debug.Log("show blue screen");

        StartCoroutine(RemoveBlueScreenForSeconds(timeToShow));
    }

    IEnumerator WaitFreezing(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        ShowBlueScreen2();
    }

    public void ShowBlueScreen1()
    {
        // freez time
        //initThrust = thePlayer.thrust;
        //thePlayer.thrust = 0;
        //initRotation = radar.rotationDegree;
        //radar.rotationDegree = 0;
        Time.timeScale = 0;


        StartCoroutine(WaitFreezing(timeFreezing));
    }
}
