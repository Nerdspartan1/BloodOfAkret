using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertWandering : MonoBehaviour
{
	public GameObject Player;
	public GameObject DesertPlane;
	public GameObject Sandstorm;
	public GameObject Pyramid;
	public GameObject Plane;

	public float DistanceToWanderToGetLost = 100f;
	public float PyramidMovePeriod = 10f;
	public float PyramidToPlayerMoveDistance = 100f;

	private float _time;
	private bool _lostInDesert;

	void Start()
    {
		_time = 0f;
		_lostInDesert = false;
    }

	public void Update()
	{
		Sandstorm.transform.position = Player.transform.position + new Vector3(0, 3, -10);
		if (Vector3.Distance(Player.transform.position, Plane.transform.position) > DistanceToWanderToGetLost)
			_lostInDesert = true;

		if (_lostInDesert)
		{
			if (_time > PyramidMovePeriod)
			{
				if(Vector3.Distance(Player.transform.position, Pyramid.transform.position) > PyramidToPlayerMoveDistance)
					MovePyramidInFrontOfPlayer();
				_time = 0f;
			}
			_time += Time.deltaTime;
		}
	}


	public void MovePyramidInFrontOfPlayer()
	{
		Pyramid.transform.position = Player.transform.position + Player.transform.forward * PyramidToPlayerMoveDistance;
		Pyramid.transform.LookAt(Player.transform, Vector3.up);
	}
}
