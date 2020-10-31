using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesToMinesPraxisManager : MonoBehaviour
{
    MinesPraxisManagerScript mpm;

    // Start is called before the first frame update
    void Start()
    {
        mpm = FindObjectOfType<MinesPraxisManagerScript>();
    }

    void OnDestroy()
    {
        mpm.MinusMine();
    }
}
