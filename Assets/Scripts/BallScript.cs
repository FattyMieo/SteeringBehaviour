using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
	[Header("Physics")]
	public float mass = 1.0f;
	public Vector2 velocity = Vector2.zero;
	public Vector2 acceleration = Vector2.zero;

	[Header("Repel Settings")]
	public float repelRadius = 0.5f;
	public List<Transform> nearbyBalls;
	private List<Vector2> externalForces = new List<Vector2>();
	public float repelAmplification = 5.0f;

	[Header("Other Settings")]
	public bool ignoreMass = false;
	public bool useFixedAcceleration = false;
	public float fixedAcceleration = 0.1f;
	public bool useCappedSpeed = false;
	public float maxSpeed = 3.0f;

	[Header("Extra Stuff")]
	public bool useSpecialEffect = false;
	public float minSize = 0.0001f;

	void Update()
	{
		if(Input.GetMouseButton(0)) //Resets the velocity & acceleration
		{
			velocity = Vector2.zero;
			acceleration = Vector2.zero;
		}
		if(Input.GetMouseButton(1)) //Resets the position to the gravity field
		{
			transform.position = SpawnManager.instance.gravField.position;
		}

		Vector2 dir = SpawnManager.instance.gravField.position - transform.position;

		CheckNearbyBalls();
		Vector2 externalForce = Vector2.zero;
		for(int i = 0; i < externalForces.Count; i++)
		{
			externalForce += externalForces[i] * repelAmplification;
		}

		float totalMag = 0.0f;

		if(useFixedAcceleration)
			totalMag = fixedAcceleration;
		else
			totalMag = (dir + externalForce).magnitude;
		
		AddForce2D(totalMag, (dir + externalForce).normalized);

		//Extra stuff
		if(useSpecialEffect)
		{
			transform.localScale = new Vector3(acceleration.sqrMagnitude + minSize, acceleration.sqrMagnitude + minSize, 1.0f) * mass;
		}
	}

	void FixedUpdate ()
	{
		velocity += acceleration;

		if(useCappedSpeed)
		{
			if(velocity.sqrMagnitude > maxSpeed * maxSpeed)
			{
				velocity = velocity.normalized * maxSpeed;
			}
		}
		
		transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
	}

	public void AddForce2D(float magnitude, Vector2 direction)
	{
		//Make sure the direction is normalized
		direction.Normalize();

		//F = ma
		acceleration = direction * magnitude;
		if(!ignoreMass) acceleration /= mass;
	}

	public void CheckNearbyBalls()
	{
		Transform[] ballList = SpawnManager.instance.ballList.ToArray();

		externalForces.Clear();
		//Debug
		nearbyBalls.Clear();

		for(int i = 0; i < ballList.Length; i++)
		{
			if(ballList[i] == this.transform) continue;

			Vector3 dir = transform.position - ballList[i].position;
			if(dir.sqrMagnitude <= repelRadius * repelRadius)
			{
				externalForces.Add((Vector2)dir);
				//Debug
				nearbyBalls.Add(ballList[i]);
			}
		}
	}
}
