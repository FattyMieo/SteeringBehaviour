using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	//================================================================================
	// Singleton
	//================================================================================

	private static SpawnManager _instance;

	public static SpawnManager instance
	{
		get { return _instance; }
	}

	//================================================================================
	// Variables
	//================================================================================

	[Header("Settings")]
	public int amount					= 50;
	public Vector2 minMaxMass			= new Vector2(250.0f, 2000.0f);

	[Header("Tracked Objects")]
	public string ballTag;
	public string obstacleTag;
	public List<Transform> ballList;
	public List<Transform> obstacleList;

	[Header("Prefabs")]
	public GameObject ballPrefab;

	[Header("Connections")]
	public Transform gravField;

	//================================================================================
	// Listeners
	//================================================================================

	void Awake ()
	{
		//================================================================================
		// * Singleton Declaration
		//================================================================================

		if(_instance == null) _instance = this;
		else if(_instance != this) Destroy(gameObject);
	}

	void Start ()
	{
		//================================================================================
		// * Finding Pre-exist Objects
		//================================================================================

		GameObject[] pBalls = GameObject.FindGameObjectsWithTag(ballTag);

		for(int i = 0; i < pBalls.Length; i++)
		{
			ballList.Add(pBalls[i].transform);
		}

		GameObject[] pObstacles = GameObject.FindGameObjectsWithTag(obstacleTag);

		for(int i = 0; i < pObstacles.Length; i++)
		{
			obstacleList.Add(pObstacles[i].transform);
		}

		//================================================================================
		// * Spawning
		//================================================================================

		for(int i = 0; i < amount; i++)
		{
			// Generate a random point on the screen
			Vector3 randVec = new Vector3(Random.Range(0.0f, Screen.width), Random.Range(0.0f, Screen.height), 0.0f);
			randVec = Camera.main.ScreenToWorldPoint(randVec);
			randVec.z = 0.0f;

			// Instantiate the ball
			GameObject newBall = Instantiate(ballPrefab, randVec, Quaternion.identity, this.transform);
			newBall.name = ballPrefab.name + " " + i.ToString("000");
			newBall.GetComponent<BallScript>().mass = Random.Range(minMaxMass.x, minMaxMass.y);
			ballList.Add(newBall.transform);
		}
	}
}
