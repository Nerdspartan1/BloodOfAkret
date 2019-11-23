using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : vp_FPPlayerDamageHandler
{
	private vp_FPController _controller;
	private vp_FPInput _input;
	private vp_PlayerInventory _inventory;
	private vp_FPWeaponHandler _weaponHandler;
	
	public List<Perk> Perks;

	public vp_UnitType InfiniteAmmo;
	public Rigidbody Head;
	public Camera FPSCamera;

	public GameObject GameOverScreen;

	private float _baseDamping;
	private float _baseJumpForce;

	private void Start()
	{
		_controller = GetComponent<vp_FPController>();
		_input = GetComponent<vp_FPInput>();
		_weaponHandler = GetComponent<vp_FPWeaponHandler>();
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

		_input.MouseCursorForced = true;
		_weaponHandler.enabled = false;

		this.enabled = false;

		GameOverScreen.SetActive(true);
	}


	public void PerkUp(Perk perk)
	{
		Perks.Add(perk);

		switch (perk.Name)
		{
			case "Grace of Bastet":
				_controller.MotorDamping *= 0.75f;
				break;
			case "Feathers of Nemty":
				_controller.MotorJumpForce *= 1.4f;
				break;

			case "Frenesy of Montu":
				var shooters = GetComponentsInChildren<vp_FPWeaponShooter>();
				foreach (var shooter in shooters)
				{
					shooter.ProjectileFiringRate *= 0.7f;
					shooter.ProjectileTapFiringRate *= 0.7f;
				}
				break;

		}
		
		


	}


}
