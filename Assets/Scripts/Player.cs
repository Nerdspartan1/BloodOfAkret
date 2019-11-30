using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
	public Text HealthCounter;

	private float _baseDamping;
	private float _baseJumpForce;

	private float _mouseSensitivity = 5f;

	[HideInInspector]
	public vp_FPWeaponShooter[] _shooters;

	protected override void Awake()
	{
		_shooters = GetComponentsInChildren<vp_FPWeaponShooter>();
		base.Awake();
	}

	public void SetSensitivity(float sensitivity)
	{
		_mouseSensitivity = sensitivity;
		_input.MouseLookSensitivity = sensitivity * Vector2.one;
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
	}

	private void Start()
	{
		MaxHealth += PlayerPrefs.GetInt("bonusMaxHealth", 0);
		CurrentHealth = MaxHealth;

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
		if (Input.GetKeyDown(KeyCode.C)) PerkUp(new Perk { Name = "Weapon II" });

		HealthCounter.text = $"{CurrentHealth}";

		_input.MouseLookSensitivity = _mouseSensitivity * Vector2.one;

		if(transform.position.y < -70) //fell in void
		{
			Die();
		}
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

        WaveManager.Instance.ingamemusicEvent.setParameterByName("Death", 1f);

        GameOverScreen.SetActive(true);
	}


	public void PerkUp(Perk perk)
	{
		Perks.Add(perk);

		

		switch (perk.Name)
		{
			case "Heal":
				MaxHealth += 1;
				CurrentHealth = MaxHealth;
				break;
			case "Grace of Bastet":
				_controller.MotorDamping *= 0.90f;
				_controller.MotorJumpForce *= 1.10f;
				break;
			case "Grace of Bastet II":
				_controller.MotorDamping *= 0.80f;
				_controller.MotorJumpForce *= 1.20f;
				break;
			case "Grace of Bastet III":
				_controller.MotorDamping *= 0.70f;
				_controller.MotorJumpForce *= 1.30f;
				break;
			case "Frenesy":
				foreach (var shooter in _shooters)
				{
					shooter.ProjectileFiringRate *= 0.95f;
					shooter.ProjectileTapFiringRate *= 0.95f;
				}
				break;
			case "Frenesy II":
				foreach (var shooter in _shooters)
				{
					shooter.ProjectileFiringRate *= 0.90f;
					shooter.ProjectileTapFiringRate *= 0.90f;
				}
				break;
			case "Frenesy III":
				foreach (var shooter in _shooters)
				{
					shooter.ProjectileFiringRate *= 0.85f;
					shooter.ProjectileTapFiringRate *= 0.85f;
				}
				break;
			case "Mirror of Ptah":
				AddMirroredCamera();
				foreach (var shooter in _shooters)
				{
					shooter.ProjectileCount += 1;
				}
				break;
			case "Weapon":
				if (!_hasRifle)
				{
					_inventory.TryGiveUnitBank(Rifle, Rifle.Capacity, 0);
					_inventory.TryGiveUnits(RifleAmmo, 4 * Rifle.Capacity);
					_hasRifle = true;
				}
				else
				{
					_inventory.SetUnitCount(RifleAmmo, 5 * Rifle.Capacity);
				}
				break;
			case "Weapon II":
				if (!_hasMachinegun)
				{
					_inventory.TryGiveUnitBank(Machinegun, Machinegun.Capacity, 0);
					_inventory.TryGiveUnits(MachinegunAmmo, 4 * Machinegun.Capacity);
					_hasMachinegun = true;
				}
				else
				{
					_inventory.SetUnitCount(MachinegunAmmo, 5 * Machinegun.Capacity);
				}
				break;
			case "Blessing":
				PlayerPrefs.SetInt("bonusMaxHealth", PlayerPrefs.GetInt("bonusMaxHealth",0) + 5);
				break;
			default:
				throw new System.Exception("Perk not recognized");
		}
		
		


	}


}
