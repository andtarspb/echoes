﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;                // приложенная сила  

    public Rigidbody rb;


    // PRIVATE INIT
    bool isRadarOn;
    [SerializeField]
    float radarRadius;

    GameObject radarRay;

    Vector3 mousePos;   // координаты мыши
    Vector3 direction;  // направление куда смотрит игрок

    // references
    CameraShake camShake;

    // managers
    LevelManager lvlManager;
    MenuManager menuManager;
    BlinkManager blinkManager;
    AudioManager audioManager;
    TimerManager timerManager;

    float nextTimeblink;
    float nextTimeblinkSunken;
    [SerializeField]
    float blinkDelay = 0.5f;

    public bool inSafeZone;    // if player in safe zone    

    public bool canControl;              // if player canMove - not in a cutscene

    // turbo booster
    public float booster = 1f;
    public bool turboOn;
    public bool shieldOn;
    MagnetZone magnet;
    BoosterTurbov2 turbo;
    BoosterShieldV2 shield;

    void Start()
    {
        isRadarOn = true;
        canControl = true;

        //rb = GetComponent<Rigidbody>();
        var radar = FindObjectOfType<Rotate>();
        radarRay = radar.gameObject;

        camShake = FindObjectOfType<CameraShake>();
        lvlManager = FindObjectOfType<LevelManager>();
        menuManager = FindObjectOfType<MenuManager>();
        blinkManager = FindObjectOfType<BlinkManager>();
        audioManager = FindObjectOfType<AudioManager>();
        timerManager = FindObjectOfType<TimerManager>();

        shield = GetComponent<BoosterShieldV2>();
        magnet = FindObjectOfType<MagnetZone>();
        turbo = FindObjectOfType<BoosterTurbov2>();

        direction = Vector3.up;

        nextTimeblink = Time.time;
        nextTimeblinkSunken = Time.time;
    }

    void FixedUpdate()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction

        if (canControl)
        {
            MovePlayer();
        }

        OtherInput();
    }

    void MovePlayer()
    {
        float realThrust = thrust;
        if (turboOn)
        {
            realThrust = thrust * booster;
        }


        // считаем расстояние от камеры до игрока, чтоб перемещаться только если курсор внутри ИКО
        float dst = Vector3.Distance(mousePos, transform.position) - Mathf.Abs(Camera.main.transform.position.z);
        //Debug.Log("dst to click: " + dst);

        // if player controlles with mouse - move towards mouse
        ////if (Input.GetMouseButton(1) && dst < radarRadius)
        ////{
        ////    Vector3 force = transform.up * thrust * 1.5f;
        ////    rb.AddForce(force);
        ////    //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        ////}
        ////else 
        if (Input.GetMouseButton(0) && dst < radarRadius)
        {
            Vector3 force = transform.up * realThrust;
            rb.AddForce(force);
            //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        }
#if (UNITY_EDITOR)
        //else if (Input.GetMouseButtonDown(1) && dst < radarRadius)
        //{
        //    transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);            
        //}
#endif
        // if player controlles with keyboard - move according to keuboard
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector3 force = new Vector3(inputX, inputY, 0f);

            rb.AddForce(Vector3.ClampMagnitude(force, 1) * realThrust);
        }

    }

    void FaceMouse(Vector3 mousePosition)
    {
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //Debug.Log("Mouse position (player): " + mousePos.x + "; " + mousePos.y);
    }

    void OtherInput()
    {
#if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.P))    // turn on/off radar
        {
            isRadarOn = !isRadarOn;
            radarRay.SetActive(isRadarOn);
        }

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    menuManager.LevelCompleted();
        //}

        // проверка трясущейся камеры: слабое и сильное трясение
        if (Input.GetKeyDown(KeyCode.Z))
        {
            camShake.SmallShake();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            camShake.Shake();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            timerManager.StartTimer();
        }
#endif
    }

    void HandleObstacleCollision(Collision collision)
    {
        // если игрок сталкивается с препятстием - покажи точку соприкосновение и потряси
        if ((collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "door"
            || collision.gameObject.tag == "generator") && (Time.time >= nextTimeblink))
        {
            camShake.SmallShake();

            audioManager.Play("wall_hit");

            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.tag == "obstacle")
                    blinkManager.CreateBlink(blinkManager.blinkGreen, contact.point);
                if (collision.gameObject.tag == "door")
                    blinkManager.CreateBlink(blinkManager.blinkGray, collision.transform.position);

            }

            nextTimeblink = Time.time + blinkDelay;
        }
    }

    void HandleSunkenCollision(Collision collision, bool enter)
    {

        // если игрок сталкивается с обломком - покажи его
        if (collision.gameObject.tag == "sunken" && (Time.time >= nextTimeblinkSunken))
        {
            blinkManager.CreateBlinkFollow(blinkManager.blinkCircleGray, collision.transform.position, collision.gameObject);
            nextTimeblinkSunken = Time.time + blinkDelay;

            if (enter)
            {
                audioManager.Play("sunken_hit");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleObstacleCollision(collision);
        HandleSunkenCollision(collision, true);
    }

    void OnCollisionStay(Collision collision)
    {
        HandleObstacleCollision(collision);
        HandleSunkenCollision(collision, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "end")
        {
            menuManager.LevelCompleted();
            //Debug.Log("Level completed!");
        }

        if (other.tag == "mine" || other.tag == "mine_boss" || other.tag == "rocket" || other.tag == "persuer")
        {
            if (other.gameObject.GetComponent<RocketBoss>())
            {
                other.gameObject.GetComponent<RocketBoss>().LaunchNewRocket();
            }


            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy(false, false);

            DestroyPlayer(11, false, false);
        }

        if (other.tag == "safe_zone")
        {
            inSafeZone = true;
        }

        lvlManager.ResetArrays();

        // dark section
        if (other.tag == "dark_start")
        {
            radarRay.SetActive(false);
            if (other.GetComponent<StartBlueScreen>() != null)
            {
                //audioManager.Play("radar_off");

                //other.GetComponent<BlowUpMine>().BlowMine();
                other.GetComponent<StartBlueScreen>().ShowBlueScreen1();
                //other.gameObject.SetActive(false);
                other.gameObject.transform.position = new Vector3(
                    other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z - 100);
            }
        }
        if (other.tag == "dark_stop")
        {
            if (!radarRay.activeSelf)
            {
                audioManager.Play("radar_on");
                radarRay.SetActive(true);
            }
        }

        // boss setion
        if (other.tag == "boss_start")
        {
            // start boss battle
            FindObjectOfType<BossBatleManager>().StartPhase1();
            other.gameObject.SetActive(false);
        }

        // туннель вслепую
        if (other.tag == "tunnel_start")
        {
            other.gameObject.SetActive(false);
            audioManager.Play("laser_on");
            FindObjectOfType<TunnelEmittersManager>().ActivateEmitters();
        }
        //if (other.tag == "tunnel_stop")
        //{
        //    other.gameObject.SetActive(false);
        //    audioManager.Play("radar_off");
        //    FindObjectOfType<TunnelEmittersManager>().DeactivateEmitters();
        //}

        // finish the game
        if (other.tag == "game_finish")
        {
            //Debug.Log("Game completed!");

            FindObjectOfType<FinishGame>().StartFinishSequence();
            //menuManager.GameCompleted();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "safe_zone")
        {
            inSafeZone = false;
        }
    }

    public bool DestroyPlayer(float dmg, bool ignoreShield, bool handleInvinsibility)
    {
        if ((!inSafeZone && !shieldOn) || ignoreShield)    // if player not in the safe zone or not under the shield - destroy him
        {
            audioManager.Stop("turbo_on");
            magnet.playerAlive = false;

            camShake.Shake();
            timerManager.StopTimer(true);

            audioManager.Play("explosion");

            // deactivate marker
            EndMarkScript endMark = FindObjectOfType<EndMarkScript>();

            if (endMark != null)
            {
                endMark.SetMarker(false, true);
            }

            magnet.magnetAviable = false;

            blinkManager.CreateBlink(blinkManager.blinkCircleOrange, transform.position);
            MakeVisible(false);
            menuManager.PlayerDead();

            return true;
        }
        else if (!inSafeZone)
        {
            shield.TakeDamage(dmg, !handleInvinsibility);
            audioManager.Play("blowup_enemy");
        }

        return false;
    }

    public void MakeVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        radarRay.SetActive(isVisible);
    }
}
