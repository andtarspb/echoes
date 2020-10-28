using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoss : MonoBehaviour
{

    BossBatleManager bossMng;

    void Start()
    {
        bossMng = FindObjectOfType<BossBatleManager>();
    }


    public void LaunchNewRocket()
    {
        bossMng.LaunchNewRocket();
    }
}
