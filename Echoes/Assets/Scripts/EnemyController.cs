﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // PUBLIC INIT    
    public bool startChasing;
    public Vector3 targetPosition;
    
    public BlinkManager bm;
    public Rigidbody rb;

    public static List<Rigidbody> EnemyRBs;

    // PRIVATE INIT
    float blinkGap = 1;
    float nextTimeBlink;

    float detectBlinkDelay = 0.4f;
    float nextTimeDetectBlink;

    public GameObject[] blinkType = new GameObject[2];

    [SerializeField]
    int explosionForce;
    [SerializeField]
    int explosionRadius;

    LevelManager lvlManager;
    AudioManager audioManager;
    CameraShake camShake;

    float initDrag;

    float nextTimeChangeDrag;
    float dragChangeDelay = 1;

    // time to next explosion for sunken
    float explGap = 0.2f;
    float nextExpl;

    // Start is called before the first frame update
    public void Start()
    {
        AssignRBs();
        rb = GetComponent<Rigidbody>();

        bm = FindObjectOfType<BlinkManager>();
        lvlManager = FindObjectOfType<LevelManager>();        
        camShake = FindObjectOfType<CameraShake>();
        audioManager = FindObjectOfType<AudioManager>();

        blinkType = AssignIcon();
        
        bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);

        if (gameObject.tag == "sunken")
            initDrag = GetComponent<Rigidbody>().drag;
    }

    GameObject[] AssignIcon()
    {
        GameObject[] blink = new GameObject[2];

        switch (gameObject.tag)
        {
            case "mine":
                blink[0] = bm.blinkCrossRed;
                blink[1] = bm.blinkCrossOrange;
                break;
            case "mine_boss":
                blink[0] = bm.blinkCrossRedLong;
                blink[1] = bm.blinkCrossOrange;
                break;
            case "rocket":
                blink[0] = bm.blinkTriangleRed;
                blink[1] = bm.blinkTriangleOrange; ;
                break;
            case "persuer":
                blink[0] = bm.blinkCircleRed;
                blink[1] = bm.blinkCircleOrange;
                break;
            case "runaway":
                blink[0] = bm.blinkCirclePink;
                blink[1] = bm.blinkCircleOrange;
                break;
            case "sunken":
                blink[0] = bm.blinkCircleGray;
                blink[1] = bm.blinkCircleGray;
                break;
            case "door":
                blink[0] = bm.blinkGray;
                blink[1] = bm.blinkGray;
                break;
        }

        return blink;
    }

    public void CreateBlink()
    {
        if (nextTimeBlink < Time.time)
        {
            if (gameObject.tag == "mine" && !GetComponent<WaypointMovement>())
            {
                bm.CreateBlink(blinkType[0], transform.position);
            }
            else
                bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);

            nextTimeBlink = Time.time + blinkGap;
        }        
    }

    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    public void AssignRBs()
    {
        rb = GetComponent<Rigidbody>();

        if (EnemyRBs == null)
        {
            EnemyRBs = new List<Rigidbody>();
        }
        EnemyRBs.Add(rb);
    }

    public void FaceTarget(Vector3 targetPos)
    {
        Vector3 difference = targetPos - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    public void ChaseToPosition(Vector3 positionToChase)
    {
        startChasing = true;
        targetPosition = positionToChase;

        if (nextTimeDetectBlink < Time.time)
        {
            bm.CreateBlink(bm.blinkRed, positionToChase);
            nextTimeDetectBlink = Time.time + detectBlinkDelay;
        }
    }

    public void ChangeDrag(float dragValue, bool doChange)
    {
        if (doChange)
        {
            rb.drag = dragValue;

            nextTimeChangeDrag = Time.time + dragChangeDelay;
        }
        else if (Time.time > nextTimeChangeDrag)
        {
            rb.drag = dragValue;
        }
    }

    public void BlowUpEnemy(bool playSound, bool givePraxis)
    {      
        if (Vector3.Distance(FindObjectOfType<PlayerController>().transform.position, transform.position) < 20)
        {
            if (playSound)
                audioManager.Play("blowup_enemy");

            // создаем взрыв
            CreateExplosion(explosionRadius);

            // отображаем взрыв
            bm.CreateBlink(blinkType[1], transform.position);

            camShake.MediumShake();
        }

        // отделяем дочерний блинк если есть
        if (gameObject.transform.childCount > 0)
        {
            //Debug.Log("Дочерних объектов: " + gameObject.transform.childCount);
            GameObject childBlink = gameObject.transform.GetChild(0).gameObject;
            childBlink.transform.parent = null;
            childBlink.transform.position = Vector3.zero;
        }

        if (gameObject.CompareTag("rocket") && givePraxis)
        {
            if (!gameObject.GetComponent<RocketBoss>())
            {
                GameObject praxis = gameObject.GetComponent<RocketController>().praxis;
                Instantiate(praxis, transform.position, Quaternion.identity);

            }
            
        }

        // потом уничтожаем сам объект
        Destroy(gameObject);
        //gameObject.SetActive(false);

        lvlManager.ResetArrays();
    }

    private void OnDestroy()
    {
        EnemyRBs.Remove(rb);
    }

    void CreateExplosion(float radiusOfExplosion)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusOfExplosion);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidBody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidBody != null && (rigidBody.tag == "sunken" || rigidBody.tag == "Player"))
            {
                // apply explosion force
                rigidBody.AddExplosionForce(explosionForce, transform.position, radiusOfExplosion);

                // show blinks    
                if (rigidBody.tag == "sunken")    
                    bm.CreateBlinkFollow(rigidBody.gameObject.GetComponent<EnemyController>().blinkType[1], rigidBody.transform.position, rigidBody.gameObject);
            }
        }
    }

    void CreateExplosion(float radiusOfExplosion, Vector3 explosionPos)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radiusOfExplosion);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidBody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidBody != null && (rigidBody.tag == "sunken" || rigidBody.tag == "Player") && (Time.time >= nextExpl))
            {
                // apply explosion force
                rigidBody.AddExplosionForce(explosionForce, explosionPos, radiusOfExplosion);

                // show blinks    
                if (rigidBody.tag == "sunken")
                    bm.CreateBlinkFollow(rigidBody.gameObject.GetComponent<EnemyController>().blinkType[1], rigidBody.transform.position, rigidBody.gameObject);
                //Debug.Log("boom!");
                nextExpl = Time.time + explGap;
            }                      
        }
    }

    void HandleMineBossDestroy()
    {
        bm.CreateBlink(blinkType[1], transform.position);
        transform.position = Vector3.zero;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // если обломок сталкивается с другими врагами - взорви их
        if (gameObject.tag == "sunken" && (other.tag == "mine"))//|| other.tag == "rocket" || other.tag == "persuer"))
        {
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy(true, false);
            lvlManager.ResetArrays();
        }

        // если мина босса сталкивается с перпятсвием
        if (other.gameObject.tag == "obstacle" && gameObject.tag == "mine_boss")
        {
            HandleMineBossDestroy();
        }

        //// если обломок сталкивается с генератором
        //if (gameObject.tag == "sunken" && other.tag == "generator")
        //{
        //    other.GetComponent<GeneratorController>().DestroyGenerator();
        //    Debug.Log("обломок сталкивается с генератором ");
        //}
    }

    void OnCollisionEnter(Collision collision)
    {     
        // если обломок сталкивается с обломком - покажи его
        if (collision.gameObject.tag == "sunken" && gameObject.tag == "sunken")
        {
            //bm.CreateBlinkFollow(blinkType[0], collision.transform.position, collision.gameObject);
            bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);
        }

        // если обломок сталкивается с игроком - drag как сначала
        if (collision.gameObject.tag == "Player" && gameObject.tag == "sunken")
        {
            //gameObject.GetComponent<Rigidbody>().drag = initDrag;
            ChangeDrag(initDrag, true);
        }

        // если обломок сталкивается со стеной - показываем значек
        if ((collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "BBeaconEmitter" || collision.gameObject.tag == "emitter") 
            && gameObject.tag == "sunken")
        {
            bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);

            // отталкиваем обломок
            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.tag == "obstacle")
                    bm.CreateBlink(bm.blinkGreen, contact.point);

                rb.velocity = Vector3.zero;
                CreateExplosion(0.5f, contact.point);
            }
                
        }

        //if (gameObject.tag == "sunken")
        //{
        //    bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);
        //}
    }

    void OnCollisionExit(Collision collision)
    {
        // если обломок сталкивается с игроком - drag как сначала
        if (collision.gameObject.tag == "Player" && gameObject.tag == "sunken")
        {
            //gameObject.GetComponent<Rigidbody>().drag = initDrag;
            ChangeDrag(initDrag, true);
        }
    }
}
