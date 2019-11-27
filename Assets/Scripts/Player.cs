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
	private Collider _collider;
	
	public List<Perk> Perks;

	public vp_UnitType PistolAmmo;
	public vp_UnitBankType Rifle;
	public vp_UnitType RifleAmmo;
	public vp_UnitBankType Machinegun;
	public vp_UnitType MachinegunAmmo;
	private bool _hasRifle = false;
	private bool _hasMachinegun = false;
	
	public Rigidbody Head;
	public Camera FPSCamera;

	public Camera WeaponCamera;
	private int _nbOfCamera = 1;

	public GameObject GameOverScreen;

	private float _baseDamping;
	private float _baseJumpForce;

	private void Start()
	{


		_controller = GetComponent<vp_FPController>();
		_collider = GetComponent<Collider>();
		_input = GetComponent<vp_FPInput>();
		_weaponHandler = GetComponent<vp_FPWeaponHandler>();
		_baseDamping = _controller.MotorDamping;
		_baseJumpForce = _controller.MotorJumpForce;
		_inventory = GetComponent<vp_PlayerInventory>();
		_inventory.TryGiveUnits(PistolAmmo,int.MaxValue);
		Head.isKinematic = true;

	}

	public void AddMirroredCamera()
	{
		if (_nbOfCamera >= 4) return;

		var camObj = new GameObject();
		camObj.transform.parent = WeaponCamera.transform.parent;
		camObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		var cam = camObj.AddComponent<Camera>();
		cam.enabled = true;
		cam.CopyFrom(WeaponCamera);
		switch (_nbOfCamera++)
		{
			case 1:
				cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
				break;
			case 2:
				cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(1, -1, 1));
				break;
			case 3:
				cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, -1, 1));
				break;
		}
		
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.T)) WaveManager.Instance.Points += 10000;
		if (Input.GetKeyDown(KeyCode.C)) AddMirroredCamera();
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

		_collider.enabled = false;
		this.enabled = false;

		GameOverScreen.SetActive(true);
	}


	public void PerkUp(Perk perk)
	{
		Perks.Add(perk);

		var shooters = GetComponentsInChildren<vp_FPWeaponShooter>();

		switch (perk.Name)
		{
			case "Heal":
				CurrentHealth = MaxHealth;
				break;
			case "Grace of Bastet":
				_controller.MotorDamping *= 0.90f;
				break;
			case "Feathers of Nemty":
				_controller.MotorJumpForce *= 1.40f;
				break;

			case "Frenesy":
				foreach (var shooter in shooters)
				{
					shooter.ProjectileFiringRate *= 0.85f;
					shooter.ProjectileTapFiringRate *= 0.85f;
				}
				break;
			case "Mirror of Ptah":
				AddMirroredCamera();
				foreach (var shooter in shooters)
				{
					shooter.ProjectileCount += 1;
				}
				break;
			case "Weapon":
				if (!_hasRifle)
				{
					_inventory.TryGiveUnitBank(Rifle, Rifle.Capacity, 0);
					_inventory.TryGiveUnits(RifleAmmo, 8 * Rifle.Capacity);
					_hasRifle = true;
				}
				else
				{
					_inventory.SetUnitCount(RifleAmmo, 8 * Rifle.Capacity);
				}
				break;
			default:
				throw new System.Exception("Perk not recognized");
		}
		
		


	}


}
