using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSystem : MonoBehaviour
{
    public bool isFireOn;

    public GameObject fire1On;
    public GameObject fire2On;
    public GameObject fire1Off;
    public GameObject fire2Off;

    public ParticleSystem fire1Particle;
    public ParticleSystem fire2Particle;
	
    private AudioSource audio;

    void Start()
    {
    	audio = GetComponent<AudioSource>();
        isFireOn = false;
        InvokeRepeating("TurnFireOnOff", 0f, 4f);
    }
    void TurnFireOnOff()
    {
        Debug.Log(isFireOn);
        if (isFireOn)
        {
        	//TODO Improve this part
            // Turn off the fire
            fire1Off.SetActive(true);
            fire2Off.SetActive(true);
            fire1On.SetActive(false);
            fire2On.SetActive(false);
            isFireOn = false;
        }
        else if (!isFireOn)
        {

            // Turn on the fire
            fire1Off.SetActive(false);
            fire2Off.SetActive(false);
            fire1On.SetActive(true);
            fire2On.SetActive(true);
            isFireOn = true;

            // Play the sound and particle
            fire1Particle.Play();
            fire2Particle.Play();
            audio.Play();
        }
    }
}
