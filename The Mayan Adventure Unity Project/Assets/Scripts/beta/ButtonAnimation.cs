using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
	private ParticleSystem particle;
	private AudioSource audio;

    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        audio = GetComponent<AudioSource>();
        audio.Stop();
      	particle.Stop();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "agent")
        {
            Debug.Log("Collision");
        	audio.Play();
        	particle.Play();
        }            
    }
}
