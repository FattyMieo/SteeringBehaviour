using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldControl : MonoBehaviour
{
	//================================================================================
	// Listeners
	//================================================================================

	void Update ()
	{
		// Update position to the cursor
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0.0f;
		transform.position = pos;
	}
}
