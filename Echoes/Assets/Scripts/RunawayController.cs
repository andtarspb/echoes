﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayController : EnemyController
{
    // PUBLIC INIT
    public float thrust;

    public Vector3[] localWaypoints;

    // PRIVATE INIR
    int nextWaypoint;
    Vector3[] globalWaypoints;


    // Start is called before the first frame update
    void Start()
    {
        nextWaypoint = 0;

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
