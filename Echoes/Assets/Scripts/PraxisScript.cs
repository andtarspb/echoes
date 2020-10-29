using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PraxisScript : MonoBehaviour
{
    SkillMenuScript skillManager;
    AudioManager am;


    // Start is called before the first frame update
    void Start()
    {
        skillManager = FindObjectOfType<SkillMenuScript>();
        am = FindObjectOfType<AudioManager>();

        //var foundSkillManager = FindObjectsOfType<SkillMenuScript>();
        //Debug.Log(foundSkillManager + " : " + foundSkillManager.Length);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<MenuManager>().DisplayPraxisTaken(true);

            skillManager.HandlePraxisModule(1);

            am.Play("praxis_taken");

            Destroy(gameObject);
        }

       
    }
}
