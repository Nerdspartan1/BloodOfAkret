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

	private Animator _anim;

	public float DistanceToWanderToGetLost = 100f;
	public float PyramidMovePeriod = 10f;
	public float PyramidToPlayerMoveDistance = 100f;
	public float PyramidMaxSpawnDistance = 1200f;

	private float _time;
	private bool _lostInDesert;
    private bool _intromusicStarted;

    public FMOD.Studio.EventInstance sandstormEvent;

    public FMOD.Studio.EventInstance intromusicEvent;

	private void Awake()
	{
		_time = 0f;
		_lostInDesert = false;
		_anim = GetComponent<Animator>();

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

        if (!_intromusicStarted && Vector3.Distance(Player.transform.position, this.gameObject.transform.position) > 35f)
        {
            intromusicEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.intro);
            intromusicEvent.start();
            _intromusicStarted = true;
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
