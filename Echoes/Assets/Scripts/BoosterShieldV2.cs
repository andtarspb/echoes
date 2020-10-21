using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterShieldV2 : MonoBehaviour
{
    bool shieldOn;

    [SerializeField]
    float sp;               // shield points - health of the shield

    [SerializeField]
    float closeDmg;         // damage taken from mines and rockets
    [SerializeField]
    float laserDmg;         // damage taken from searchers and emitters

    [SerializeField]
    BossBarScript slider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateShield()
    {

    }

    void TakeDamage(float dmg)
    {
        sp -= dmg;
    }
}
