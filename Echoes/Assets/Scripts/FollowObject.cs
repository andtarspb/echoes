using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    GameObject objToFollow;
    
    // Update is called once per frame
    void Update()
    {
        if (objToFollow)
        {
            transform.position = objToFollow.transform.position;

            
        }
        
    }
}
