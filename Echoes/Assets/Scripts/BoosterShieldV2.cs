using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterShieldV2 : MonoBehaviour
{
    bool shieldOn;
    bool shieldAviable;

    [SerializeField]
    SphereCollider detectable;
    float initRad;

    [SerializeField]
    float sp;               // shield points - health of the shield
    float currentSP;        // current shield points

    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    [SerializeField]
    float recoveryRatio;         // time when recovery starts

    float recoveryTime;

    [SerializeField]
    float closeDmg;         // damage taken from mines and rockets
    [SerializeField]
    float laserDmg;         // damage taken from searchers and emitters

    [SerializeField]
    BossBarScript slider;

    PlayerController thePlayer;

    // to draw shield
    [SerializeField]
    int segments;
    [SerializeField]
    float radius;
    LineRenderer line;

    // power manager interactions
    public int powerLevel;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();

        initRad = detectable.radius;

        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        shieldAviable = true;

        //Debug.Log("sp = " + sp);
        currentSP = sp;
        slider.SetSliderMaxValue(currentSP);
    }

    // Update is called once per frame
    void Update()
    {
        slider.SetSliderValue(currentSP);

        if (Input.GetKeyDown(KeyCode.K) && powerLevel > 0)
        {
            if (shieldAviable)
            {
                ActivateShield();
            }
        }

        if (powerLevel == 0)
        {
            DeactivateShield();
        }

        // start shiled recovery
        if (Time.time >= recoveryTime && !shieldAviable)
        {
            if (currentSP < sp )   
            {
                currentSP += Time.deltaTime / recoveryRatio;
            }
            else
            {
                currentSP = sp;
                shieldAviable = true;
            }
        }
    }

    void ActivateShield()
    {
        if (!shieldOn)
        {
            //Debug.Log("shield on");
            DrawShield(radius);

            detectable.radius = 2;

            shieldOn = true;
            thePlayer.shieldOn = true;
        }
        //else
        //{
        //    DrawShield(0);

        //    detectable.radius = initRad;

        //    shieldOn = false;
        //    thePlayer.shieldOn = false;
        //}
       
    }

    void DeactivateShield()
    {
        DrawShield(0);

        detectable.radius = initRad;

        shieldAviable = false;
        shieldOn = false;
        thePlayer.shieldOn = false;
    }

    public void TakeDamage(float dmg)
    {
        currentSP -= dmg;
        recoveryTime = Time.time + timeBeforeRecovery;

        //Debug.Log("sp = " + currentSP);
        if (currentSP <= 0)
        {
            currentSP = 0; 
            DeactivateShield();
        }
    }

    void DrawShield(float rad)
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * rad;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * rad;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
}
