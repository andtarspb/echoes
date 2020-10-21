using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterInvis : MonoBehaviour
{
    bool invisOn;           // if inxisibility effect is on
    bool enableInvis;       // if invisibility is aviable

    [SerializeField]
    SpriteRenderer greenCircle;
    [SerializeField]
    GameObject detectable;
    
    LineRenderer line;      // liner renderer of the radar

    [SerializeField]
    float invisLength;      // length of invisibility effect
    [SerializeField]
    float invisRecovery;    // recovery time of invisibility effect
    float recoveryRatio;

    float secondsPasses;        // how much seconds passed since activation

    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    float recoveryTime;         // time when recovery starts

    Color initCircleColor;
    Color initRadarColor;
    

    Rotate radar;

    // UI
    [SerializeField]
    BossBarScript slider;
    bool startCountDown;    // when bar value is changes


    // Start is called before the first frame update
    void Start()
    {
        radar = FindObjectOfType<Rotate>();
        line = radar.GetComponent<LineRenderer>();

        // save initial colors
        initCircleColor = greenCircle.color;
        initRadarColor = line.material.color;

        enableInvis = true;
        recoveryRatio = invisRecovery / invisLength;

        slider.SetSliderMaxValue(invisLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            slider.SetSliderValue(invisLength - secondsPasses);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (enableInvis)
            {
                invisOn = true;
            }
        }

        ActivateInvis();

    }

    void ActivateInvis()
    {
        if (invisOn)
        {
            //Debug.Log("invis");
            //invisOn = true;

            startCountDown = true;
            secondsPasses += Time.deltaTime;

            // make transparent
            greenCircle.color = new Color(initCircleColor.r, initCircleColor.g, initCircleColor.b, 0.4f);
            line.material.color = new Color(initRadarColor.r, initRadarColor.g, initRadarColor.b, 0.3f);
            // make undetectable
            detectable.SetActive(false);
            // make radar not detectable
            radar.invisible = true;

            if (secondsPasses > invisLength + 0.01f)    // when invis is over
            {
                enableInvis = false;
                invisOn = false;
                startCountDown = false;

                recoveryTime = Time.time + timeBeforeRecovery;
            }      
        }
        else
        {
            // make visible
            invisOn = false;
            greenCircle.color = new Color(initCircleColor.r, initCircleColor.g, initCircleColor.b, 1);
            line.material.color = new Color(initRadarColor.r, initRadarColor.g, initRadarColor.b, initRadarColor.a);
            detectable.SetActive(true);
            radar.invisible = false;

            if (Time.time >= recoveryTime)
            {
                if (secondsPasses > 0)  // recover boost
                {
                    secondsPasses -= Time.deltaTime / recoveryRatio;
                    //enableInvis = true;
                    startCountDown = true;
                }
                else 
                {
                    // can activate invisivility again only after it is fully recovered

                    enableInvis = true;

                    // play sound of recovery
                }
            }
        }

    }
}
