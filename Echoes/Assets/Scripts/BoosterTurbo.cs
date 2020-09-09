using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterTurbo : MonoBehaviour
{
    // turbo parameters
    private bool boost;
    public float booster;  // thrust multiplier
    private int boostCd;
    private int boostCdMax;
    private int boostLen;
    private int boostLenMax;

    PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // turbo parameters set
        boost = false;
        booster = 1f;  // thrust multiplier
        boostCd = 0;
        boostCdMax = 2000;
        boostLen = 0;
        boostLenMax = 600;

        thePlayer = GetComponent<PlayerController>();
    }

    void Booster(bool turnON = false)
    {
        booster = boost ? 2.5f : 1f; // not optimal but readable and editable
        thePlayer.booster = booster;

        if (turnON)
        {
            if (!boost && boostCd == 0)
            {
                Debug.Log("BOOSTER ACTIVATED!");
                boost = true;
                boostLen = boostLenMax;
            }
        }
        else
        {
            if (boost)
            {
                if (boostLen > 0)
                    boostLen -= 1;
                if (boostLen <= 0)
                {
                    boost = false;
                    boostCd = boostCdMax;
                    Debug.Log("BOOSTER DEPLETED!");
                }
            }
            else if (boostCd > 0)
            {
                boostCd -= 1;
                if (boostCd == 0)
                    Debug.Log("BOOSTER READY!");
            }
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Booster(true);
        }
    }

    void FixedUpdate()
    {
        Booster();
    }
}
