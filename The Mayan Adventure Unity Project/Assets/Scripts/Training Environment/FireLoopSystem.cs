using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLoopSystem : MonoBehaviour
{
    public bool isFireOn;
    public GameObject FireOn;
    public GameObject FireOff;

    void Start()
    {
        isFireOn = false;
        InvokeRepeating("TurnFireOnOff", 0f, 4f);
    }
    void TurnFireOnOff()
    {
        if (isFireOn)
        {
            // Turn off the fire
            FireOff.SetActive(true);
            FireOn.SetActive(false);
            isFireOn = false;
        }
        else if (!isFireOn)
        {

            // Turn on the fire
            FireOff.SetActive(false);
            FireOn.SetActive(true);
            isFireOn = true;
        }
    }
}
