using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    PlayerController thePlayer;   


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    

    // Update is called once per frame
    void LateUpdate()
    {
        if (thePlayer)
        {
            transform.position = thePlayer.transform.position;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mine" || other.tag == "mine_boss" || other.tag == "rocket" || other.tag == "persuer")
        {
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy(false);

            gameObject.SetActive(false);
        }
    }
}
