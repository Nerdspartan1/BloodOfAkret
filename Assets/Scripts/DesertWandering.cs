﻿using System.Collections;
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
	public float PyramidMaxSpawnDistance = 1200f;

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
		if (!_lostInDesert && Vector3.Distance(Player.transform.position, Plane.transform.position) > DistanceToWanderToGetLost)
		{
			_lostInDesert = true;
			Plane.SetActive(false);
		}
			

		if (_lostInDesert)
		{
			if (_time > PyramidMovePeriod)
			{
				if(Vector3.Distance(Player.transform.position, Pyramid.transform.position) > PyramidToPlayerMoveDistance
					&& (Mathf.Abs(Player.transform.position.x) < PyramidMaxSpawnDistance
					&& Mathf.Abs(Player.transform.position.z) < PyramidMaxSpawnDistance) )
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
