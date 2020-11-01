using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterTurbov2 : MonoBehaviour
{
    bool turboOn;           // if turbo effect is ON
    bool enableTurbo;       // if turbo is aviable

    [SerializeField]
    float turboBoost;       // multiplier to the player's thrust
    [SerializeField]
    float radarBoost;       // multiplier to the radar's rotation speed

    float secondsPressed;   // time of space bar being pressed in seconds

    float recoveryBoost;

    [SerializeField]
    float turboLength;      // length of turbo effect in seconds
    float tp;               // turbo points

    [SerializeField]
    float turboRecovery;    // recovery time of turbo effect in seconds
    float recoveryRatio;

    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    float recoveryTime;         // time when recovery starts

    PlayerController theplayer;
    Rotate theRadar;

    AudioManager am;
    bool soundIsPlaying;

    // UI
    [SerializeField]
    BossBarScript slider;
    bool startCountDown;    // when bar value is changes


    // power manager interactions
    public int powerLevel;

    // Start is called before the first frame update
    void Start()
    {
        theplayer = GetComponent<PlayerController>();
        theRadar = FindObjectOfType<Rotate>();
        am = FindObjectOfType<AudioManager>();

        // set player's and radar's values
        theplayer.booster = turboBoost;
        theRadar.radarBoost = radarBoost;

        enableTurbo = true;
        recoveryRatio = turboRecovery / turboLength;

        slider.SetSliderMaxValue(turboLength);
    }
    
    public void SetPowerLevel(int levelToSet)
    {
        powerLevel = levelToSet;

        if (powerLevel == 1)
        {
            recoveryBoost = 1;
        }
        else if (powerLevel == 2)
        {
            recoveryBoost = 1.5f;
        }
    }

    void Booster()
    {
        // set player's and radar's values
        theplayer.turboOn = turboOn;
        theRadar.turboOn = turboOn;
               

        if (turboOn)    // turbo on
        {
            // start countdown
            startCountDown = true;
            secondsPressed += Time.deltaTime;
            tp = secondsPressed / recoveryBoost;
            //Debug.Log("seconds pressed:" + secondsPressed);

            if (!soundIsPlaying)
            {
                am.Play("turbo_on");
                soundIsPlaying = true;
            }

            if (tp > turboLength)   // when turbo is over
            {
                //Debug.Log("STOPE");
                enableTurbo = false;
                turboOn = false;
                startCountDown = false;

                recoveryTime = Time.time + timeBeforeRecovery;
            }
        }
        else
        {
            soundIsPlaying = false;
            am.Stop("turbo_on");

            if (Time.time >= recoveryTime)  // after recovery time - start recovery
            {
                if (tp > 0) // recover booster
                {                   

                    tp -= Time.deltaTime / recoveryRatio;

                    if (powerLevel != 2)
                    {
                        secondsPressed = tp;
                    }
                    else
                    {
                        secondsPressed = tp * recoveryBoost;
                    }
                    
                    enableTurbo = true;
                    startCountDown = true;
                }
            }
        }        
    }

    void FixedUpdate()
    {
        Booster();
    }

    // Update is called once per frame
    void Update()
    {
        // display turbo progress bar
        if (startCountDown)
        {
            slider.SetSliderValue(turboLength - tp);

        }


        if (Input.GetKey(KeyCode.Space) && powerLevel > 0)    
        {
            if (enableTurbo)
            {
                turboOn = true;
            }            
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            turboOn = false;
            if (powerLevel > 0)
            {
                recoveryTime = Time.time + timeBeforeRecovery;
            }
           
        }
        else if (powerLevel == 0)
        {
            turboOn = false;
        }
    }
}
