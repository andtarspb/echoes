﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSpriteWithDelay : PoolObject
{
    // PUBLIC INIT
    public float lifeTime;

    // PRIVATE INIT
    bool fading = false;
    SpriteRenderer rend;
    Color spriteColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        spriteColor = rend.color;
        rend.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        //Fade(false, lifeTime);
    }

    public override void OnObjectReuse()
    {
        rend.enabled = true;
        rend.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);

        Fade(false, lifeTime);
    }

    void Fade(bool fadeIn, float duration)
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}