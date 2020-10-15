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

    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        //CreatePoints();

        radius = maxRad;
        radShrink = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            ChangeRad();

            CreatePoints(radius);
            //ActivateMagnet();
            attract = true;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            attract = false;
            //Debug.Log("deactivate circle");
            CreatePoints(0);


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
