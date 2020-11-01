using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetZone : MonoBehaviour
{
    public int segments;
    float radius;
    LineRenderer line;

    public bool attract;

    [SerializeField]
    float maxRad;
    [SerializeField]
    float minRad;
    bool radShrink;
    [SerializeField]
    float animSpeed;

    public bool magnetAviable;

    public bool playerAlive;

    [SerializeField]
    float attractionLength;
    [SerializeField]
    float attractionRecovery;
    float recoveryRatio;
    [SerializeField]
    float timeBeforeRecovery;   // how long wait before recovery starts
    float recoveryTime;         // time when recovery starts

    float secondsPressed;

    [SerializeField]
    BossBarScript slider;

    AudioManager am;
    bool soundIsPlaying;

    // power manager interactions
    public int powerLevel;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();

        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        //CreatePoints();

        radius = maxRad;
        radShrink = true;

        magnetAviable = true;

        slider.SetSliderMaxValue(attractionLength);

        recoveryRatio = attractionRecovery / attractionLength;

        playerAlive = true;
    }

    void Update()
    {
        slider.SetSliderValue(attractionLength - secondsPressed);

        //if (magnetAviable)
        //{
        if (Input.GetKey(KeyCode.M) && magnetAviable && powerLevel > 0 && playerAlive)
        {
            ChangeRad();

            CreatePoints(radius);
            //ActivateMagnet();
            attract = true;

            if (!soundIsPlaying)
            {
                am.Play("magnet_on");
                soundIsPlaying = true;
            }

            secondsPressed += Time.deltaTime;
            if (secondsPressed >= attractionLength)
            {
                magnetAviable = false;
                attract = false;
                CreatePoints(0);

                secondsPressed = attractionLength;

                //recoveryTime = Time.time + timeBeforeRecovery;
            }

            recoveryTime = Time.time + timeBeforeRecovery;

        }
        else
        {
            if (Input.GetKeyUp(KeyCode.M))
            {
                RecoverMagnet();
            }
            if (secondsPressed <= attractionLength)
            {
                RecoverMagnet();
            }
        } 

       

    }

    void RecoverMagnet()
    {
        soundIsPlaying = false;
        am.Stop("magnet_on");

        attract = false;
        //Debug.Log("deactivate circle");
        CreatePoints(0);

        if (Time.time >= recoveryTime)
        {
            //magnetAviable = true;
            if (secondsPressed > 0)
            {
                secondsPressed -= Time.deltaTime / recoveryRatio;
                
                magnetAviable = true;
            }
        }
    }

    void LateUpdate()
    {
        if (attract)
        {
            ActivateMagnet();
        }
    }

    void ActivateMagnet()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidBody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidBody != null && (rigidBody.tag == "sunken" || rigidBody.tag == "mine" || rigidBody.tag == "rocket"))
            {
                // attract sunkens
                if (rigidBody.tag == "sunken" || rigidBody.tag == "rocket")
                    rigidBody.gameObject.GetComponent<MagnetAttraction>().Attract();


                // show nearby objects
                rigidBody.gameObject.GetComponent<EnemyController>().CreateBlink();
               

            }
        }
    }

    void CreatePoints(float rad)
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

    void ChangeRad()
    {        
        if (radShrink)
        {
            radius -= animSpeed * Time.deltaTime;
            if (radius <= minRad)
            {
                radShrink = !radShrink;
            }
        }
        else
        {
            radius += animSpeed * Time.deltaTime;
            if (radius >= maxRad)
            {
                radShrink = !radShrink;
            }
        }
    }
}
