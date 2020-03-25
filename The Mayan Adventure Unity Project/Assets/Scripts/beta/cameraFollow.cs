using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
	// Thanks to amazing Brackeys tutorial
	public Transform agent;
	public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = agent.position + offset;
    }
}
