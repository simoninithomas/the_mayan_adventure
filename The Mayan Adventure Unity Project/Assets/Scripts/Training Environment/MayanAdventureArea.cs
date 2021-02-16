using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MayanAdventureArea : MonoBehaviour
{
	public Rigidbody agentRb;
	public GameObject spawnAreaAgent;

	public GameObject goalGameObject;
    public Rigidbody goalRb;
    public GameObject spawnAreaGoal;

    // The empty game object that will contain the generated part of the level
    public GameObject levelGenerated;

    // Levels
    public GameObject bigWoodBridgeLevel;
    public GameObject woodBridgeLevel;
    public GameObject fireLevel;
    public GameObject goalLevel;
    
    // Will be used in the v2
    public GameObject rotateWoodBridgeLevel;
    
    public GameObject baseGeneration;
    public GameObject[] levels_array;

    // If training no animations are played or panel are activated
    public bool training;

	/// <summary>
	/// Reset the Area including the agent and goal placement
	/// </summary>
	public void ResetArea()
	{
    	// Spawn randomly the agent on the start level
		ResetPosition(agentRb, spawnAreaAgent);

		// Spawn randomly the goal on the goal level
		ResetPosition(goalRb, spawnAreaGoal);

		// Destroy the former area and generate the new one
        LevelGenerationv2();
	}

    /// <summary>
    /// Generate the level
    /// 1. Destroy the former generation
    /// 2. Select the level array based on the curricula
    /// 3. Instantiate the new area
    /// </summary>
    void LevelGenerationv2()
    {

        // 1. Destroy the former generation
        foreach (Transform child in levelGenerated.transform)
        {
            Destroy(child.gameObject);
        } 

        // 2. Select the level array based on the curricula
        float levelNumber = Academy.Instance.FloatProperties.GetPropertyWithDefault("level", 3.0f);
        GameObject[] levels_array = new GameObject[] {bigWoodBridgeLevel, fireLevel, woodBridgeLevel};
        float[] levels_size = new float[] {24.2f, 30f, 39f};

        /// 4. Instantiate the new area
        Vector3 baseGenerationVector3 = levelGenerated.transform.position + new Vector3(0,0,10);

        if (levelNumber == 0f)
        {
            return;
        }
        else
        {
            for (int level = 0; level <= levelNumber-1; level++)
            {
                GameObject lev = Instantiate(levels_array[level], baseGenerationVector3, Quaternion.identity);
                lev.transform.SetParent(levelGenerated.transform);
                baseGenerationVector3 = baseGenerationVector3 + new Vector3(0,0,levels_size[level]) ;
            }
            goalLevel.transform.position = baseGenerationVector3;
        }
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
            new Vector3(randomPosX, 3f, randomPosZ);
       
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
