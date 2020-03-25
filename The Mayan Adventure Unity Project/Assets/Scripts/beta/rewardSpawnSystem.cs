using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewardSpawnSystem : MonoBehaviour
{
	public Rigidbody gemRb;
	public GameObject spawnAreaGem;

    // Start is called before the first frame update
    void Start()
    {
     	ResetPosition(gemRb, spawnAreaGem);   
    }

        /// <summary>
    /// Gets a random spawn position in the spawningArea.
    /// </summary>
    /// <returns>The random spawn position.</returns>
    public Vector3 GetRandomSpawnPos(GameObject spawnArea)
    {
        // Get the bounds
        Bounds spawnAreaBounds = spawnArea.GetComponent<Collider>().bounds;
        
        // Select the x position
        var randomPosX = Random.Range(spawnAreaBounds.min.x,
            spawnAreaBounds.max.x);

        
        // Select the y position
        var randomPosZ = Random.Range(spawnAreaBounds.min.z,
            spawnAreaBounds.max.z);
       
        // Spawn position

        Vector3 randomSpawnPos = 
            new Vector3(randomPosX, 2.15f, randomPosZ);
        //Debug.Log(randomSpawnPos);
       
        return randomSpawnPos;
    }

    // Reset the position
    public void ResetPosition(Rigidbody Rb, GameObject spawnArea)
    {
        spawnArea.SetActive(true);
        
        Rb.transform.position = GetRandomSpawnPos(spawnArea);
        Rb.velocity = Vector3.zero;

        spawnArea.SetActive(false);
    }
}
