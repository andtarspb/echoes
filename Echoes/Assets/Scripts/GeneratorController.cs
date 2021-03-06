﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour // генератор, который питает закрытую дверь и его нужно уничтожить
{
    [SerializeField]
    float explosForce;  // explosion force
    [SerializeField]
    float explosRadius; // explosion radius

    [SerializeField]
    GameObject genMng;
    BlinkManager bm;

    // for the blinking animation
    SpriteRenderer rend;
    [SerializeField]
    Sprite spriteYellow;
    Color spriteColor;
    bool fading = false;
    float duration = 0.5f;


    AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        //genMng = FindObjectOfType<GeneratorManager>();
        bm = FindObjectOfType<BlinkManager>();
        am = FindObjectOfType<AudioManager>();

        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteYellow;
        spriteColor = rend.color;
    }
    
    public void DestroyGenerator()
    {
        am.Play("blowup_gen");
        CreateExplosion(explosForce, explosRadius);
        HandleManager();    // tell the particular manager that we've destroyed one generator
        bm.CreateBlink(bm.blinkCircleOrange, transform.position);
        Destroy(gameObject);
    }

    void Update()
    {
        // чтобы не крутились во время битвы с боссом
        transform.rotation = Quaternion.identity;
    }

    void HandleManager()
    {
        if (genMng.GetComponent<GeneratorManager>())
        {
            genMng.GetComponent<GeneratorManager>().MinusGenerator();
        } else if (genMng.GetComponent<BossBatleManager>())
        {
            genMng.GetComponent<BossBatleManager>().MinusGenerator();
        }
    }

    void CreateExplosion(float explosionForce, float radiusOfExplosion)
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

    public void Fade(bool fadeIn)
    {
        if (fading)
        {
            return;
        }
        fading = false;

        StartCoroutine(FadeTo(fadeIn, duration));
    }

    IEnumerator FadeTo(bool fadeIn, float duration)
    {
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        //Enable MyRenderer component
        if (!rend.enabled)
            rend.enabled = true;

        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            //Debug.Log(alpha);

            rend.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            //Disable Mesh Renderer
            rend.enabled = false;

        }
        fading = false; //So that we can call this function next time

        StartCoroutine(FadeTo(!fadeIn, duration));
    }
}
