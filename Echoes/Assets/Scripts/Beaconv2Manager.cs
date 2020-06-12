﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaconv2Manager : MonoBehaviour
{
    [SerializeField]
    Beaconv2Controller[] beacons;   // массив маячков
    
    int requiredAmmount;            // сколько нужно активировать маячков для открытия двери
    int counter;                    // счетчик активированных маячков

    public bool puzzleSolved;

    [SerializeField]
    DoorController theDoor;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        requiredAmmount = CountActivationBeacons(beacons);

        beacons = SetIDs(beacons);
    }

    int CountActivationBeacons(Beaconv2Controller[] bb)
    {
        int numberOfActivationBeacons = 0;

        for (int i = 0; i < bb.Length; i++)
        {
            if (bb[i].activationBeacon)
            {
                numberOfActivationBeacons++;
            }
        }

        return numberOfActivationBeacons;
    }
    
    Beaconv2Controller[] SetIDs(Beaconv2Controller[] bb)
    {
        for (int i = 0; i < bb.Length; i++)
        {
            bb[i].id = i;
        }

        return bb;
    }

    public void HandleActivation(int id)
    {
        if (beacons[id].activationBeacon) // если это активационный маячек (!)
        {
            DeactivateXs();
            counter++;
            if (counter == requiredAmmount)
            {
                PuzzleSolved();
            }
        }
        else // если это деактивационный маячек (x)
        {
            DeactivateBeacons(id);
        }
    }

    void DeactivateXs()
    {
        for (int i = 0; i < beacons.Length; i++)
        {
            if (!beacons[i].activationBeacon)
            {
                beacons[i].DeactivateBeacon();
            }
        }
    }

    void DeactivateBeacons(int id) // деактивирем все блинки кроме последнего активного
    {
        if (!puzzleSolved)
        {
            counter = 0;

            for (int i = 0; i < beacons.Length; i++)
            {
                if (i != id)
                {
                    beacons[i].DeactivateBeacon();
                }
            }
        }        
    }

    void PuzzleSolved()
    {
        //Debug.Log("Puzzle solved");

        puzzleSolved = true;

        theDoor.OpenTheDoor();
    }
}