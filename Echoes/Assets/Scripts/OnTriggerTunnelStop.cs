using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerTunnelStop : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    GameObject triggerStop;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (transform.position.x <= triggerStop.transform.position.x)
        {
            triggerStop.gameObject.SetActive(false);
            audioManager.Play("radar_off");
            FindObjectOfType<TunnelEmittersManager>().DeactivateEmitters();
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "tunnel_stop")
    //    {
    //        Debug.Log("triggernulsa");
    //        other.gameObject.SetActive(false);
    //        audioManager.Play("radar_off");
    //        FindObjectOfType<TunnelEmittersManager>().DeactivateEmitters();
    //    }
    //}
}
