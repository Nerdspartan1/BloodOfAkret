using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidEntering : MonoBehaviour
{
	public GameObject Desert;
	public Collider PlayerCollider;
	public GameObject Door;

	public void Start()
	{
		Door.SetActive(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other == PlayerCollider)
		{
			Desert.SetActive(false);
			Door.SetActive(true);
		}
	}

}
