using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertWandering : MonoBehaviour
{
	public GameObject Player;
	public GameObject Sandstorm;
	public GameObject Pyramid;
	public GameObject Plane;

	private Animator _anim;

	public float PyramidToPlayerMoveDistance = 100f;
	public float PyramidMaxSpawnDistance = 1200f;

	private float _time;
	private bool _lostInDesert;
    private bool _musicStarted;

	private float _nextPyramidMoveTime;

    public FMOD.Studio.EventInstance sandstormEvent;

    public FMOD.Studio.EventInstance intromusicEvent;

	private void Awake()
	{
		_lostInDesert = false;
		_anim = GetComponent<Animator>();

	}

	public void Update()
	{

		if (!_musicStarted && Vector3.Distance(Player.transform.position, this.gameObject.transform.position) > 10f)
		{
			intromusicEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.intro);
			intromusicEvent.start();
			_musicStarted = true;
		}

		Sandstorm.transform.position = Player.transform.position + new Vector3(0, 3, -10);
		if (!_lostInDesert && Vector3.Distance(Player.transform.position, Plane.transform.position) > 150f)
		{
			_lostInDesert = true;
			Plane.SetActive(false);
		}
			

		if (_lostInDesert)
		{
			if (Time.time >= _nextPyramidMoveTime)
			{
				if(Vector3.Distance(Player.transform.position, Pyramid.transform.position) > PyramidToPlayerMoveDistance
					&& (Mathf.Abs(Player.transform.position.x) < PyramidMaxSpawnDistance
					&& Mathf.Abs(Player.transform.position.z) < PyramidMaxSpawnDistance) )
					MovePyramidInFrontOfPlayer();
				_nextPyramidMoveTime = Time.time + 10f;
			}
		}


	}

	public void MovePyramidInFrontOfPlayer()
	{
		Pyramid.transform.position = Player.transform.position + Player.transform.forward * PyramidToPlayerMoveDistance;
		Pyramid.transform.LookAt(Player.transform, Vector3.up);
	}


    public void PlayIntro()
	{
		sandstormEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.sandstorm);

		_anim.SetTrigger("play");
		Player.GetComponent<vp_FPController>().enabled = false;
		Player.GetComponent<vp_FPWeaponHandler>().enabled = false;
		Player.GetComponent<vp_FPInput>().enabled = false;
	}

	public void GiveControlToPlayer()
	{

        sandstormEvent.start();

		Player.GetComponent<vp_FPController>().enabled = true;
		Player.GetComponent<vp_FPWeaponHandler>().enabled = true;
		Player.GetComponent<vp_FPInput>().enabled = true;
		foreach(var weapon in Player.GetComponentsInChildren<vp_Weapon>())
		{
			weapon.SetState("Idle");
			weapon.GetComponent<vp_WeaponShooter>().enabled = false;
		}
		
		GameManager.Instance.CanPause = true;

	}
}
