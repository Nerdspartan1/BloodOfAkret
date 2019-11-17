using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : vp_FPPlayerDamageHandler
{
	private vp_FPController _controller;
	private vp_PlayerInventory _inventory;
	
	public List<Perk> Perks;

	public vp_UnitType InfiniteAmmo;
	public Rigidbody Head;
	public Camera FPSCamera;

	private float _baseDamping;
	private float _baseJumpForce;

	private void Start()
	{
		_controller = GetComponent<vp_FPController>();
		_baseDamping = _controller.MotorDamping;
		_baseJumpForce = _controller.MotorJumpForce;
		_inventory = GetComponent<vp_PlayerInventory>();
		_inventory.TryGiveUnits(InfiniteAmmo,int.MaxValue);
		Head.isKinematic = true;

	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.T)) WaveManager.Instance.Points += 10000;
	}

	public override void Die()
	{
		base.Die();
		Head.transform.parent = null;
		Head.transform.position = FPSCamera.transform.position;
		Head.isKinematic = false;
		FPSCamera.GetComponent<vp_FPCamera>().enabled = false;
		FPSCamera.transform.parent = Head.transform;
	}


	public void PerkUp(Perk perk)
	{
		Perks.Add(perk);

		int dl = Perks.Count(p => p.Name == "Demon Legs");
		_controller.MotorDamping = _baseDamping*Mathf.Pow(1f-0.25f,dl);
		_controller.MotorJumpForce = _baseJumpForce * Mathf.Pow(1.25f, dl);
	}


}
