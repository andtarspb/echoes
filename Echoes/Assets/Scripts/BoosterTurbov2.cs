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

    [SerializeField]
    float turboLength;      // length of turbo effect in seconds

    [SerializeField]
    float turboRecovery;    // recovery time of turbo effect in seconds
    float recoveryRatio;

    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    float recoveryTime;         // time when recovery starts

    PlayerController theplayer;
    Rotate theRadar;


    // UI
    [SerializeField]
    BossBarScript slider;
    bool startCountDown;    // when bar value is changes

    
    // Start is called before the first frame update
    void Start()
    {
        theplayer = GetComponent<PlayerController>();
        theRadar = FindObjectOfType<Rotate>();

        // set player's and radar's values
        theplayer.booster = turboBoost;
        theRadar.radarBoost = radarBoost;

        enableTurbo = true;
        recoveryRatio = turboRecovery / turboLength;

        slider.SetSliderMaxValue(turboLength);
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
            //Debug.Log("seconds pressed:" + secondsPressed);

            if (secondsPressed > turboLength)   // when turbo is over
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
            if (Time.time >= recoveryTime)  // after recovery time - start recovery
            {
                if (secondsPressed > 0) // recover booster
                {
                    secondsPressed -= Time.deltaTime / recoveryRatio;
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
            slider.SetSliderValue(turboLength - secondsPressed);
        }


        if (Input.GetKey(KeyCode.Space))    
        {
            if (enableTurbo)
            {
                turboOn = true;
            }            
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            turboOn = false;
            recoveryTime = Time.time + timeBeforeRecovery;
        }
    }
}
