using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodBridgeAnimation : MonoBehaviour
{
	private Animator anim;
	private AudioSource audio;

    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void BreakWoodBridgeAnimation()
    {
        
        anim.Play("Scene");
        audio.Play();
    }
}
