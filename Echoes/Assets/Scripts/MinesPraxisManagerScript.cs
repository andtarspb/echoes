using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesPraxisManagerScript : MonoBehaviour
{
    [SerializeField]
    int minesCount;

    [SerializeField]
    GameObject praxisModule;

    AudioManager am;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    public void MinusMine()
    {
        minesCount--;
        if (minesCount == 0)
        {
            OnAllMinesDestroyed();
        }
    }

    void OnAllMinesDestroyed()
    {
        // play sound
        am.Play("blowup_gen");

        // give away praxis
        praxisModule.SetActive(true);
    }

    
}
