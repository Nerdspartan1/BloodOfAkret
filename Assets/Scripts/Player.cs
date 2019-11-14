using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Perk
{
	DemonLegs,
}

public class Player : vp_PlayerDamageHandler
{
	private vp_FPController _controller;
	private vp_PlayerInventory _inventory;
	public List<Perk> Perks;

	public vp_UnitType InfiniteAmmo;

	private float _baseDamping;
	private float _baseJumpForce;

	private void Start()
	{
		_controller = GetComponent<vp_FPController>();
		_baseDamping = _controller.MotorDamping;
		_baseJumpForce = _controller.MotorJumpForce;
		_inventory = GetComponent<vp_PlayerInventory>();
		_inventory.TryGiveUnits(InfiniteAmmo,int.MaxValue);

	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.T)) PerkUp(Perk.DemonLegs);
	}


	public void PerkUp(Perk perk)
	{
		Perks.Add(perk);

		int dl = Perks.Count(p => p == Perk.DemonLegs);
		_controller.MotorDamping = _baseDamping*Mathf.Pow(1f-0.25f,dl);
		_controller.MotorJumpForce = _baseJumpForce * Mathf.Pow(1.25f, dl);
	}


}
