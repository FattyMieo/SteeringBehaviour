using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private static SpawnManager _instance;

	public static SpawnManager instance
	{
		get { return _instance; }
	}

	[Header("Settings")]
	public int amount = 50;
	public Vector2 minMaxMass = new Vector2(250.0f, 2000.0f);

	public List<Transform> ballList;

	[Header("Prefabs")]
	public GameObject ballPrefab;

	[Header("Connections")]
	public Transform gravField;

	void Awake ()
	{
		if(_instance == null) _instance = this;
		else if(_instance != this) Destroy(gameObject);
	}

	void Start ()
	{
		for(int i = 0; i < amount; i++)
		{
			Vector3 randVec = new Vector3(Random.Range(0.0f, Screen.width), Random.Range(0.0f, Screen.height), 0.0f);
			randVec = Camera.main.ScreenToWorldPoint(randVec);
			randVec.z = 0.0f;

			GameObject newBall = Instantiate(ballPrefab, randVec, Quaternion.identity, this.transform);
			newBall.name = ballPrefab.name + " " + i.ToString("000");
			newBall.GetComponent<BallScript>().mass = Random.Range(minMaxMass.x, minMaxMass.y);
			ballList.Add(newBall.transform);
		}
	}
}
