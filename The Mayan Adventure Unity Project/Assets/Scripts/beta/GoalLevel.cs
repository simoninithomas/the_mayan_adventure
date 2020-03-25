using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalLevel : MonoBehaviour
{

	public bool win;
	private AudioSource audio;
	public ParticleSystem particle;

	public GameObject winPanel;



    // Start is called before the first frame update
    void Start()
    {
        win = false;
        winPanel.SetActive(false);
        audio = GetComponent<AudioSource>();
    }

    public IEnumerator Win(bool win)
    {
    	if (win == true)
    	{
    		// Music and Prefab Fireworks
    		particle.Play();
            audio.Play();

        	// Wait 4 seconds
        	//WaitForSeconds.AudioSource.isPlaying();
        	yield return new WaitForSeconds(4f);

        	// Display the win panel
        	winPanel.SetActive(true);

            //WaitForSeconds.AudioSource.isPlaying();
            yield return new WaitForSeconds(4f);

            // Display the win panel
            particle.Stop();
            winPanel.SetActive(false);

    	}
    }
}
