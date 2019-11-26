﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidEntering : MonoBehaviour
{
	public GameObject Desert;
	public GameObject Player;
	public GameObject Door;
	public GameObject Pyramid;

    

	private bool _playerInPyramid = false;

	public void Start()
	{
		Door.SetActive(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(!_playerInPyramid && other.gameObject == Player)
		{
			var desertWandering = GameManager.Instance.Intro.GetComponent<DesertWandering>();
            desertWandering.sandstormEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            desertWandering.sandstormEvent.release();


            Desert.SetActive(false);
			Door.SetActive(true);
			Player.transform.SetParent(Pyramid.transform);
			Pyramid.transform.position = new Vector3(0f,4f,0f);
			Player.transform.SetParent(null);
			GameManager.Instance.Game.SetActive(true);


			_playerInPyramid = true;
		}
	}

	public void Update()
	{
		//if player drops into arena
		if (_playerInPyramid && Player.transform.position.y < -40f)
		{
			foreach (var weapon in Player.GetComponentsInChildren<vp_Weapon>())
			{
				weapon.SetState("Idle", false);
				weapon.GetComponent<vp_WeaponShooter>().enabled = true;
			}
			GameManager.Instance.Game.GetComponent<WaveManager>().StartGame();
			GameManager.Instance.Intro.SetActive(false);
		}
		
		
	}

}
