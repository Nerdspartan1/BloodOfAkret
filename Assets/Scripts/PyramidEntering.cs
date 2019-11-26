using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidEntering : MonoBehaviour
{
	public GameObject Desert;
	public GameObject Player;
	public GameObject Door;
	public GameObject Pyramid;

    public GameObject StopSandstormEvent;

	public void Start()
	{
		Door.SetActive(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == Player)
		{
            

            StopSandstormEvent.GetComponent<DesertWandering>().sandstormEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            StopSandstormEvent.GetComponent<DesertWandering>().sandstormEvent.release();


            Desert.SetActive(false);
			Door.SetActive(true);
			Player.transform.SetParent(Pyramid.transform);
			Pyramid.transform.position = Vector3.zero;
			Player.transform.SetParent(null);
			GameManager.Instance.Game.SetActive(true);
			Player.GetComponentInChildren<vp_Weapon>().SetState("Idle", false);
			Player.GetComponentInChildren<vp_WeaponShooter>().enabled = true;
		}
	}

}
