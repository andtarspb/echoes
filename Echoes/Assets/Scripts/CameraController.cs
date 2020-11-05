using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // private init
    Vector3 targetPosition;
    PlayerController followTarget;

    public bool isFollowing;
    
    void Start()
    {
        isFollowing = true;
        followTarget = FindObjectOfType<PlayerController>();
    }

    void LateUpdate()
    {
        if (isFollowing)
        {
            targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, followTarget.transform.position.z - 10);
            transform.position = targetPosition;
        }
        else
        {
            if (gameObject.transform.childCount > 0)
            {
                //Debug.Log("Дочерних объектов: " + gameObject.transform.childCount);
                GameObject child = gameObject.transform.GetChild(0).gameObject;
                child.transform.parent = null;
                child.GetComponent<FollowObject>().enabled = true;
            }
        }
        
    }
}
