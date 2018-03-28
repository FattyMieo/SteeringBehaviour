using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
	//================================================================================
	// Variables
	//================================================================================

	[Header("Physics")]
	public float   mass							= 1.0f;
	public Vector2 velocity						= Vector2.zero;
	public Vector2 acceleration					= Vector2.zero;

	[Header("Repelling Force")]
	public float ballRepelRadius				= 0.5f;
	public float ballRepelAmplification			= 5.0f;
	public float obstacleRepelRadius			= 0.5f;
	public float obstacleRepelAmplification		= 5.0f;
	public List<Transform> nearbyObjects;

	private List<Vector2> externalForces		= new List<Vector2>();

	[Header("Other Settings")]
	public bool  ignoreMass						= false;
	public bool  useFixedAcceleration			= false;
	public float fixedAcceleration				= 0.1f;
	public bool  useCappedSpeed					= false;
	public float maxSpeed						= 3.0f;

	[Header("Special Effects")]
	public bool  useSpecialEffect				= false;
	public float minScale						= 0.0001f;

	//================================================================================
	// Listeners
	//================================================================================

	void Update()
	{
		//================================================================================
		// * Mouse Inputs
		//================================================================================

		if(Input.GetMouseButton(0))
		{
			// Resets the velocity & acceleration
			velocity = Vector2.zero;
			acceleration = Vector2.zero;
		}

		if(Input.GetMouseButton(1))
		{
			// Resets the position to the gravity field
			transform.position = SpawnManager.instance.gravField.position;
		}

		//================================================================================
		// * Calculate Forces
		//================================================================================

		// Get direction towards cursor
		Vector2 dir = SpawnManager.instance.gravField.position - transform.position;

		// Get all external repelling forces
		Vector2 externalForce = Vector2.zero;
		CheckNearbyObjects();
		for(int i = 0; i < externalForces.Count; i++)
		{
			externalForce += externalForces[i];
		}

		// Calculate total magnitude of force
		float totalMag = (dir + externalForce).magnitude;
		if(useFixedAcceleration) totalMag = fixedAcceleration;

		// Calculate direction of force
		// Do "dir.normalized" to prioritise on external forces
		Vector2 totalDir = dir.normalized + externalForce;

		// Add force to the ball
		AddForce2D(totalMag, totalDir.normalized);

		//================================================================================
		// * Special Effects
		//================================================================================

		if(useSpecialEffect)
		{
			transform.localScale = new Vector3(acceleration.sqrMagnitude + minScale, acceleration.sqrMagnitude + minScale, 1.0f) * mass;
		}
	}

	void FixedUpdate ()
	{
		//================================================================================
		// * Update Physics
		//================================================================================

		// Update Velocity
		velocity += acceleration;

		if(useCappedSpeed)
		{
			// Limit the speed within range 
			if(velocity.sqrMagnitude > maxSpeed * maxSpeed)
			{
				velocity = velocity.normalized * maxSpeed;
			}
		}

		// Update Position
		transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
	}

	//================================================================================
	// Utility Functions
	//================================================================================

	public void AddForce2D(float magnitude, Vector2 direction)
	{
		// Make sure the direction is normalized
		direction.Normalize();

		// Calculate acceleration (F = ma)
		acceleration = direction * magnitude;
		if(!ignoreMass) acceleration /= mass;
	}

	//================================================================================
	// Update Functions
	//================================================================================

	public void CheckNearbyObjects()
	{
		// Get array from SpawnManager
		Transform[] ballList = SpawnManager.instance.ballList.ToArray();
		Transform[] obstacleList = SpawnManager.instance.obstacleList.ToArray();

		// Clear saved lists
		externalForces.Clear();
		nearbyObjects.Clear();

		for(int i = 0; i < ballList.Length; i++)
		{
			if(ballList[i] == this.transform) continue;

			// Add repelling force to list if ball is within radius
			Vector3 dir = transform.position - ballList[i].position;
			float totalRepelRadius = ballRepelRadius + (transform.localScale.x / 2.0f) + (ballList[i].localScale.x / 2.0f);
			if(dir.sqrMagnitude <= totalRepelRadius * totalRepelRadius)
			{
				externalForces.Add((Vector2)dir * ballRepelAmplification);
				nearbyObjects.Add(ballList[i]);
			}
		}

		for(int i = 0; i < obstacleList.Length; i++)
		{
			// Add repelling force to list if obstacle is within radius
			Vector3 dir = transform.position - obstacleList[i].position;
			float totalRepelRadius = obstacleRepelRadius + (transform.localScale.x / 2.0f) + (obstacleList[i].localScale.x / 2.0f);
			if(dir.sqrMagnitude <= totalRepelRadius * totalRepelRadius)
			{
				externalForces.Add((Vector2)dir * obstacleRepelAmplification);
				nearbyObjects.Add(obstacleList[i]);
			}
		}
	}
}
