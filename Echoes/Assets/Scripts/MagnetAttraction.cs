using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAttraction : MonoBehaviour
{
    [SerializeField]
    float speed;

    PlayerController thePlayer;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    public void Attract()
    {
        transform.up = new Vector2(thePlayer.transform.position.x - transform.position.x, thePlayer.transform.position.y - transform.position.y);
        Vector3 force = transform.up * speed;
        rb.AddForce(force);
    }
    
}
