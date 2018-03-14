using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldControl : MonoBehaviour
{
	void Update ()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0.0f;
		transform.position = pos;
	}
}
