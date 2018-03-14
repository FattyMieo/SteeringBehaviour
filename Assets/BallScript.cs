using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
	public Transform gravField;

	public float mass = 1.0f;
	public Vector2 velocity = Vector2.zero;
	public Vector2 acceleration = Vector2.zero;

	void Update()
	{
		if(Input.GetMouseButton(0)) //Resets the velocity & acceleration
		{
			velocity = Vector2.zero;
			acceleration = Vector2.zero;
		}
		if(Input.GetMouseButton(1)) //Resets the position to the gravity field
		{
			transform.position = gravField.position;
		}

		Vector2 dir = gravField.position - transform.position;
		AddForce2D(dir.magnitude, dir.normalized);
	}

	void FixedUpdate ()
	{
		velocity += acceleration;
		transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
	}

	public void AddForce2D(float magnitude, Vector2 direction)
	{
		//Make sure the direction is normalized
		direction.Normalize();

		//F = ma
		acceleration = direction * magnitude / mass;
	}
}
