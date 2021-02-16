using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class MayanAdventureAgent : Agent
{
    MayanAdventureSettings mayanAdventureSettings;

	public string physics;
    public Material WoodMaterial;
    public Material RockMaterial;

    public bool isRock;

    public GameObject goalLevel;
    private GameObject goal;

	public MayanAdventureArea area;
	Rigidbody rigidbody;

    public GameObject gameOverPanel;


	/// <summary>
	/// This function is called once when the agent wakes
	/// </summary>
    public override void InitializeAgent()
    {
        mayanAdventureSettings = FindObjectOfType<MayanAdventureSettings>();
    	base.InitializeAgent();
    	rigidbody = GetComponent<Rigidbody>();
    	goal = area.goalGameObject;
    }

    /// <summary>
	/// Perform actions based on a vector of numbers
	/// </summary>
	/// <param name="vectorAction">The list of actions to take</param>
	public void MoveAgent(float[] act)
	{
    	var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = (int)act[0];
        var rightAxis = (int)act[1];
        var rotateAxis = (int)act[2];

            switch (forwardAxis)
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    dirToGo = -transform.forward;
                    break;
            }

            switch (rightAxis)
            {
                case 1:
                    dirToGo = transform.right;
                    break;
                case 2:
                    dirToGo = -transform.right;
                    break;
            }

            switch (rotateAxis)
            {
                case 1:
                    rotateDir = -transform.up;
                    break;
                case 2:
                    rotateDir = transform.up;
                    break;
            }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * mayanAdventureSettings.agentRotateSpeed);
        rigidbody.AddForce(dirToGo * mayanAdventureSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    public override void AgentAction(float[] vectorAction)
    {
        // Apply a tiny negative reward every step to encourage action
        AddReward(-1f / maxStep);
        MoveAgent(vectorAction);

        if (area.training)
        {
            // If fell of the platform (-2 neg reward)
            if(this.transform.position.y < 1.68f)
            {
                SetReward(-1f);
                Done();
            }
        }
        else if (area.training == false)
        {
            // If fell of the platform (-2 neg reward)
            if(this.transform.position.y < -5)
            {
                StartCoroutine(GameOver());
            }
        }
    }
    /// <summary>   
    /// Collect the observations except ray casts.
    /// local velocity
    /// isRock Bool
    /// </summary>
    public override void CollectObservations()
    {
        var localVelocity = transform.InverseTransformDirection(this.rigidbody.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
        AddVectorObs(isRock);
    }

    /// <summary>   
    /// Read inputs from the keyboard and convert them to a list of actions.
    /// This is called only when the player wants to control the agent and has set
    /// Behavior Type to "Heuristic Only" in the Behavior Parameters inspector.
    /// </summary>
    /// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
    public override float[] Heuristic()
    {
        var action = new float[4];
        if (Input.GetKey(KeyCode.D))
        {
            action[2] = 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action[2] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2f;
        }
        return action;
    }

    /// <summary>   
    /// Reset the agent and the area
    /// </summary>
	public override void AgentReset()
    {
    	area.ResetArea();
    	isRock = true;
    	GetComponent<Renderer>().material = RockMaterial;
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "gem")
        {
            AddReward(1f);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "goal")
        {
            if (area.training)
            {
                SetReward(1f);
                Done();
            }
            if (area.training == false)
            {
                goal.SetActive(false);
                StartCoroutine (goalLevel.GetComponent<GoalLevel>().Win(true));
                StartCoroutine (ResetAfterWin());
            }
        }
        if (collision.gameObject.tag == "ToWoodButton")
        {
            // Change the material color
            GetComponent<Renderer>().material = WoodMaterial;
            isRock = false;
        }
        if (collision.gameObject.tag == "ToRockButton")
        {
            // Change the material color
            GetComponent<Renderer>().material = RockMaterial;
            isRock = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("woodBridge") && isRock)
        {
            if (area.training)
            {
                SetReward(-1f);
                Done();
            }
                
            if (area.training == false)
            {
                // Turn off the box collider to fall the agent
                collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                // Animation
                collision.gameObject.GetComponent<woodBridgeAnimation>().BreakWoodBridgeAnimation();
                
                // Set reward and done will be handled by agent.y < 2.0
                StartCoroutine (FallingBridge());
            }
        }
        if (collision.gameObject.CompareTag("fireOn") && !isRock)
        {
            if (area.training)
            {
                SetReward(-1f);
                Done();
            }
            if (area.training == false)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    /// <summary>   
    /// Wait 8 seconds before reset after win
    /// </summary>
    public IEnumerator ResetAfterWin(){
            yield return new WaitForSeconds(8f);
            SetReward(1f);
            goal.SetActive(true);
            Done();
    }

    /// <summary>   
    /// Let the wood bridge destroy animation run for 3 seconds before reset
    /// </summary>
     public IEnumerator FallingBridge(){
            yield return new WaitForSeconds(3f);
            StartCoroutine(GameOver());
    }
    
    /// <summary>   
    /// Handle the gameover situation in replay

    /// </summary>
    public IEnumerator GameOver()
    {
        // Display the game over panel
        gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        gameOverPanel.SetActive(false);

        this.gameObject.SetActive(false);
        SetReward(-1f);
        Done();
    }   
}
