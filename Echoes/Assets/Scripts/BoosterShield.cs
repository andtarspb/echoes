using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterShield : MonoBehaviour
{
    [SerializeField]
    GameObject shield;

    bool shieldActive;

    [SerializeField]
    float shieldRecovery;       // recovery time for shield effect in seconds

    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    float recoveryTime;         // time when recovery starts
    float timeProgress;         // time we need to display the progress bar

    // UI
    [SerializeField]
    BossBarScript slider;
    bool startCountDown;        // when bar value is changes   

    // Start is called before the first frame update
    void Start()
    {
        slider.SetSliderMaxValue(shieldRecovery);
        ActivateShield(shieldActive);
    }

    void ActivateShield(bool isActive)
    {
        if (isActive)
        {
            shield.SetActive(true);

            startCountDown = true;
            timeProgress += Time.deltaTime * 5;
            if (timeProgress > shieldRecovery)
            {
                startCountDown = false;
            }
        }
        else
        {
            shield.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // display turbo progress bar
        if (startCountDown)
        {
            slider.SetSliderValue(shieldRecovery - timeProgress);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            shieldActive = true;
            ActivateShield(shieldActive);
        }
    }
}
